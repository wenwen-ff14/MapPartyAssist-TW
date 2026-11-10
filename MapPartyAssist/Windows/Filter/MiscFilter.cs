using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using System;
using System.Threading.Tasks;

namespace MapPartyAssist.Windows.Filter {
    public class MiscFilter : DataFilter {
        public override string Name => "Other";

        public bool LootOnly { get; set; }

        public bool ShowDeleted { get; set; }

        public MiscFilter() { }

        internal MiscFilter(Plugin plugin, Func<Task> action, MiscFilter? filter = null) : base(plugin, action) {
            if(filter is not null) {
                LootOnly = filter.LootOnly;
                ShowDeleted = filter.ShowDeleted;
            }
        }

        internal override void Draw() {
            using var table = ImRaii.Table("miscFilterTable", 3, ImGuiTableFlags.NoClip);
            if(table) {
                ImGui.TableSetupColumn($"c1", ImGuiTableColumnFlags.WidthFixed, float.Min(ImGui.GetContentRegionAvail().X / 3, ImGuiHelpers.GlobalScale * 350f));
                ImGui.TableSetupColumn($"c2", ImGuiTableColumnFlags.WidthFixed, float.Min(ImGui.GetContentRegionAvail().X / 3, ImGuiHelpers.GlobalScale * 350f));
                ImGui.TableSetupColumn($"c3", ImGuiTableColumnFlags.WidthFixed, float.Min(ImGui.GetContentRegionAvail().X / 3, ImGuiHelpers.GlobalScale * 350f));
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                bool lootOnly = LootOnly;
                if(ImGui.Checkbox("僅顯示有戰利品的紀錄", ref lootOnly)) {
                    _plugin!.DataQueue.QueueDataOperation(() => {
                        LootOnly = lootOnly;
                        Refresh();
                    });
                }
                ImGui.TableNextColumn();
                bool showDeleted = ShowDeleted;
                if(ImGui.Checkbox("顯示已刪除或未完成的紀錄", ref showDeleted)) {
                    _plugin!.DataQueue.QueueDataOperation(() => {
                        ShowDeleted = showDeleted;
                        Refresh();
                    });
                }
            }
        }
    }
}
