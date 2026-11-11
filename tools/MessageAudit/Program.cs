using Lumina; using Lumina.Data; using Lumina.Excel.Sheets;
System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += (context, name) => name.Name == "Lumina" ? context.LoadFromAssemblyPath(Path.Combine(AppContext.BaseDirectory, "Lumina.dll")) : null; Run(args);
static void Run(string[] args) {
using var data = new GameData(args[0], new LuminaOptions { DefaultExcelLanguage = Language.TraditionalChinese });
foreach(var action in data.GetExcelSheet<Lumina.Excel.Sheets.Action>().Where(a => a.Name.ToString().Contains("挖"))) Console.WriteLine($"ACTION {action.RowId}: {action.Name}");
foreach(var row in data.GetExcelSheet<LogMessage>()) {
 var text=row.Text.ToMacroString();
 if((row.RowId >= 1030 && row.RowId <= 1050) || row.RowId == 6998 || (row.RowId >= 3750 && row.RowId <= 3801) || row.RowId == 4405 || (row.RowId >= 9350 && row.RowId <= 9367) || System.Text.RegularExpressions.Regex.IsMatch(text,"使用了|發動了|獲得了|獲得.*Gil|戰利品|區畫|區域.*開|所有.*魔物|所有.*敵|全部.*敵|攻略.*結束|結束.*攻略|召喚|退去|寶物庫")) Console.WriteLine($"{row.RowId}: {text} MACRO={row.Text.ToMacroString()}");
}

}
