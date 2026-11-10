using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using MapPartyAssist.Helper;
using MapPartyAssist.Settings;
using System.Numerics;

namespace MapPartyAssist.Windows {
    internal class ConfigWindow : Window {

        private Plugin _plugin;

        internal ConfigWindow(Plugin plugin) : base("藏寶隊伍助手設定") {
            SizeConstraints = new WindowSizeConstraints {
                MinimumSize = new Vector2(300, 50),
                MaximumSize = new Vector2(2000, 2000)
            };
            PositionCondition = ImGuiCond.Appearing;
            _plugin = plugin;
        }

        public override void Draw() {
            using(var tabBar = ImRaii.TabBar("SettingsTabBar", ImGuiTabBarFlags.None)) {
                if(tabBar) {
                    using(var tab1 = ImRaii.TabItem("一般統計")) {
                        if(tab1) {
                            DrawStatsSettings();
                        }
                    }
                    using(var tab2 = ImRaii.TabItem("藏寶圖追蹤")) {
                        if(tab2) {
                            DrawMapSettings();
                        }
                    }
                    using(var tab3 = ImRaii.TabItem("寶物庫進度")) {
                        if(tab3) {
                            DrawDutySettings();
                        }
                    }
                }
            }
        }

        private void DrawMapSettings() {
            bool requireDoubleTap = _plugin.Configuration.RequireDoubleClickOnClearAll;
            if(ImGui.Checkbox("「全部清除」需要按兩下", ref requireDoubleTap)) {
                _plugin.Configuration.RequireDoubleClickOnClearAll = requireDoubleTap;
                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
            }

            bool hideZoneTable = _plugin.Configuration.HideZoneTable;
            if(ImGui.Checkbox("隱藏「依區域顯示地圖座標」", ref hideZoneTable)) {
                _plugin.Configuration.HideZoneTable = hideZoneTable;
                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
            }

            bool hideZoneTableEmpty = _plugin.Configuration.HideZoneTableWhenEmpty;
            if(ImGui.Checkbox("僅在沒有資料時隱藏區域地圖座標", ref hideZoneTableEmpty)) {
                _plugin.Configuration.HideZoneTableWhenEmpty = hideZoneTableEmpty;
                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
            }

            bool undockZoneWindow = _plugin.Configuration.UndockZoneWindow;
            if(ImGui.Checkbox("將區域地圖座標顯示為獨立視窗", ref undockZoneWindow)) {
                _plugin.Configuration.UndockZoneWindow = undockZoneWindow;
                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
            }

            bool noOverwriteMapLink = _plugin.Configuration.NoOverwriteMapLink;
            if(ImGui.Checkbox("不要覆寫地圖座標", ref noOverwriteMapLink)) {
                _plugin.DataQueue.QueueDataOperation(() => {
                    _plugin.Configuration.NoOverwriteMapLink = noOverwriteMapLink;
                    _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
                });
            }
            ImGui.SameLine();
            ImGuiHelper.HelpMarker("Will only clear map link on new treasure map added to player, manual removal, or manual re-assignment of latest map only.");

            bool highlightCurrentZoneLinks = _plugin.Configuration.HighlightLinksInCurrentZone;
            if(ImGui.Checkbox("以黃色標示目前區域的地圖座標", ref highlightCurrentZoneLinks)) {
                _plugin.Configuration.HighlightLinksInCurrentZone = highlightCurrentZoneLinks;
                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
            }

            bool highlightClosestLink = _plugin.Configuration.HighlightClosestLink;
            if(ImGui.Checkbox("以橘色標示最近的地圖座標", ref highlightClosestLink)) {
                _plugin.Configuration.HighlightClosestLink = highlightClosestLink;
                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
            }

            string mapLinkMessage = _plugin.Configuration.MapLinkChat ?? "<flag>";
            ImGui.Text("地圖座標聊天訊息格式");
            ImGuiHelper.HelpMarker("Message to display in party chat when using the announce map link function. Recommend enabling 'Don't overwrite map links' with this feature. Hit enter to save after editing.\n\nCustom shortcuts:\nPlayer name: <name>\nFull name with home world: <fullname>\nFirst name: <firstname>");
            if(ImGui.InputText("###MapLinkChatPrefix", ref mapLinkMessage, 50, ImGuiInputTextFlags.EnterReturnsTrue)) {
                _plugin.Configuration.MapLinkChat = mapLinkMessage;
                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
            }
        }

