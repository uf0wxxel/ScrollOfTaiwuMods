# [SmartWear](https://github.com/Litfal/Taiwu_mods/tree/SmartWear/SmartWear)

練功與製作時自動換上適合的裝備與功法 

不用開倉找裝備啦

不怕炒菜差一級啦

不怕打架忘了運功啦

不怕較藝忘了裝書啦

## 注意

勿與RestEquip一起使用，本MOD已包含RestEquip的功能

## 使用方法

於 Ctrl+F10 的設定中, 選擇想要的功能

* ### 練功
	包含 修習、突破、研讀
    
    * **使用功法**：可選擇一組功法，於練功時自動切換。設定一組悟性高的功法吧！ 預設*不切換*
    * **自動裝備適合的飾品**：從*裝備*、*包裹*與*倉庫*內，尋找資質與悟性高的三個飾品(寶物)自動裝備。預設*開啟*
    	* 依據當前練功的類型，選擇對應的資質
    	* 資質高的優先，資質相同時取悟性高的
    	* 關閉練功畫面時，會自動換回原本的裝備
	* **進階研讀模式**：難度超過 50% 則資質優先、悟性其次；否則悟性優先。預設*開啟*

* ### 製造
	包含 锻造、制木、炼药、炼毒、织锦、制石、烹飪
    * 自動裝備適合的飾品：從*裝備*、*包裹*與*倉庫*內，尋找資質高的三個飾品(寶物)自動裝備。預設*開啟*
    	* 依據當前製作的類型，選擇對應的資質
    	* 關閉製作畫面時，會自動換回原本的裝備
* ### 戰鬥與較藝
	* 進入戰鬥準備畫面時，使用指定功法 預設*不切換*
	* 進入戰鬥準備畫面時，使用指定裝備 預設*不切換*
	* 進入較藝準備畫面時，使用指定裝備 預設*不切換*
		* 戰鬥或較藝結束後，**不會**換回原本的功法或裝備
		* 於準備畫面時，依然可以進人物選單做調整
* ### 其他
	* **療傷與驅毒自動裝備適合的飾品**：從*裝備*、*包裹*與*倉庫*內，尋找資質(醫術/毒術)高的三個飾品自動裝備。預設*開啟*
		* 如果不在城鎮/門派格，將不會使用倉庫裡的裝備
	* **跨月恢復內息時使用功法**：選擇一個高內息的功法，跨月時可以更有效地恢復內息。預設*不切換*
	* **跨月恢復內息時自動裝備適合的武器** ：從*裝備*、*包裹*與*倉庫*內，尋找內息高的三個武器自動裝備。預設*開啟*
		* 如果不在城鎮/門派格，將不會使用倉庫裡的裝備

## 支援版本

V0.2.4.0 ~ V0.2.6.0

## 版本紀錄

* 1.1.6
	* 支援 V0.2.6.0
* 1.1.5
	* 修正進入製造畫面可能會跳錯的bug
	* 改用比較安全的方法穿上/脫下裝備
* 1.1.4
	* 修正讀書後, 原本身上的寶物可能會遺失的 bug 
* 1.1.3
	* 支援 V0.2.5.2 
* 1.1.2
	* 加入 LOG 設定
* 1.1.1
	* 修正 **跨月恢復內息時自動裝備適合的武器** 功能不會使用已裝備的武器的 bug
* 1.1.0
	* 加入 戰鬥與較藝 自動切換功法與裝備的功能
* 1.0.0
	* 修正會使用重複裝備的bug
	* 加入 療傷與驅毒自動裝備適合的飾品 (效果微乎其微就是了)
	* 加入 跨月恢復內息使用的功法與自動裝備
	* 加入 進階研讀模式：難度超過 50% 則資質優先、悟性其次；否則悟性優先
* 0.9.1
	* 修正作者耍笨忘了檢查設定開關的bug
* 0.9.0 
	* 初版