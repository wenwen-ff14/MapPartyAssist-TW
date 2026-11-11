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
foreach(var message in new[] { "打倒了所有敵人！", "打開了通往第二區的大門！", "打倒了所有敵人！", "打開了通往第三區的大門！" }) {
    if(TraditionalChineseDutyMessages.IsNextDoorCheckpoint(message, count)) count++;
}
if(count != 4) throw new Exception("Third chamber progress failed");
if(TraditionalChineseDutyMessages.IsNextDoorCheckpoint("打開了通往第三區的大門！", count)) throw new Exception("Duplicate door accepted");
if(!TraditionalChineseDutyMessages.IsEnd("陷阱發動，被強制傳送出寶物庫……")) throw new Exception("Trap ending missing");
if(!TraditionalChineseDutyMessages.IsEnd("因為團滅導致被強制傳送出寶物庫……")) throw new Exception("Wipe ending missing");
if(TraditionalChineseDutyMessages.IsEnd("通往寶物庫的傳送魔紋出現了！")) throw new Exception("Portal falsely ended duty");
foreach(var chambers in new[] {5, 7}) {
    var checkpoints = 0;
    for(var room = 1; room <= chambers; room++) {
        if(room > 1 && TraditionalChineseDutyMessages.IsNextDoorCheckpoint($"打開了通往{(room == chambers ? "最終區" : "第" + "一二三四五六七"[room - 1] + "區")}的大門！", checkpoints, chambers)) checkpoints++;
        if(TraditionalChineseDutyMessages.IsNextDoorCheckpoint("打倒了所有敵人！", checkpoints)) checkpoints++;
    }
    if(checkpoints != chambers * 2 - 1) throw new Exception("Full clear replay failed");
}
Console.WriteLine("TC door progress and ending regression checks passed.");
foreach(var tier in new[] { "下級召喚", "中級召喚", "上級召喚", "特殊召喚", "召喚式變動" }) {
    var summon = TraditionalChineseDutyMessages.Summon.Match($"發動了{tier}，測試魔物出現了！");
    if(!summon.Success || summon.Groups["tier"].Value != tier) throw new Exception("Summon tier not parsed");
}
if(!TraditionalChineseDutyMessages.Summon.Match("測試魔物消失後，發動了上級召喚，另一魔物出現了！").Groups["saved"].Success) throw new Exception("Save missing");
if(TraditionalChineseDutyMessages.Summon.IsMatch("更多魔物出現了！")) throw new Exception("Reinforcement counted as summon");
if(TraditionalChineseDutyMessages.Summon.IsMatch("召喚失敗，測試魔物出現了！")) throw new Exception("Failure counted as summon");
if(!TraditionalChineseDutyMessages.IsSummonDefeated("討伐了所有的魔物！") || !TraditionalChineseDutyMessages.IsSummonDeparted("觀察者的幻體消失了……")) throw new Exception("Roulette end missing");
if(TraditionalChineseDutyMessages.IsNextDoorCheckpoint("打開了通往第四區的大門！", 1)) throw new Exception("Wrong chamber accepted");
if(TraditionalChineseDutyMessages.Quantity(TraditionalChineseDutyMessages.LootList.Match("獲得了新的戰利品測試物品×12。")) != 12) throw new Exception("Loot count failed");
if(TraditionalChineseDutyMessages.Quantity(TraditionalChineseDutyMessages.ObtainedItem.Match("測試角色獲得了「測試物品」。")) != 1) throw new Exception("Single item failed");
if(TraditionalChineseDutyMessages.Quantity(TraditionalChineseDutyMessages.Gil.Match("獲得了10,000Gil。")) != 10000) throw new Exception("Gil failed");
Console.WriteLine("TC summon, chamber number and loot template checks passed.");
namespace LiteDB { public class BsonIgnoreAttribute : Attribute {} public class BsonIdAttribute : Attribute {} }
namespace MapPartyAssist.Types.Attributes { public class ValidatedDataTypeAttribute : Attribute {} }
namespace MapPartyAssist.Types { public class MPAMap {} public class MPAMapLink {} }
