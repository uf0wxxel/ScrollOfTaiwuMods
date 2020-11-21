# 更新日志

## 1.0.0
- 从SaveLoadBtns添加了读取SaveBackup存档的功能

## 1.0.1
- 修改读取load.zip为读取当前存档

## 1.1.0
- 替换太吾百晓册图标为快速读档

## 1.2.0
- 同时适配测试版和稳定版
- 修正加速文件中记录的时间错误

## 1.2.1
- 改为反射调用

## 1.2.2
- 适配0.1.6.0

## 1.3.0
- 不再依赖SaveBackup
- 备份逻辑修改为仅在存档后备份存档

## 1.3.1
- 添加锁，防止并行冲突

## 1.3.2
- 临时文件使用guid生成文件名，防止冲突

## 1.3.3
- 修复备份存档的压缩包的文件名错误

## 1.3.4
- 修改存档的备份，防止备份了其他的文件或文件夹

## 1.4.0
- 通过7z打包或解压存档，解决.net4下无法调用ionic.zip.dll的问题

## 1.4.1
- 修复了ionic.zip的调用问题，不再使用7z处理压缩文档

## 1.4.2
- 调整图标替换，适配新版本——不再替换了太吾百晓册按钮而是直接添加3个按钮 调整其他按钮为添加的按钮空出位置

## 1.4.3
- 调整存档备份流程
- 替换DotNetZip.dll与Taiwu本体相同版本

## 1.4.4
- 不再手动导入DotNetZip.dll等DLL文件
- 备份时将会备份chunk文件夹中的内容
- 直接拷贝系统自动备份到备份文件夹，不重复生成

## 1.4.5
- 恢复在游戏存档后备份的策略，兼容0.2.2.0

## 1.4.6
- 解决读档时卡在99%的问题
- 解决“解读奇书”按钮重合问题

## 1.4.7
- 兼容0.2.3.0

## 1.4.8
- 修复读档时部分内存数据未清理的bug

## 1.4.9
(根据 @ciwei100000 的pr实现)
- 修复"解读奇书"按钮重合问题
- 防止存档后备份存档时进行读档存档操作
- 修复系统自动备份失效的bug
- 修复系统无法读取存档的bug

## 1.4.9.2 by litfal
- 支援太吾 0.2.4.0

## 1.4.9.3 by litfal
- 支援太吾 0.2.4.1
- 取消了對原程式 DoBackup 的處理，因為現在存檔結構和備份結構不太一樣。所以這個版本存檔時會做兩次的存檔備份 (SLManager一次、太吾自己做一次)

## 1.4.9.4 by litfal
- 調整備份管理機制，減少存檔滿時的Rename行為
- 修正存檔到了第1000份後，沒有辦法建立新的存檔備份的bug

## 1.5.0
- 支援太吾 0.2.5.6

## 1.5.1
- 加入讀檔後重置亂數種子的功能

## 1.5.2
- 修正沒有完整備份整個存檔的bug (讀取居然不會跳錯!)

## 1.5.3
- 修正戰鬥結束後馬上讀檔，會使得戰鬥黑屏卡死的bug

## 1.5.4
- 修正開新角色第一次使用讀檔功能時，會進入創角頁面的bug

## 1.5.5
- 支援太吾 0.2.6.x
- 改以負面列表方式處理存檔包含的檔案，減少未來改版造成存檔遺失的風險

## 1.5.6
- 适配太吾 0.2.7.x