using System.Text.RegularExpressions;

namespace MapPartyAssist.Helper;

// Verified against the TC client's LogMessage rows 3777, 3800, 6998,
// 6999, 9354, 9355 and 9365. Keep the upstream alternating checkpoints.
internal static class TraditionalChineseDutyMessages {
    internal static bool IsEnd(string text) =>
        Regex.IsMatch(text, @"強制傳送出寶物庫[…。.!！]*$");

    internal static bool IsNextDoorCheckpoint(string text, int checkpointIndex) =>
        checkpointIndex % 2 == 0
            ? text == "打倒了所有敵人！"
            : Regex.IsMatch(text, @"^打開了通往.+的大門！$");
}
