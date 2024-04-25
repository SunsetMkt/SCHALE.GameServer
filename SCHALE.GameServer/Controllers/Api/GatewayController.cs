﻿using Microsoft.AspNetCore.Mvc;
using SCHALE.Common.Crypto;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Controllers.Api.ProtocolHandlers;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SCHALE.GameServer.Controllers.Api
{
    [Route("/api/[controller]")]
    public class GatewayController : ControllerBase
    {
        IProtocolHandlerFactory protocolHandlerFactory;
        ILogger<GatewayController> logger;

        public GatewayController(IProtocolHandlerFactory _protocolHandlerFactory, ILogger<GatewayController> _logger)
        {
            protocolHandlerFactory = _protocolHandlerFactory;
            logger = _logger;
        }

        [HttpPost]
        public IResult GatewayRequest()
        {
            var formFile = Request.Form.Files.GetFile("mx");
            if (formFile is null)
                return Results.BadRequest("Expecting an mx file");

            using var reader = new BinaryReader(formFile.OpenReadStream());

            // CRC + Protocol type conversion + payload length
            reader.BaseStream.Position = 12;

            byte[] compressedPayload = reader.ReadBytes((int)reader.BaseStream.Length - 12);
            XOR.Crypt(compressedPayload, [0xD9]);
            using var gzStream = new GZipStream(new MemoryStream(compressedPayload), CompressionMode.Decompress);
            using var payloadMs = new MemoryStream();
            gzStream.CopyTo(payloadMs);

            try
            {
                var payloadStr = Encoding.UTF8.GetString(payloadMs.ToArray());
                var jsonNode = JsonSerializer.Deserialize<JsonNode>(payloadStr);
                var protocol = (Protocol?)jsonNode?["Protocol"]?.GetValue<int?>() ?? Protocol.None;
                if (protocol == Protocol.None)
                {
                    logger.LogWarning("Failed to read protocol from JsonNode, {Payload:j}", payloadStr);
                    goto protocolErrorRet;
                }

                var requestType = protocolHandlerFactory.GetRequestPacketTypeByProtocol(protocol);
                if (requestType is null)
                {
                    logger.LogError("Protocol {Protocol} doesn't have corresponding type registered", protocol);
                    goto protocolErrorRet;
                }

                var payload = (JsonSerializer.Deserialize(payloadStr, requestType) as RequestPacket)!;

                var rsp = protocolHandlerFactory.Invoke(protocol, [payload]);
                if (rsp is null)
                {
                    logger.LogDebug("{Protocol} {Payload:j}", payload.Protocol, payloadStr);
                    logger.LogError("Protocol {Protocol} is unimplemented and left unhandled", payload.Protocol);

                    goto protocolErrorRet;
                }
                
                return Results.Json(new
                {
                    packet = JsonSerializer.Serialize(rsp),
                    protocol = payload.Protocol.ToString()
                });

protocolErrorRet:
                return Results.Json(new
                {
                    packet = JsonSerializer.Serialize(new ErrorPacket() { Reason = "Protocol not implemented (Server Error)", ErrorCode = WebAPIErrorCode.InternalServerError }),
                    protocol = Protocol.Error.ToString()
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
