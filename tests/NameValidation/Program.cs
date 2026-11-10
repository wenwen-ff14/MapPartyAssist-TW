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
namespace LiteDB { public class BsonIgnoreAttribute : Attribute {} public class BsonIdAttribute : Attribute {} }
namespace MapPartyAssist.Types.Attributes { public class ValidatedDataTypeAttribute : Attribute {} }
namespace MapPartyAssist.Types { public class MPAMap {} public class MPAMapLink {} }
