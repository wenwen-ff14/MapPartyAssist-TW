using MapPartyAssist.Types;
using System;

namespace MapPartyAssist.Helper {
    internal static class PlayerHelper {

        public static Region GetRegion(byte? regionByte) {
            return regionByte switch {
                1 => Region.Japan,
                2 => Region.NorthAmerica,
                3 => Region.Europe,
                4 => Region.Oceania,
                _ => Region.Unknown,
            };
        }

        public static bool IsAliasMatch(string fullName, string abbreviatedName) {
            if(string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(abbreviatedName)) {
                return false;
            }
            // Chinese character names have no first/last-name space.
            if(fullName.Equals(abbreviatedName, StringComparison.OrdinalIgnoreCase) ||
                fullName.StartsWith(abbreviatedName + " ", StringComparison.OrdinalIgnoreCase)) {
                return true;
            }
            var abbreviatedNameList = abbreviatedName.Trim().Split(' ');
            var fullNameList = fullName.Trim().Split(' ');
            if(abbreviatedNameList.Length < 2 || fullNameList.Length < 2) {
                //error!
                return false;
            }
            for(int i = 0; i < 2; i++) {
                var curFullName = fullNameList[i];
                var curAbbreviatedName = abbreviatedNameList[i];
                if(curFullName.Length == 0 || curAbbreviatedName.Length == 0) return false;
                if(curAbbreviatedName.Contains('.')) {
                    if(curFullName[0] != curAbbreviatedName[0]) {
                        return false;
                    }
                } else if(!curFullName.Equals(curAbbreviatedName, StringComparison.OrdinalIgnoreCase)) {
                    return false;
                }
            }
            return true;
        }
    }
}
