using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
var pluginDir = Path.GetFullPath(args[0]);
var sdkDir = Path.GetFullPath(args[1]);
AssemblyLoadContext.Default.Resolving += (context, name) => {
    foreach(var dir in new[] {pluginDir, sdkDir}) {
        var file = Path.Combine(dir, name.Name + ".dll");
        if(File.Exists(file)) return context.LoadFromAssemblyPath(file);
    }
    return null;
};
var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(pluginDir, "MapPartyAssist.dll"));
Type T(string name) => assembly.GetType("MapPartyAssist." + name, true)!;
var managerType = T("Services.DutyManager");
var manager = RuntimeHelpers.GetUninitializedObject(managerType);
var addLoot = managerType.GetMethod("AddLootResults", BindingFlags.NonPublic | BindingFlags.Instance)!;
var resultsType = T("Types.DutyResults");
var checkpointType = T("Types.CheckpointResults");
foreach(var legacyNull in new[] {false, true}) {
    var results = Activator.CreateInstance(resultsType)!;
    var checkpoint = Activator.CreateInstance(checkpointType)!;
    var lootProperty = checkpointType.GetProperty("LootResults")!;
    if(!legacyNull && lootProperty.GetValue(checkpoint) == null) throw new Exception("Checkpoint loot list not initialized");
    if(legacyNull) lootProperty.SetValue(checkpoint, null);
    ((IList)resultsType.GetProperty("CheckpointResults")!.GetValue(results)!).Add(checkpoint);
    resultsType.GetProperty("IsComplete")!.SetValue(results, true);
    foreach(var gil in new[] {55500, 100000}) addLoot.Invoke(manager, new object?[] {results, (uint)1, false, gil, "測試角色 伊弗利特"});
    var loot = (IList)lootProperty.GetValue(checkpoint)!;
    var total = loot.Cast<object>().Sum(item => (int)item.GetType().GetProperty("Quantity")!.GetValue(item)!);
    if(loot.Count != 2 || total != 155500) throw new Exception($"Lost post-clear Gil: {total}");
    Console.WriteLine($"Passed {(legacyNull ? "legacy null" : "new checkpoint")}: 55,500 + 100,000 = {total:N0}");
}
