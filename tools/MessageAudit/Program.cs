using Lumina; using Lumina.Data; using Lumina.Excel.Sheets;
System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += (context, name) => name.Name == "Lumina" ? context.LoadFromAssemblyPath(Path.Combine(AppContext.BaseDirectory, "Lumina.dll")) : null; Run(args);
static void Run(string[] args) {
using var data = new GameData(args[0], new LuminaOptions { DefaultExcelLanguage = Language.TraditionalChinese });
foreach(var row in data.GetExcelSheet<LogMessage>()) {
 var text=row.Text.ToString();
 if((row.RowId >= 9350 && row.RowId <= 9367) || System.Text.RegularExpressions.Regex.IsMatch(text,"區畫|區域.*開|所有.*魔物|所有.*敵|全部.*敵|攻略.*結束|結束.*攻略|召喚|退去|寶物庫")) Console.WriteLine($"{row.RowId}: {text}");
}

}



