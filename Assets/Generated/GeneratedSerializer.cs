using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using alpoLib.Core.Serialization;
using UnityEngine;

namespace alpoLib.Data.Serialization.Generated {
public sealed record __r_BoardDefineBase_1226491235_Wrapped : MergeBoard.Data.Table.BoardDefineBase {
public static SchemeDefinition[] __schema__ = {
new() { NameHash = 3615872654, TypeHash = 3327941314 }, /* String Name */
new() { NameHash = 3839824690, TypeHash = 869011071 }, /* BoardType BoardType */
new() { NameHash = 3095585490, TypeHash = 3327941314 }, /* String NameStringKey */
new() { NameHash = 2293390633, TypeHash = 2513291021 }, /* BGMKey BGMKey */
new() { NameHash = 733356138, TypeHash = 3333469098 }, /* CustomBoolean IsDefault */
new() { NameHash = 4214230326, TypeHash = 1798982289 }, /* Int32 Id */
new() { NameHash = 3871067742, TypeHash = 3333469098 }, /* CustomBoolean IsActive */
/* Definition Count: 7 */
};
public System.String __r_Name_Ovr { set { Name = value; } }
public MergeBoard.Data.BoardType __r_BoardType_Ovr { set { BoardType = value; } }
public System.String __r_NameStringKey_Ovr { set { NameStringKey = value; } }
public MergeBoard.Sound.BGMKey __r_BGMKey_Ovr { set { BGMKey = value; } }
public alpoLib.Core.Foundation.CustomBoolean __r_IsDefault_Ovr { set { IsDefault = value; } }
public System.Int32 __r_Id_Ovr { set { Id = value; } }
public alpoLib.Core.Foundation.CustomBoolean __r_IsActive_Ovr { set { IsActive = value; } }
}
public sealed class __r_BoardDefineBase_1226491235_Serializer : SerializerBase<MergeBoard.Data.Table.BoardDefineBase> {
public override SchemeDefinition[] GetSchemeDefinitions() { return __r_BoardDefineBase_1226491235_Wrapped.__schema__; }
public override MergeBoard.Data.Table.BoardDefineBase Deserialize(BufferStream stream) {
var da = new __r_BoardDefineBase_1226491235_Wrapped();
da.__r_Name_Ovr = stream.ReadStr();
da.__r_BoardType_Ovr = (MergeBoard.Data.BoardType)stream.ReadS32(); // ENUM
da.__r_NameStringKey_Ovr = stream.ReadStr();
da.__r_BGMKey_Ovr = (MergeBoard.Sound.BGMKey)stream.ReadS32(); // ENUM
da.__r_IsDefault_Ovr = stream.ReadCustomBoolean();
da.__r_Id_Ovr = stream.ReadS32();
da.__r_IsActive_Ovr = stream.ReadCustomBoolean();
return da; }
public override void Serialize(BufferStream stream, MergeBoard.Data.Table.BoardDefineBase da) {
stream.WriteStr(da.Name);
stream.WriteS32((System.Int32)da.BoardType); // ENUM
stream.WriteStr(da.NameStringKey);
stream.WriteS32((System.Int32)da.BGMKey); // ENUM
stream.WriteCustomBoolean(da.IsDefault);
stream.WriteS32(da.Id);
stream.WriteCustomBoolean(da.IsActive);
}
public override MergeBoard.Data.Table.BoardDefineBase JsonToObject(JToken token) {
var da = new MergeBoard.Data.Table.BoardDefineBase();
var __BGMKey_converter__ = TypeDescriptor.GetConverter(typeof(MergeBoard.Sound.BGMKey));
da.BGMKey = (MergeBoard.Sound.BGMKey)__BGMKey_converter__.ConvertFrom((String)token["BGMKey"]);
var __BoardType_converter__ = TypeDescriptor.GetConverter(typeof(MergeBoard.Data.BoardType));
da.BoardType = (MergeBoard.Data.BoardType)__BoardType_converter__.ConvertFrom((String)token["BoardType"]);
da.Id = token["Id"] != null ? token["Id"].ToObject<System.Int32>() : default;
da.IsActive = token["IsActive"] != null ? token["IsActive"].ToObject<alpoLib.Core.Foundation.CustomBoolean>() : default;
da.IsDefault = token["IsDefault"] != null ? token["IsDefault"].ToObject<alpoLib.Core.Foundation.CustomBoolean>() : default;
da.Name = token["Name"] != null ? token["Name"].ToObject<System.String>() : default;
da.NameStringKey = token["NameStringKey"] != null ? token["NameStringKey"].ToObject<System.String>() : default;
return da; }
public override void ObjectToJson(JToken token, MergeBoard.Data.Table.BoardDefineBase da) {
}
}
public sealed record __r_BoardInitialDataBase_375919288_Wrapped : MergeBoard.Data.Table.BoardInitialDataBase {
public static SchemeDefinition[] __schema__ = {
new() { NameHash = 4285929551, TypeHash = 1798982289 }, /* Int32 BoardId */
new() { NameHash = 3904163235, TypeHash = 1798982289 }, /* Int32 X */
new() { NameHash = 3052880830, TypeHash = 1798982289 }, /* Int32 Y */
new() { NameHash = 2267539208, TypeHash = 1798982289 }, /* Int32 ItemId */
new() { NameHash = 4214230326, TypeHash = 1798982289 }, /* Int32 Id */
new() { NameHash = 3871067742, TypeHash = 3333469098 }, /* CustomBoolean IsActive */
/* Definition Count: 6 */
};
public System.Int32 __r_BoardId_Ovr { set { BoardId = value; } }
public System.Int32 __r_X_Ovr { set { X = value; } }
public System.Int32 __r_Y_Ovr { set { Y = value; } }
public System.Int32 __r_ItemId_Ovr { set { ItemId = value; } }
public System.Int32 __r_Id_Ovr { set { Id = value; } }
public alpoLib.Core.Foundation.CustomBoolean __r_IsActive_Ovr { set { IsActive = value; } }
}
public sealed class __r_BoardInitialDataBase_375919288_Serializer : SerializerBase<MergeBoard.Data.Table.BoardInitialDataBase> {
public override SchemeDefinition[] GetSchemeDefinitions() { return __r_BoardInitialDataBase_375919288_Wrapped.__schema__; }
public override MergeBoard.Data.Table.BoardInitialDataBase Deserialize(BufferStream stream) {
var da = new __r_BoardInitialDataBase_375919288_Wrapped();
da.__r_BoardId_Ovr = stream.ReadS32();
da.__r_X_Ovr = stream.ReadS32();
da.__r_Y_Ovr = stream.ReadS32();
da.__r_ItemId_Ovr = stream.ReadS32();
da.__r_Id_Ovr = stream.ReadS32();
da.__r_IsActive_Ovr = stream.ReadCustomBoolean();
return da; }
public override void Serialize(BufferStream stream, MergeBoard.Data.Table.BoardInitialDataBase da) {
stream.WriteS32(da.BoardId);
stream.WriteS32(da.X);
stream.WriteS32(da.Y);
stream.WriteS32(da.ItemId);
stream.WriteS32(da.Id);
stream.WriteCustomBoolean(da.IsActive);
}
public override MergeBoard.Data.Table.BoardInitialDataBase JsonToObject(JToken token) {
var da = new MergeBoard.Data.Table.BoardInitialDataBase();
da.BoardId = token["BoardId"] != null ? token["BoardId"].ToObject<System.Int32>() : default;
da.Id = token["Id"] != null ? token["Id"].ToObject<System.Int32>() : default;
da.IsActive = token["IsActive"] != null ? token["IsActive"].ToObject<alpoLib.Core.Foundation.CustomBoolean>() : default;
da.ItemId = token["ItemId"] != null ? token["ItemId"].ToObject<System.Int32>() : default;
da.X = token["X"] != null ? token["X"].ToObject<System.Int32>() : default;
da.Y = token["Y"] != null ? token["Y"].ToObject<System.Int32>() : default;
return da; }
public override void ObjectToJson(JToken token, MergeBoard.Data.Table.BoardInitialDataBase da) {
}
}
public sealed record __r_ItemBase_1229283027_Wrapped : MergeBoard.Data.Table.ItemBase {
public static SchemeDefinition[] __schema__ = {
new() { NameHash = 2489168048, TypeHash = 3327941314 }, /* String Category */
new() { NameHash = 2543258826, TypeHash = 1798982289 }, /* Int32 Sequence */
new() { NameHash = 3467706342, TypeHash = 1798982289 }, /* Int32 Energy */
new() { NameHash = 2813828439, TypeHash = 1798982289 }, /* Int32 MergeToSeq */
new() { NameHash = 1490168075, TypeHash = 3333469098 }, /* CustomBoolean CanPop */
new() { NameHash = 1472935463, TypeHash = 1798982289 }, /* Int32 PopCount */
new() { NameHash = 942986764, TypeHash = 1798982289 }, /* Int32 PopCooltime */
new() { NameHash = 3372987824, TypeHash = 1798982289 }, /* Int32 SellValue */
new() { NameHash = 1369144902, TypeHash = 3327941314 }, /* String AtlasName */
new() { NameHash = 2923162350, TypeHash = 3327941314 }, /* String SpriteName */
new() { NameHash = 4214230326, TypeHash = 1798982289 }, /* Int32 Id */
new() { NameHash = 3871067742, TypeHash = 3333469098 }, /* CustomBoolean IsActive */
/* Definition Count: 12 */
};
public System.String __r_Category_Ovr { set { Category = value; } }
public System.Int32 __r_Sequence_Ovr { set { Sequence = value; } }
public System.Int32 __r_Energy_Ovr { set { Energy = value; } }
public System.Int32 __r_MergeToSeq_Ovr { set { MergeToSeq = value; } }
public alpoLib.Core.Foundation.CustomBoolean __r_CanPop_Ovr { set { CanPop = value; } }
public System.Int32 __r_PopCount_Ovr { set { PopCount = value; } }
public System.Int32 __r_PopCooltime_Ovr { set { PopCooltime = value; } }
public System.Int32 __r_SellValue_Ovr { set { SellValue = value; } }
public System.String __r_AtlasName_Ovr { set { AtlasName = value; } }
public System.String __r_SpriteName_Ovr { set { SpriteName = value; } }
public System.Int32 __r_Id_Ovr { set { Id = value; } }
public alpoLib.Core.Foundation.CustomBoolean __r_IsActive_Ovr { set { IsActive = value; } }
}
public sealed class __r_ItemBase_1229283027_Serializer : SerializerBase<MergeBoard.Data.Table.ItemBase> {
public override SchemeDefinition[] GetSchemeDefinitions() { return __r_ItemBase_1229283027_Wrapped.__schema__; }
public override MergeBoard.Data.Table.ItemBase Deserialize(BufferStream stream) {
var da = new __r_ItemBase_1229283027_Wrapped();
da.__r_Category_Ovr = stream.ReadStr();
da.__r_Sequence_Ovr = stream.ReadS32();
da.__r_Energy_Ovr = stream.ReadS32();
da.__r_MergeToSeq_Ovr = stream.ReadS32();
da.__r_CanPop_Ovr = stream.ReadCustomBoolean();
da.__r_PopCount_Ovr = stream.ReadS32();
da.__r_PopCooltime_Ovr = stream.ReadS32();
da.__r_SellValue_Ovr = stream.ReadS32();
da.__r_AtlasName_Ovr = stream.ReadStr();
da.__r_SpriteName_Ovr = stream.ReadStr();
da.__r_Id_Ovr = stream.ReadS32();
da.__r_IsActive_Ovr = stream.ReadCustomBoolean();
return da; }
public override void Serialize(BufferStream stream, MergeBoard.Data.Table.ItemBase da) {
stream.WriteStr(da.Category);
stream.WriteS32(da.Sequence);
stream.WriteS32(da.Energy);
stream.WriteS32(da.MergeToSeq);
stream.WriteCustomBoolean(da.CanPop);
stream.WriteS32(da.PopCount);
stream.WriteS32(da.PopCooltime);
stream.WriteS32(da.SellValue);
stream.WriteStr(da.AtlasName);
stream.WriteStr(da.SpriteName);
stream.WriteS32(da.Id);
stream.WriteCustomBoolean(da.IsActive);
}
public override MergeBoard.Data.Table.ItemBase JsonToObject(JToken token) {
var da = new MergeBoard.Data.Table.ItemBase();
da.AtlasName = token["AtlasName"] != null ? token["AtlasName"].ToObject<System.String>() : default;
da.CanPop = token["CanPop"] != null ? token["CanPop"].ToObject<alpoLib.Core.Foundation.CustomBoolean>() : default;
da.Category = token["Category"] != null ? token["Category"].ToObject<System.String>() : default;
da.Energy = token["Energy"] != null ? token["Energy"].ToObject<System.Int32>() : default;
da.Id = token["Id"] != null ? token["Id"].ToObject<System.Int32>() : default;
da.IsActive = token["IsActive"] != null ? token["IsActive"].ToObject<alpoLib.Core.Foundation.CustomBoolean>() : default;
da.MergeToSeq = token["MergeToSeq"] != null ? token["MergeToSeq"].ToObject<System.Int32>() : default;
da.PopCooltime = token["PopCooltime"] != null ? token["PopCooltime"].ToObject<System.Int32>() : default;
da.PopCount = token["PopCount"] != null ? token["PopCount"].ToObject<System.Int32>() : default;
da.SellValue = token["SellValue"] != null ? token["SellValue"].ToObject<System.Int32>() : default;
da.Sequence = token["Sequence"] != null ? token["Sequence"].ToObject<System.Int32>() : default;
da.SpriteName = token["SpriteName"] != null ? token["SpriteName"].ToObject<System.String>() : default;
return da; }
public override void ObjectToJson(JToken token, MergeBoard.Data.Table.ItemBase da) {
}
}
public sealed record __r_LevelBase_1753856995_Wrapped : MergeBoard.Data.Table.LevelBase {
public static SchemeDefinition[] __schema__ = {
new() { NameHash = 1499963410, TypeHash = 1798982289 }, /* Int32 Level */
new() { NameHash = 3723260675, TypeHash = 1798982289 }, /* Int32 MaxExp */
new() { NameHash = 4214230326, TypeHash = 1798982289 }, /* Int32 Id */
new() { NameHash = 3871067742, TypeHash = 3333469098 }, /* CustomBoolean IsActive */
/* Definition Count: 4 */
};
public System.Int32 __r_Level_Ovr { set { Level = value; } }
public System.Int32 __r_MaxExp_Ovr { set { MaxExp = value; } }
public System.Int32 __r_Id_Ovr { set { Id = value; } }
public alpoLib.Core.Foundation.CustomBoolean __r_IsActive_Ovr { set { IsActive = value; } }
}
public sealed class __r_LevelBase_1753856995_Serializer : SerializerBase<MergeBoard.Data.Table.LevelBase> {
public override SchemeDefinition[] GetSchemeDefinitions() { return __r_LevelBase_1753856995_Wrapped.__schema__; }
public override MergeBoard.Data.Table.LevelBase Deserialize(BufferStream stream) {
var da = new __r_LevelBase_1753856995_Wrapped();
da.__r_Level_Ovr = stream.ReadS32();
da.__r_MaxExp_Ovr = stream.ReadS32();
da.__r_Id_Ovr = stream.ReadS32();
da.__r_IsActive_Ovr = stream.ReadCustomBoolean();
return da; }
public override void Serialize(BufferStream stream, MergeBoard.Data.Table.LevelBase da) {
stream.WriteS32(da.Level);
stream.WriteS32(da.MaxExp);
stream.WriteS32(da.Id);
stream.WriteCustomBoolean(da.IsActive);
}
public override MergeBoard.Data.Table.LevelBase JsonToObject(JToken token) {
var da = new MergeBoard.Data.Table.LevelBase();
da.Id = token["Id"] != null ? token["Id"].ToObject<System.Int32>() : default;
da.IsActive = token["IsActive"] != null ? token["IsActive"].ToObject<alpoLib.Core.Foundation.CustomBoolean>() : default;
da.Level = token["Level"] != null ? token["Level"].ToObject<System.Int32>() : default;
da.MaxExp = token["MaxExp"] != null ? token["MaxExp"].ToObject<System.Int32>() : default;
return da; }
public override void ObjectToJson(JToken token, MergeBoard.Data.Table.LevelBase da) {
}
}
public sealed record __r_PopProbabilityBase_2220820701_Wrapped : MergeBoard.Data.Table.PopProbabilityBase {
public static SchemeDefinition[] __schema__ = {
new() { NameHash = 3042431834, TypeHash = 3442900740 }, /* PopType PopType */
new() { NameHash = 2267539208, TypeHash = 1798982289 }, /* Int32 ItemId */
new() { NameHash = 507102812, TypeHash = 1798982289 }, /* Int32 PopItemId */
new() { NameHash = 2478329637, TypeHash = 1798982289 }, /* Int32 Probability */
new() { NameHash = 4214230326, TypeHash = 1798982289 }, /* Int32 Id */
new() { NameHash = 3871067742, TypeHash = 3333469098 }, /* CustomBoolean IsActive */
/* Definition Count: 6 */
};
public MergeBoard.Data.PopType __r_PopType_Ovr { set { PopType = value; } }
public System.Int32 __r_ItemId_Ovr { set { ItemId = value; } }
public System.Int32 __r_PopItemId_Ovr { set { PopItemId = value; } }
public System.Int32 __r_Probability_Ovr { set { Probability = value; } }
public System.Int32 __r_Id_Ovr { set { Id = value; } }
public alpoLib.Core.Foundation.CustomBoolean __r_IsActive_Ovr { set { IsActive = value; } }
}
public sealed class __r_PopProbabilityBase_2220820701_Serializer : SerializerBase<MergeBoard.Data.Table.PopProbabilityBase> {
public override SchemeDefinition[] GetSchemeDefinitions() { return __r_PopProbabilityBase_2220820701_Wrapped.__schema__; }
public override MergeBoard.Data.Table.PopProbabilityBase Deserialize(BufferStream stream) {
var da = new __r_PopProbabilityBase_2220820701_Wrapped();
da.__r_PopType_Ovr = (MergeBoard.Data.PopType)stream.ReadS32(); // ENUM
da.__r_ItemId_Ovr = stream.ReadS32();
da.__r_PopItemId_Ovr = stream.ReadS32();
da.__r_Probability_Ovr = stream.ReadS32();
da.__r_Id_Ovr = stream.ReadS32();
da.__r_IsActive_Ovr = stream.ReadCustomBoolean();
return da; }
public override void Serialize(BufferStream stream, MergeBoard.Data.Table.PopProbabilityBase da) {
stream.WriteS32((System.Int32)da.PopType); // ENUM
stream.WriteS32(da.ItemId);
stream.WriteS32(da.PopItemId);
stream.WriteS32(da.Probability);
stream.WriteS32(da.Id);
stream.WriteCustomBoolean(da.IsActive);
}
public override MergeBoard.Data.Table.PopProbabilityBase JsonToObject(JToken token) {
var da = new MergeBoard.Data.Table.PopProbabilityBase();
da.Id = token["Id"] != null ? token["Id"].ToObject<System.Int32>() : default;
da.IsActive = token["IsActive"] != null ? token["IsActive"].ToObject<alpoLib.Core.Foundation.CustomBoolean>() : default;
da.ItemId = token["ItemId"] != null ? token["ItemId"].ToObject<System.Int32>() : default;
da.PopItemId = token["PopItemId"] != null ? token["PopItemId"].ToObject<System.Int32>() : default;
var __PopType_converter__ = TypeDescriptor.GetConverter(typeof(MergeBoard.Data.PopType));
da.PopType = (MergeBoard.Data.PopType)__PopType_converter__.ConvertFrom((String)token["PopType"]);
da.Probability = token["Probability"] != null ? token["Probability"].ToObject<System.Int32>() : default;
return da; }
public override void ObjectToJson(JToken token, MergeBoard.Data.Table.PopProbabilityBase da) {
}
}
public sealed record __r_QuestConditionBase_1430477122_Wrapped : MergeBoard.Data.Table.QuestConditionBase {
public static SchemeDefinition[] __schema__ = {
new() { NameHash = 718830648, TypeHash = 1798982289 }, /* Int32 QuestId */
new() { NameHash = 1096459174, TypeHash = 3327941314 }, /* (Compound) String Condition.ConditionType */
new() { NameHash = 4255764946, TypeHash = 1798982289 }, /* (Compound) Int32 Condition.ConditionId */
new() { NameHash = 182750618, TypeHash = 1798982289 }, /* (Compound) Int32 Condition.ConditionValue */
new() { NameHash = 1619518505, TypeHash = 4266003888 }, /* QuestCondition Condition */
new() { NameHash = 4214230326, TypeHash = 1798982289 }, /* Int32 Id */
new() { NameHash = 3871067742, TypeHash = 3333469098 }, /* CustomBoolean IsActive */
/* Definition Count: 7 */
};
public System.Int32 __r_QuestId_Ovr { set { QuestId = value; } }
public MergeBoard.Data.Table.QuestCondition __r_Condition_Ovr { set { Condition = value; } }
public System.Int32 __r_Id_Ovr { set { Id = value; } }
public alpoLib.Core.Foundation.CustomBoolean __r_IsActive_Ovr { set { IsActive = value; } }
}
public sealed class __r_QuestConditionBase_1430477122_Serializer : SerializerBase<MergeBoard.Data.Table.QuestConditionBase> {
public override SchemeDefinition[] GetSchemeDefinitions() { return __r_QuestConditionBase_1430477122_Wrapped.__schema__; }
public override MergeBoard.Data.Table.QuestConditionBase Deserialize(BufferStream stream) {
var da = new __r_QuestConditionBase_1430477122_Wrapped();
da.__r_QuestId_Ovr = stream.ReadS32();
da.__r_Condition_Ovr = DeserializeComp<MergeBoard.Data.Table.QuestCondition, __r_QuestCondition_567522278_Serializer>(stream);
da.__r_Id_Ovr = stream.ReadS32();
da.__r_IsActive_Ovr = stream.ReadCustomBoolean();
return da; }
public override void Serialize(BufferStream stream, MergeBoard.Data.Table.QuestConditionBase da) {
stream.WriteS32(da.QuestId);
SerializeComp<MergeBoard.Data.Table.QuestCondition, __r_QuestCondition_567522278_Serializer>(stream, da.Condition);
stream.WriteS32(da.Id);
stream.WriteCustomBoolean(da.IsActive);
}
public override MergeBoard.Data.Table.QuestConditionBase JsonToObject(JToken token) {
var da = new MergeBoard.Data.Table.QuestConditionBase();
da.Condition = new MergeBoard.Data.Table.QuestCondition {
ConditionType = token["ConditionType"] != null ? token["ConditionType"].ToObject<System.String>() : default,
ConditionId = token["ConditionId"] != null ? token["ConditionId"].ToObject<System.Int32>() : default,
ConditionValue = token["ConditionValue"] != null ? token["ConditionValue"].ToObject<System.Int32>() : default,
};
da.Id = token["Id"] != null ? token["Id"].ToObject<System.Int32>() : default;
da.IsActive = token["IsActive"] != null ? token["IsActive"].ToObject<alpoLib.Core.Foundation.CustomBoolean>() : default;
da.QuestId = token["QuestId"] != null ? token["QuestId"].ToObject<System.Int32>() : default;
return da; }
public override void ObjectToJson(JToken token, MergeBoard.Data.Table.QuestConditionBase da) {
}
}
public sealed record __r_QuestCondition_567522278_Wrapped : MergeBoard.Data.Table.QuestCondition {
public static SchemeDefinition[] __schema__ = {
new() { NameHash = 1096459174, TypeHash = 3327941314 }, /* String ConditionType */
new() { NameHash = 4255764946, TypeHash = 1798982289 }, /* Int32 ConditionId */
new() { NameHash = 182750618, TypeHash = 1798982289 }, /* Int32 ConditionValue */
/* Definition Count: 3 */
};
public System.String __r_ConditionType_Ovr { set { ConditionType = value; } }
public System.Int32 __r_ConditionId_Ovr { set { ConditionId = value; } }
public System.Int32 __r_ConditionValue_Ovr { set { ConditionValue = value; } }
}
public sealed class __r_QuestCondition_567522278_Serializer : SerializerBase<MergeBoard.Data.Table.QuestCondition> {
public override SchemeDefinition[] GetSchemeDefinitions() { return __r_QuestCondition_567522278_Wrapped.__schema__; }
public override MergeBoard.Data.Table.QuestCondition Deserialize(BufferStream stream) {
var da = new __r_QuestCondition_567522278_Wrapped();
da.__r_ConditionType_Ovr = stream.ReadStr();
da.__r_ConditionId_Ovr = stream.ReadS32();
da.__r_ConditionValue_Ovr = stream.ReadS32();
return da; }
public override void Serialize(BufferStream stream, MergeBoard.Data.Table.QuestCondition da) {
stream.WriteStr(da.ConditionType);
stream.WriteS32(da.ConditionId);
stream.WriteS32(da.ConditionValue);
}
public override MergeBoard.Data.Table.QuestCondition JsonToObject(JToken token) {
var da = new MergeBoard.Data.Table.QuestCondition();
return da; }
public override void ObjectToJson(JToken token, MergeBoard.Data.Table.QuestCondition da) {
}
}
public sealed record __r_QuestDefineBase_84062391_Wrapped : MergeBoard.Data.Table.QuestDefineBase {
public static SchemeDefinition[] __schema__ = {
new() { NameHash = 3844141154, TypeHash = 3327941314 }, /* String Key */
new() { NameHash = 4285929551, TypeHash = 1798982289 }, /* Int32 BoardId */
new() { NameHash = 4214230326, TypeHash = 1798982289 }, /* Int32 Id */
new() { NameHash = 3871067742, TypeHash = 3333469098 }, /* CustomBoolean IsActive */
/* Definition Count: 4 */
};
public System.String __r_Key_Ovr { set { Key = value; } }
public System.Int32 __r_BoardId_Ovr { set { BoardId = value; } }
public System.Int32 __r_Id_Ovr { set { Id = value; } }
public alpoLib.Core.Foundation.CustomBoolean __r_IsActive_Ovr { set { IsActive = value; } }
}
public sealed class __r_QuestDefineBase_84062391_Serializer : SerializerBase<MergeBoard.Data.Table.QuestDefineBase> {
public override SchemeDefinition[] GetSchemeDefinitions() { return __r_QuestDefineBase_84062391_Wrapped.__schema__; }
public override MergeBoard.Data.Table.QuestDefineBase Deserialize(BufferStream stream) {
var da = new __r_QuestDefineBase_84062391_Wrapped();
da.__r_Key_Ovr = stream.ReadStr();
da.__r_BoardId_Ovr = stream.ReadS32();
da.__r_Id_Ovr = stream.ReadS32();
da.__r_IsActive_Ovr = stream.ReadCustomBoolean();
return da; }
public override void Serialize(BufferStream stream, MergeBoard.Data.Table.QuestDefineBase da) {
stream.WriteStr(da.Key);
stream.WriteS32(da.BoardId);
stream.WriteS32(da.Id);
stream.WriteCustomBoolean(da.IsActive);
}
public override MergeBoard.Data.Table.QuestDefineBase JsonToObject(JToken token) {
var da = new MergeBoard.Data.Table.QuestDefineBase();
da.BoardId = token["BoardId"] != null ? token["BoardId"].ToObject<System.Int32>() : default;
da.Id = token["Id"] != null ? token["Id"].ToObject<System.Int32>() : default;
da.IsActive = token["IsActive"] != null ? token["IsActive"].ToObject<alpoLib.Core.Foundation.CustomBoolean>() : default;
da.Key = token["Key"] != null ? token["Key"].ToObject<System.String>() : default;
return da; }
public override void ObjectToJson(JToken token, MergeBoard.Data.Table.QuestDefineBase da) {
}
}
public sealed record __r_QuestRewardBase_1524181191_Wrapped : MergeBoard.Data.Table.QuestRewardBase {
public static SchemeDefinition[] __schema__ = {
new() { NameHash = 718830648, TypeHash = 1798982289 }, /* Int32 QuestId */
new() { NameHash = 1946898862, TypeHash = 2627367613 }, /* Reward Reward */
new() { NameHash = 4214230326, TypeHash = 1798982289 }, /* Int32 Id */
new() { NameHash = 3871067742, TypeHash = 3333469098 }, /* CustomBoolean IsActive */
/* Definition Count: 4 */
};
public System.Int32 __r_QuestId_Ovr { set { QuestId = value; } }
public MergeBoard.Data.Reward __r_Reward_Ovr { set { Reward = value; } }
public System.Int32 __r_Id_Ovr { set { Id = value; } }
public alpoLib.Core.Foundation.CustomBoolean __r_IsActive_Ovr { set { IsActive = value; } }
}
public sealed class __r_QuestRewardBase_1524181191_Serializer : SerializerBase<MergeBoard.Data.Table.QuestRewardBase> {
public override SchemeDefinition[] GetSchemeDefinitions() { return __r_QuestRewardBase_1524181191_Wrapped.__schema__; }
public override MergeBoard.Data.Table.QuestRewardBase Deserialize(BufferStream stream) {
var da = new __r_QuestRewardBase_1524181191_Wrapped();
da.__r_QuestId_Ovr = stream.ReadS32();
da.__r_Reward_Ovr = DeserializeComp<MergeBoard.Data.Reward, __r_Reward_4056358923_Serializer>(stream);
da.__r_Id_Ovr = stream.ReadS32();
da.__r_IsActive_Ovr = stream.ReadCustomBoolean();
return da; }
public override void Serialize(BufferStream stream, MergeBoard.Data.Table.QuestRewardBase da) {
stream.WriteS32(da.QuestId);
SerializeComp<MergeBoard.Data.Reward, __r_Reward_4056358923_Serializer>(stream, da.Reward);
stream.WriteS32(da.Id);
stream.WriteCustomBoolean(da.IsActive);
}
public override MergeBoard.Data.Table.QuestRewardBase JsonToObject(JToken token) {
var da = new MergeBoard.Data.Table.QuestRewardBase();
da.Id = token["Id"] != null ? token["Id"].ToObject<System.Int32>() : default;
da.IsActive = token["IsActive"] != null ? token["IsActive"].ToObject<alpoLib.Core.Foundation.CustomBoolean>() : default;
da.QuestId = token["QuestId"] != null ? token["QuestId"].ToObject<System.Int32>() : default;
da.Reward = new MergeBoard.Data.Reward {
};
return da; }
public override void ObjectToJson(JToken token, MergeBoard.Data.Table.QuestRewardBase da) {
}
}
public sealed record __r_Reward_4056358923_Wrapped : MergeBoard.Data.Reward {
public static SchemeDefinition[] __schema__ = {
new() { NameHash = 4002822995, TypeHash = 692989292 }, /* RewardType Type */
new() { NameHash = 4214230326, TypeHash = 1798982289 }, /* Int32 Id */
new() { NameHash = 3367774595, TypeHash = 1798982289 }, /* Int32 Value */
/* Definition Count: 3 */
};
public MergeBoard.Data.RewardType __r_Type_Ovr { set { Type = value; } }
public System.Int32 __r_Id_Ovr { set { Id = value; } }
public System.Int32 __r_Value_Ovr { set { Value = value; } }
}
public sealed class __r_Reward_4056358923_Serializer : SerializerBase<MergeBoard.Data.Reward> {
public override SchemeDefinition[] GetSchemeDefinitions() { return __r_Reward_4056358923_Wrapped.__schema__; }
public override MergeBoard.Data.Reward Deserialize(BufferStream stream) {
var da = new __r_Reward_4056358923_Wrapped();
da.__r_Type_Ovr = (MergeBoard.Data.RewardType)stream.ReadS32(); // ENUM
da.__r_Id_Ovr = stream.ReadS32();
da.__r_Value_Ovr = stream.ReadS32();
return da; }
public override void Serialize(BufferStream stream, MergeBoard.Data.Reward da) {
stream.WriteS32((System.Int32)da.Type); // ENUM
stream.WriteS32(da.Id);
stream.WriteS32(da.Value);
}
public override MergeBoard.Data.Reward JsonToObject(JToken token) {
var da = new MergeBoard.Data.Reward();
da.Id = token["RewardId"] != null ? token["RewardId"].ToObject<System.Int32>() : default;
var __Type_converter__ = TypeDescriptor.GetConverter(typeof(MergeBoard.Data.RewardType));
da.Type = (MergeBoard.Data.RewardType)__Type_converter__.ConvertFrom((String)token["RewardType"]);
da.Value = token["RewardValue"] != null ? token["RewardValue"].ToObject<System.Int32>() : default;
return da; }
public override void ObjectToJson(JToken token, MergeBoard.Data.Reward da) {
}
}
public static partial class GeneratedSerializerFactory {
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
public static void RegisterSerializers() { SerializerFactory.RegisterSerializers(ob); }
static Dictionary<Type, ISerializerBase> ob = new() {
{ typeof(MergeBoard.Data.Table.BoardDefineBase), new __r_BoardDefineBase_1226491235_Serializer() },
{ typeof(MergeBoard.Data.Table.BoardInitialDataBase), new __r_BoardInitialDataBase_375919288_Serializer() },
{ typeof(MergeBoard.Data.Table.ItemBase), new __r_ItemBase_1229283027_Serializer() },
{ typeof(MergeBoard.Data.Table.LevelBase), new __r_LevelBase_1753856995_Serializer() },
{ typeof(MergeBoard.Data.Table.PopProbabilityBase), new __r_PopProbabilityBase_2220820701_Serializer() },
{ typeof(MergeBoard.Data.Table.QuestConditionBase), new __r_QuestConditionBase_1430477122_Serializer() },
{ typeof(MergeBoard.Data.Table.QuestCondition), new __r_QuestCondition_567522278_Serializer() },
{ typeof(MergeBoard.Data.Table.QuestDefineBase), new __r_QuestDefineBase_84062391_Serializer() },
{ typeof(MergeBoard.Data.Table.QuestRewardBase), new __r_QuestRewardBase_1524181191_Serializer() },
{ typeof(MergeBoard.Data.Reward), new __r_Reward_4056358923_Serializer() },
};}
}