        private void DrawDutySettings() {
            bool showTooltips = _plugin.Configuration.ShowStatsWindowTooltips;
            if(ImGui.Checkbox("顯示說明提示", ref showTooltips)) {
                _plugin.Configuration.ShowStatsWindowTooltips = showTooltips;
                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
            }

            int progressCountToInt = (int)_plugin.Configuration.ProgressTableCount;
            string[] progressCountOptions = { "計算所有次數", "僅計算最後檢查點" };
            ImGui.SetNextItemWidth(200f * ImGuiHelpers.GlobalScale);
            if(ImGui.Combo($"檢查點累計方式##CountCombo", ref progressCountToInt, progressCountOptions, 2)) {
                _plugin.Configuration.ProgressTableCount = (ProgressTableCount)progressCountToInt;
                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
            }

            int progressRateToInt = (int)_plugin.Configuration.ProgressTableRate;
            string[] progressRateOptions = { "依總挑戰次數", "依上一階段次數" };
            ImGui.SetNextItemWidth(200f * ImGuiHelpers.GlobalScale);
            if(ImGui.Combo($"進度比率分母##RateCombo", ref progressRateToInt, progressRateOptions, 2)) {
                _plugin.Configuration.ProgressTableRate = (ProgressTableRate)progressRateToInt;
                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
            }

            int clearSequenceToInt = (int)_plugin.Configuration.ClearSequenceCount;
            string[] clearSequenceOptions = { "依總挑戰次數", "自上次通關起" };
            ImGui.SetNextItemWidth(200f * ImGuiHelpers.GlobalScale);
            if(ImGui.Combo($"通關間隔計算方式##ClearSequenceCombo", ref clearSequenceToInt, clearSequenceOptions, 2)) {
                _plugin.Configuration.ClearSequenceCount = (ClearSequenceCount)clearSequenceToInt;
                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
            }

            ImGui.Text("所有寶物庫：");

            bool allDeaths = true;
            bool allSequences = true;
            bool allZeroOmit = true;
            foreach(var dutyConfig in _plugin.Configuration.DutyConfigurations) {
                allDeaths = allDeaths && dutyConfig.Value.DisplayDeaths;
                allSequences = allSequences && dutyConfig.Value.DisplayClearSequence;
                allZeroOmit = allZeroOmit && dutyConfig.Value.OmitZeroCheckpoints;
            }

            using(var table = ImRaii.Table($"##allDutiesConfigTable", 2, ImGuiTableFlags.NoBordersInBody | ImGuiTableFlags.NoHostExtendX)) {
                if(table) {
                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    if(ImGui.Checkbox("顯示通關間隔", ref allSequences)) {
                        foreach(var dutyConfig in _plugin.Configuration.DutyConfigurations) {
                            dutyConfig.Value.DisplayClearSequence = allSequences;
                        }
                        _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
                    }
                    ImGui.TableNextColumn();
                    if(ImGui.Checkbox("顯示全滅次數", ref allDeaths)) {
                        foreach(var dutyConfig in _plugin.Configuration.DutyConfigurations) {
                            dutyConfig.Value.DisplayDeaths = allDeaths;
                        }
                        _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
                    }
                    ImGui.TableNextColumn();
                    if(ImGui.Checkbox("略過未到達檢查點的紀錄", ref allZeroOmit)) {
                        foreach(var dutyConfig in _plugin.Configuration.DutyConfigurations) {
                            dutyConfig.Value.OmitZeroCheckpoints = allZeroOmit;
                        }
                        _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
                    }
                    ImGui.SameLine();
                    ImGuiHelper.HelpMarker("統計將略過未到達任何檢查點的挑戰。");
                }
            }

            foreach(var dutyConfig in _plugin.Configuration.DutyConfigurations) {
                if(ImGui.CollapsingHeader($"{_plugin.DutyManager.Duties[dutyConfig.Key].GetDisplayName()}##Header")) {
                    using(var table = ImRaii.Table($"##{dutyConfig.Key}--ConfigTable", 2, ImGuiTableFlags.NoBordersInBody | ImGuiTableFlags.NoHostExtendX)) {
                        if(table) {
                            //ImGui.TableSetupColumn("config1", ImGuiTableColumnFlags.WidthFixed, ImGuiHelpers.GlobalScale * 200f);
                            //ImGui.TableSetupColumn($"config2", ImGuiTableColumnFlags.WidthFixed, ImGuiHelpers.GlobalScale * 200f);
                            ImGui.TableNextRow();
                            ImGui.TableNextColumn();
                            bool displayClearSequence = dutyConfig.Value.DisplayClearSequence;
                            if(ImGui.Checkbox($"顯示通關間隔##{dutyConfig.Key}--ClearSequence", ref displayClearSequence)) {
                                dutyConfig.Value.DisplayClearSequence = displayClearSequence;
                                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
                            }
                            ImGui.TableNextColumn();
                            bool showDeaths = dutyConfig.Value.DisplayDeaths;
                            if(ImGui.Checkbox($"顯示全滅次數##{dutyConfig.Key}--Wipes", ref showDeaths)) {
                                dutyConfig.Value.DisplayDeaths = showDeaths;
                                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
                            }
                            ImGui.TableNextColumn();
                            bool omitZeroCheckpoints = dutyConfig.Value.OmitZeroCheckpoints;
                            if(ImGui.Checkbox($"略過未到達檢查點的紀錄##{dutyConfig.Key}--NoCheckpoints", ref omitZeroCheckpoints)) {
                                dutyConfig.Value.OmitZeroCheckpoints = omitZeroCheckpoints;
                                _plugin.DataQueue.QueueDataOperation(_plugin.Configuration.Save);
                            }
                        }
                    }
                }
            }
        }

        private void DrawStatsSettings() {
            bool separateStatsByPlayer = _plugin.Configuration.CurrentCharacterStatsOnly;
            if(ImGui.Checkbox("僅統計目前角色的紀錄", ref separateStatsByPlayer)) {
                _plugin.DataQueue.QueueDataOperation(async () => {
                    _plugin.Configuration.CurrentCharacterStatsOnly = separateStatsByPlayer;
                    await _plugin.Refresh();
                });
            }
            ImGui.SameLine();
            ImGuiHelper.HelpMarker("Only counts maps/duties in stats where the currently logged-in character was present, name-changes not-withstanding.");

            bool enablePriceCheck = _plugin.Configuration.EnablePriceCheck;
            if(ImGui.Checkbox("啟用市場價格查詢", ref enablePriceCheck)) {
                _plugin.DataQueue.QueueDataOperation(async () => {
                    _plugin.Configuration.EnablePriceCheck = enablePriceCheck;
                    if(enablePriceCheck) {
                        _plugin.PriceHistory.EnablePolling();
                    } else {
                        _plugin.PriceHistory.DisablePolling();
                    }
                    await _plugin.Refresh();
                });
            }
            ImGui.SameLine();
            ImGuiHelper.HelpMarker("使用 Universalis API。");
        }
    }
}
