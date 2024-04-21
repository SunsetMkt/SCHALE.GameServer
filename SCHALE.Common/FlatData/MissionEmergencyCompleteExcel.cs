// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace SCHALE.Common.FlatData
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct MissionEmergencyCompleteExcel : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_24_3_25(); }
  public static MissionEmergencyCompleteExcel GetRootAsMissionEmergencyCompleteExcel(ByteBuffer _bb) { return GetRootAsMissionEmergencyCompleteExcel(_bb, new MissionEmergencyCompleteExcel()); }
  public static MissionEmergencyCompleteExcel GetRootAsMissionEmergencyCompleteExcel(ByteBuffer _bb, MissionEmergencyCompleteExcel obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public MissionEmergencyCompleteExcel __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public long MissionId { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }
  public bool EmergencyComplete { get { int o = __p.__offset(6); return o != 0 ? 0!=__p.bb.Get(o + __p.bb_pos) : (bool)false; } }

  public static Offset<SCHALE.Common.FlatData.MissionEmergencyCompleteExcel> CreateMissionEmergencyCompleteExcel(FlatBufferBuilder builder,
      long MissionId = 0,
      bool EmergencyComplete = false) {
    builder.StartTable(2);
    MissionEmergencyCompleteExcel.AddMissionId(builder, MissionId);
    MissionEmergencyCompleteExcel.AddEmergencyComplete(builder, EmergencyComplete);
    return MissionEmergencyCompleteExcel.EndMissionEmergencyCompleteExcel(builder);
  }

  public static void StartMissionEmergencyCompleteExcel(FlatBufferBuilder builder) { builder.StartTable(2); }
  public static void AddMissionId(FlatBufferBuilder builder, long missionId) { builder.AddLong(0, missionId, 0); }
  public static void AddEmergencyComplete(FlatBufferBuilder builder, bool emergencyComplete) { builder.AddBool(1, emergencyComplete, false); }
  public static Offset<SCHALE.Common.FlatData.MissionEmergencyCompleteExcel> EndMissionEmergencyCompleteExcel(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<SCHALE.Common.FlatData.MissionEmergencyCompleteExcel>(o);
  }
}


static public class MissionEmergencyCompleteExcelVerify
{
  static public bool Verify(Google.FlatBuffers.Verifier verifier, uint tablePos)
  {
    return verifier.VerifyTableStart(tablePos)
      && verifier.VerifyField(tablePos, 4 /*MissionId*/, 8 /*long*/, 8, false)
      && verifier.VerifyField(tablePos, 6 /*EmergencyComplete*/, 1 /*bool*/, 1, false)
      && verifier.VerifyTableEnd(tablePos);
  }
}

}