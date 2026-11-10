using System;
using MapPartyAssist.Types;
using MapPartyAssist.Helper;
foreach(var sample in new[] { ("測試角色 巴哈姆特", "測試角色", "巴哈姆特"), ("First Last Alpha", "First Last", "Alpha") }) {
    var member = new MPAMember(sample.Item1);
    if(member.Name != sample.Item2 || member.HomeWorld != sample.Item3 || member.Key != sample.Item1) throw new Exception("Name roundtrip failed");
}
if(!PlayerHelper.IsAliasMatch("測試角色 巴哈姆特", "測試角色")) throw new Exception("Chinese alias failed");
if(!PlayerHelper.IsAliasMatch("First Last Alpha", "F. L.")) throw new Exception("English alias failed");
if(PlayerHelper.IsAliasMatch("測試角色 巴哈姆特", "別人")) throw new Exception("False alias match");
if(PlayerHelper.IsAliasMatch("Single", "A. B.")) throw new Exception("Malformed alias matched");
Console.WriteLine("Name parsing and alias regression checks passed.");
// Replay the upstream clear/open ordering through the third chamber.
var count = 0;
foreach(var message in new[] { "打倒了所有敵人！", "打開了通往第二区域的大門！", "打倒了所有敵人！", "打開了通往第三區域的大門！" }) {
    if(TraditionalChineseDutyMessages.IsNextDoorCheckpoint(message, count)) count++;
}
if(count != 4) throw new Exception("Third chamber progress failed");
if(TraditionalChineseDutyMessages.IsNextDoorCheckpoint("打開了通往第三區域的大門！", count)) throw new Exception("Duplicate door accepted");
if(!TraditionalChineseDutyMessages.IsEnd("陷阱發動，被強制傳送出寶物庫……")) throw new Exception("Trap ending missing");
if(!TraditionalChineseDutyMessages.IsEnd("因為團滅導致被強制傳送出寶物庫……")) throw new Exception("Wipe ending missing");
if(TraditionalChineseDutyMessages.IsEnd("通往寶物庫的傳送魔紋出現了！")) throw new Exception("Portal falsely ended duty");
foreach(var chambers in new[] {5, 7}) {
    var checkpoints = 0;
    for(var room = 1; room <= chambers; room++) {
        if(room > 1 && TraditionalChineseDutyMessages.IsNextDoorCheckpoint("打開了通往最終區域的大門！", checkpoints)) checkpoints++;
        if(TraditionalChineseDutyMessages.IsNextDoorCheckpoint("打倒了所有敵人！", checkpoints)) checkpoints++;
    }
    if(checkpoints != chambers * 2 - 1) throw new Exception("Full clear replay failed");
}
Console.WriteLine("TC door progress and ending regression checks passed.");
namespace LiteDB { public class BsonIgnoreAttribute : Attribute {} public class BsonIdAttribute : Attribute {} }
namespace MapPartyAssist.Types.Attributes { public class ValidatedDataTypeAttribute : Attribute {} }
namespace MapPartyAssist.Types { public class MPAMap {} public class MPAMapLink {} }

