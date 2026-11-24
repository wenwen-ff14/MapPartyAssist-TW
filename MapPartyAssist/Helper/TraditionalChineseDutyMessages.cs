using System.Text.RegularExpressions;

namespace MapPartyAssist.Helper;

// Verified against the TC client's LogMessage rows 3777, 3800, 6998,
// 6999, 9354, 9355 and 9365. Keep the upstream alternating checkpoints.
internal static class TraditionalChineseDutyMessages {
    // TC LogMessage 11262, 11265, 11269, 11270, 11274.
    // -1 = ignore, 0..4 = summon tier, 5 = complete current stage.
    internal static int SlotsEvent(string text, int reached, int total, bool special) {
        if(reached >= total || reached < 0) return -1;
        if(reached % 2 == 0) {
            var match = Regex.Match(text, @"^(小小|中等|大大|最終)夢想！(?:.+出現了！|打倒所有敵人！)$");
            if(match.Success) return match.Groups[1].Value switch { "小小" => 0, "中等" => 1, "大大" => 2, _ => 4 };
            if(text == "90秒之內撿拾100個「發光的袋子」！") return 3;
        } else if(special) {
            if(text == "「驚奇夢想」結束了……" || Regex.IsMatch(text, @"^獲得了\d+個「發光的袋子」，出現了(金|銀|銅)寶箱！$")) return 5;
        } else if(text == "打倒了所有敵人！") return 5;
        return -1;
    }
    internal static bool CanConfirmFinalClear(int reached, int total, bool ended) => !ended && total > 0 && reached == total - 1;
    internal static readonly Regex Gil = new(@"^獲得了(?<quantity>[\d,]+)Gil。");
    internal static readonly Regex ObtainedItem = new(@"^(?<player>.*?)獲得了「[^」]+」(?:×(?<quantity>[\d,]+))?。$");
    internal static readonly Regex LootList = new(@"^獲得了新的戰利品.+?(?:×(?<quantity>[\d,]+))?。(?:（限時獎勵效果生效！）)?$");
    internal static int Quantity(Match match) => match.Groups["quantity"].Success &&
        int.TryParse(match.Groups["quantity"].Value.Replace(",", ""), out var value) ? value : 1;
    // LogMessage 9352/9353: switch branches carry the summon tier.
    internal static readonly Regex Summon = new(@"^(?:(?<saved>.+消失後，))?發動了(?<tier>下級召喚|中級召喚|上級召喚|特殊召喚|召喚式變動)，(?<monster>.+)出現了！$");
    internal static bool IsSummonDefeated(string text) => text is "討伐了所有的魔物！" or "打倒了所有魔物！" or "打倒了所有敵人！";
    internal static bool IsSummonDeparted(string text) => text is "魔物都離開了！" or "觀察者的幻體消失了……";
    internal static bool IsEnd(string text) =>
        Regex.IsMatch(text, @"強制傳送出寶物庫[…。.!！]*$") || text is "被強制送出尋夢金庫……" or "小隊全滅，被強制傳送出尋夢金庫……";

    internal static bool IsNextDoorCheckpoint(string text, int checkpointIndex, int chambers = 7) =>
        checkpointIndex % 2 == 0
            ? text == "打倒了所有敵人！"
            : text == $"打開了通往{(checkpointIndex / 2 + 2 == chambers ? "最終區" : "第" + "一二三四五六七"[checkpointIndex / 2 + 1] + "區")}的大門！";
}
