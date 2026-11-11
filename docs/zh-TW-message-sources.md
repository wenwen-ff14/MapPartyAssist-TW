# 繁中訊息來源與 2.5.1.5 限制

來源：本機 FINAL FANTASY XIV TC/game/sqpack，Lumina Language.TraditionalChinese。
工具：tools/MessageAudit；使用 ToMacroString()，而非僅 ToString()。條件／switch 分支中的文字不會出現在普通純文字輸出。

| 資料列 | 用途 |
| --- | --- |
| Action 1695 + LogMessage 533 | 挖掘動作、角色名稱 |
| LogMessage 3756、3759、3765、3766、4405 | 開箱、發現寶箱、清怪、地圖消耗 |
| LogMessage 3770、3778 | 傳送門出現、申請進入 |
| LogMessage 3777、3800、6998、9365 | 清怪與開門；第二區至第六區／最終區 |
| LogMessage 9352、9353 | 下級／中級／上級／特殊召喚、召喚式變動；救援分支 |
| LogMessage 9354、9355、6999 | 強制退出與團滅 |
| LogMessage 9360、9366 | 清怪與特殊怪物離場分支 |
| LogMessage 750、751、1254、1258 | 道具取得、戰利品列表、Gil |

轉盤按召喚／擊敗交替記錄；失敗召喚不算成功檢查點，特殊召喚離場則結束該輪。訊息需在原版系統頻道收到才會更新。所有規則經本機模板回歸測試，但尚未遊戲內驗證。

未支援 Slots 結構的新版寶物庫、沒有 ItemPayload 的特殊貨幣。中途啟動或遺漏訊息不會自動補造檢查點。洞外多人同時挖掘與不同玩家連續開門仍有原版歸屬推測限制。舊版缺失的歷史進度不能憑空補回。
