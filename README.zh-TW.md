# Map Party Assist 繁體中文預覽版

來源：https://github.com/wrath16/MapPartyAssist
基底：0ce393a（2.5.1.1，Dalamud API 13），分支 zh-TW-api13。
保留原作者與 MIT 授權。

已翻譯主要追蹤視窗、設定及部分篩選器。Release 編譯通過（31 個警告，0 個錯誤）。尚未完成全部統計畫面翻譯、繁中系統訊息解析與遊戲內測試，不能視為完整繁中服適配版。

指令：/mparty 追蹤、/mpartyconfig 設定、/mpartystats 統計、/mpartyedit 編輯。

建置：安裝 .NET 9 與 Dalamud API 13 SDK，執行 dotnet build MapPartyAssist.sln -c Release -p:DalamudLibPath=C:/path/to/Dalamud/

完整適配需要實際繁中客戶端的挖掘、寶箱、陷阱、清怪、地圖消耗、傳送門、寶物庫各層進度及戰利品訊息，以及實際遊戲和 Dalamud 版本。API 13 編譯通過不代表 hook 與記憶體結構已完成遊戲驗證。不要與原版同時載入，兩者共用內部名稱和資料庫。

## 2.5.1.2 更新
固定 API 13 SDK 驗證；不支援語系略過外語辨識表初始化；建構失敗保留完整例外。此修正仍待遊戲內確認。

## 2.5.1.3 更新
修正中文角色名稱不含空白時，MPAMember 將名字與世界固定拆成三段造成的陣列越界。改由最後一個空白分隔世界，保留完整角色名；登入時資料未就緒會略過建表。

## 2.5.1.4 更新
依繁中 LogMessage 3777、3800、6998、6999、9354、9355、9365 補上開門型進度和結束判定；沿用原版清怪／開門交替檢查點。離開副本保存實際觀察的進度，不補造通關紀錄。第三層退出及五／七層通關回放通過。召喚型進度仍未適配，遊戲內實測待確認。
