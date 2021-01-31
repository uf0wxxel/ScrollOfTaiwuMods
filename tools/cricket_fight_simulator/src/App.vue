<template>
  <div 
    v-drag-and-drop:options="dragDropOptions" 
    class="main">
    <div class="left">
      <div class="container">
        <div class="search-box">
          <a-input 
            v-model.lazy="filterText" 
            placeholder="查找促织" />
          <div class="note">从列表拖动至右方图标更换促织</div>
        </div>
        <div class="ququ-list">
          <a-list 
            :data-source="displayCricketData" 
            :rowKey="(c) => c.name" 
            item-layout="horizontal">
            <a-list-item 
              slot-scope="item" 
              slot="renderItem" 
              class="ququ-list-item">
              <a-list-item-meta>
                <div 
                  slot="description" 
                  class="desc">
                  <!-- <CricketToolTipComponent :cricket="item"> -->
                  <a-popover 
                    :title="item.name" 
                    trigger="click">
                    <div slot="content">
                      <a-button 
                        @click="setCricketHome(item)" 
                        type="danger">
                        设置为红方
                      </a-button>
                      <a-button 
                        @click="setCricketAway(item)" 
                        type="primary">
                        设置为蓝方
                      </a-button>
                    </div>
                    <div>
                      {{ item.name }}
                    </div>
                  </a-popover>
                  <!-- </CricketToolTipComponent> -->
                </div>
                <CricketToolTipComponent 
                  :cricket="item" 
                  slot="avatar">
                  <CricketImageComponent 
                    :className="item.imageUrl" 
                    :size="50" />
                </CricketToolTipComponent>
              </a-list-item-meta>
            </a-list-item>
          </a-list>
        </div>
      </div>
    </div>
    <div class="middle">
      <div class="container">
        <div class="ququ-box home">
          <CricketCardComponent
            v-if="cricketHome"
            :cricket="cricketHome"
            :refreshToken="refreshTokenHome"
            side="red"
          />
          <div 
            v-if="!cricketHome" 
            class="drop-box">从左边列表拖动促织至此区域</div>
        </div>
        <div>
          <div class="button-box">
            <a-button 
              :disabled="isFightButtonDisabled" 
              @click="doFight" 
              type="primary">模拟决斗</a-button>
            <br >
            <a-button 
              :disabled="isFightButtonDisabled" 
              @click="doFightBatch(1000)" 
              type="dashed"
            >模拟1000场决斗</a-button
            >
            <br >
            <a-button 
              :disabled="isFightButtonDisabled" 
              @click="doFightBatch(10000)" 
              type="dashed"
            >模拟10000场决斗</a-button
            >
            <br >
            <a-button-group>
              <a-popconfirm 
                @confirm="onFindRivalsConfirm(true)" 
                ok-text="确定" 
                cancel-text="取消">
                <template slot="title">
                  <div style="max-width: 20vw">
                    搜索品级与类型
                    <br >
                    <!-- <a-checkbox v-model="findRivalOptionCheckBaBai" style="color:red;">八败(容易卡死慎选)</a-checkbox> -->
                    <a-checkbox-group
                      v-model="findRivalFilters"
                      :options="findRivalOptions"
                      :defaultValue="findRivalOptionsDefault"
                    />
                    <a-checkbox v-model="searchMomentum">气势型</a-checkbox>
                    <a-checkbox v-model="searchPlier">牙钳型</a-checkbox>
                    <a-checkbox v-model="searchWresling">角力型</a-checkbox>
                    <a-checkbox v-model="searchAverage">均衡型</a-checkbox>
                </div></template
                >
                <a-button 
                  :disabled="isFightButtonDisabled" 
                  type="danger">找克星</a-button>
              </a-popconfirm>
              <a-popconfirm 
                @confirm="onFindRivalsConfirm(false)" 
                ok-text="确定" 
                cancel-text="取消">
                <template slot="title">
                  <div style="max-width: 20vw">
                    搜索品级与类型
                    <br >
                    <!-- <a-checkbox v-model="findRivalOptionCheckBaBai" style="color:red;">八败(容易卡死慎选)</a-checkbox> -->
                    <a-checkbox-group
                      v-model="findRivalFilters"
                      :options="findRivalOptions"
                      :defaultValue="findRivalOptionsDefault"
                    />
                    <a-checkbox v-model="searchMomentum">气势型</a-checkbox>
                    <a-checkbox v-model="searchPlier">牙钳型</a-checkbox>
                    <a-checkbox v-model="searchWresling">角力型</a-checkbox>
                    <a-checkbox v-model="searchAverage">均衡型</a-checkbox>
                </div></template
                >
                <a-button 
                  :disabled="isFightButtonDisabled" 
                  type="primary">找克星</a-button>
              </a-popconfirm>
            </a-button-group>
            <br >
            <a-button 
              :disabled="isFightButtonDisabled" 
              @click="restoreCricketStatus" 
              type="default"
            >还原促织状态</a-button
            >
          </div>
          <div class="battle-result">
            <div class="score-board">
              <span class="red">{{ scoreHome }}</span> : <span class="blue">{{ scoreAway }}</span>
            </div>
          </div>
        </div>
        <div class="ququ-box away">
          <CricketCardComponent
            v-if="cricketAway"
            :cricket="cricketAway"
            :refreshToken="refreshTokenAway"
            side="blue"
          />
          <div 
            v-if="!cricketAway" 
            class="drop-box">从左边列表拖动促织至此区域</div>
        </div>
      </div>
    </div>
    <div class="right">
      <div class="container scroll">
        <a-divider>战斗过程记录</a-divider>
        <!-- <a-divider></a-divider> -->
        <a-timeline>
          <a-timeline-item 
            v-for="(round, idx1) in gameRecords" 
            :key="idx1">
            <p 
              v-for="(item, idx2) in round" 
              v-html="item.message" 
              :key="idx2" />
          </a-timeline-item>
        </a-timeline>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { cricketCollection, ICricketData } from './utils/cricket_data_utils';
import { cricketFight, ICricketFlightLogRecord } from './utils/cricket_fight_engine';
import { clone } from './utils/utils';
import CricketCardComponent from './components/CricketCard.vue';
import CricketToolTipComponent from './components/CricketToolTip.vue';
import CricketImageComponent from './components/CricketImage.vue';
import { VueDraggableEvent, VueDraggableOptions } from 'vue-draggable/types/vue-draggable-options';
import * as _ from 'lodash';

@Component({
    components: { CricketCardComponent, CricketToolTipComponent, CricketImageComponent },
})
export default class CricketSimulatorPage extends Vue {
    filterText = '';

    cricketHome: ICricketData | null = null;
    cricketHomeCopy: ICricketData | null = null;
    refreshTokenHome = '';
    scoreHome = 0;
    cricketAway: ICricketData | null = null;
    cricketAwayCopy: ICricketData | null = null;
    refreshTokenAway = '';
    scoreAway = 0;

    gameRecords: ICricketFlightLogRecord[][] = [];

    get isFightButtonDisabled() {
        return !this.cricketHome || !this.cricketAway;
    }

    restoreCricketStatus() {
        this.cricketHome = clone(this.cricketHomeCopy);
        this.cricketAway = clone(this.cricketAwayCopy);
    }

    doFightBatch(count = 1000) {
        if (this.isFightButtonDisabled) {
            return;
        }

        this.gameRecords.splice(0);

        this.restoreCricketStatus();

        this.scoreHome = this.scoreAway = 0;

        for (var i = 0; i < count; i++) {
            const home = clone(this.cricketHome);
            const away = clone(this.cricketAway);
            const winner = cricketFight(
                home!,
                away!,
                (l) => {},
                (c) => c.name,
            );

            if (winner === home) {
                this.scoreHome += 1;
            } else {
                this.scoreAway += 1;
            }
        }

        const homeWinRate = this.scoreHome / (this.scoreHome + this.scoreAway);
        const awayWinRate = 1 - homeWinRate;

        this.gameRecords.push([
            {
                message: `<span style="color:red;">${this.cricketHome!.name}</span>获得${this.scoreHome}胜，胜率为${(
                    homeWinRate * 100
                ).toFixed(1)}%`,
            },
            {
                message: `<span style="color:blue;">${this.cricketAway!.name}</span>获得${this.scoreAway}胜，胜率为${(
                    awayWinRate * 100
                ).toFixed(1)}%`,
            },
        ]);
    }

    doFight() {
        if (this.isFightButtonDisabled) {
            return;
        }

        this.gameRecords.splice(0);

        this.restoreCricketStatus();

        this.scoreHome = this.scoreAway = 0;

        const winner = cricketFight(
            this.cricketHome!,
            this.cricketAway!,
            (l) => {
                if (this.gameRecords.length === 0 || l.roundChanged) {
                    this.gameRecords.push([]);
                }
                _.last(this.gameRecords).push(l);
            },
            (c) => {
                var color = c === this.cricketHome ? 'red' : 'blue';
                return `<span style="color:${color};">${c.name}</span>`;
            },
        );

        if (winner === this.cricketHome) {
            this.scoreHome += 1;
        } else {
            this.scoreAway += 1;
        }
    }

    onDrop(e: VueDraggableEvent) {
        const cricketName = e.items[0].innerText;
        // console.log(e);
        console.log(cricketName);

        const cricket = clone(_.find(cricketCollection, (c) => c.name === cricketName))!;

        console.log(cricket);

        const dropTargetClassName = e.droptarget.className;
        if (dropTargetClassName.indexOf('home') >= 0) {
            this.setCricketHome(cricket);
        } else {
            this.setCricketAway(cricket);
        }
    }

    dragDropOptions: VueDraggableOptions = {
        dropzoneSelector: '.ququ-box',
        draggableSelector: '.ququ-list-item',
        handlerSelector: '',
        reactivityEnabled: true,
        multipleDropzonesItemsDraggingEnabled: true,
        showDropzoneAreas: true,
        onDrop: this.onDrop,
        onDragstart: (e) => {
            // console.log(e);
        },
        onDragenter: (e) => {
            // console.log(e);
        },
        onDragover: (e) => {
            // console.log(e);
        },
        onDragend: (e) => {
            // console.log(e);
        },
    };

    filterCricketData() {
        const t = _.trim(this.filterText);
        const sizeLimit = 200;
        if (t) {
            return _.chain(cricketCollection)
                .filter((c) => c.name.indexOf(t) >= 0)
                .slice(0, sizeLimit)
                .value();
        }
        return _.chain(cricketCollection).slice(0, sizeLimit).value();
    }

    get displayCricketData() {
        return this.filterCricketData();
    }

    setCricketHome(c: ICricketData) {
        this.cricketHome = clone(c);
        this.cricketHomeCopy = clone(c);
    }

    setCricketAway(c: ICricketData) {
        this.cricketAway = clone(c);
        this.cricketAwayCopy = clone(c);
    }

    isRival(
        c: ICricketData,
        other: ICricketData,
    ): { isRival: boolean; win: number; loss: number; opponent: ICricketData } {
        // matchCount, win threshold inclusive
        const searchSteps: number[][] = [
            // [10, 1],
            [20, 5],
            [100, 40],
            [200, 100],
        ];

        const result = { isRival: false, win: 0, loss: 0, opponent: other };
        if (c.name === other.name) {
            return result;
        }

        var aborted = false;
        const maxGameCount = _.last(searchSteps)![0];
        searchSteps.forEach((arr) => {
            if (aborted) {
                return;
            }

            const gameCount = arr[0];
            const minWinCount = arr[1];

            while (result.win + result.loss < gameCount && (result.loss < minWinCount || gameCount === maxGameCount)) {
                const home = clone(c);
                const away = clone(other);
                const winner = cricketFight(
                    home,
                    away,
                    (r) => {},
                    (c) => c.name,
                );

                if (home === winner) {
                    result.win += 1;
                } else {
                    result.loss += 1;
                }
            }

            aborted = result.loss < minWinCount;
        });

        result.isRival = !aborted;

        return result;
    }

    findRivals(
        c: ICricketData,
        searchMomentum = true,
        searchPlier = true,
        searchWresling = true,
        searchAverage = true,
        levelFilters: number[] = [],
    ) {
        const levelGroups = _.chain(cricketCollection)
            .groupBy((c) => c.level)
            .value();

        const rivals: { isRival: boolean; win: number; loss: number; opponent: ICricketData }[] = [];
        const maxRivalFound = 15;
        var rivalFound = 0;
        for (var i = 9; i > 0 && rivalFound < maxRivalFound; i--) {
            if (levelFilters?.length > 0 && levelFilters.indexOf(i) < 0) {
                continue;
            }

            const candidates = levelGroups[i];
            for (var j = 0; j < candidates.length && rivalFound < maxRivalFound; j++) {
                var opp = candidates[j];
                if (c.name === opp.name) {
                    continue;
                }

                const oppMaxAttr = _.max([opp.气势, opp.牙钳, opp.角力]);
                const isAverage =
                    oppMaxAttr! < 3 || oppMaxAttr! < (oppMaxAttr! < 5 ? 0.5 : 0.45) * (opp.气势 + opp.牙钳 + opp.角力);
                const isMomentum = !isAverage && oppMaxAttr === opp.气势;
                const isPlier = !isAverage && oppMaxAttr === opp.牙钳;
                const isWresling = !isAverage && oppMaxAttr === opp.角力;

                const included =
                    (searchMomentum && isMomentum) ||
                    (searchPlier && isPlier) ||
                    (searchWresling && isWresling) ||
                    (searchAverage && isAverage);
                if (!included) {
                    continue;
                }

                const rivalResult = this.isRival(c, opp);
                rivals.push(rivalResult);
                if (rivalResult.isRival) {
                    rivalFound += 1;
                }
            }
        }

        return _.chain(rivals)
            .filter((r) => r.loss > 0)
            .orderBy((r) => r.loss / (r.win + r.loss), 'desc')
            .take(maxRivalFound)
            .value();
    }

    findRivalFilters = [9, 8, 7, 6];
    findRivalOptions = [
        { label: '异品促织王', value: 9 },
        { label: '真色促织王', value: 8 },
        { label: '大将军', value: 7 },
        { label: '杂号将军', value: 6 },
        { label: '护军', value: 5 },
        { label: '都尉', value: 4 },
        { label: '校尉', value: 3 },
        { label: '副尉', value: 2 },
        { label: '小卒', value: 1 },
    ];
    searchMomentum = true;
    searchPlier = true;
    searchWresling = true;
    searchAverage = true;
    // findRivalOptionCheckBaBai = false;

    onFindRivalsConfirm(isHome: boolean) {
        this.gameRecords.splice(0);
        this.restoreCricketStatus();
        this.scoreHome = this.scoreAway = 0;

        const c = isHome ? this.cricketHomeCopy : this.cricketAwayCopy;
        if (!c) {
            return;
        }

        const results = this.findRivals(
            c!,
            this.searchMomentum,
            this.searchPlier,
            this.searchWresling,
            this.searchAverage,
            this.findRivalFilters,
        );
        const logMsgs: ICricketFlightLogRecord[] = [
            {
                message: `<span style="color:${isHome ? 'red' : 'blue'};">${
                    c!.name
                }</span> (克星太多时只显示找到的前20个)`,
            },
        ];
        this.gameRecords.push(logMsgs);
        if (results?.length > 0) {
            results.forEach((r) => {
                var msg = `<div style="width:100px;display: inline-block;white-space:nowrap;">${
                    r.opponent.name
                }</div>胜率${((r.win / (r.win + r.loss)) * 100).toFixed(1)}% (${r.win}:${r.loss})`;
                msg = `<span style="color:${r.isRival ? 'green' : 'grey'};">${msg}</span>`;

                logMsgs.push({
                    message: msg,
                });
            });
        } else {
            logMsgs.push({ message: '没有找到对手' });
        }
    }

    mounted() {
        for (var i = 0; i < cricketCollection.length; i++) {
            const cricket = cricketCollection[i];
            if (cricket.name.indexOf('天蓝青') >= 0) {
                if (!this.cricketHome) {
                    this.setCricketHome(cricket);
                }
            }

            if (cricket.name.indexOf('三段锦') >= 0) {
                if (!this.cricketAway) {
                    this.setCricketAway(cricket);
                }
            }
        }
    }
}
</script>

<style lang="scss" scoped>
.main {
    text-size-adjust: auto;
    -webkit-text-size-adjust: auto;

    -moz-transform-origin: left top;
    -ms-transform-origin: left top;
    -webkit-transform-origin: left top;
    transform-origin: left top;

    height: 96vh;
    margin: 2vh 2vw;
    box-shadow: 1px 1px 5px grey;

    .title {
        margin-bottom: 50px;
        text-align: center;

        font-size: 70px;
        line-height: 1em;

        color: #fff;
    }

    .left,
    .middle,
    .right {
        float: left;
        height: 100%;
        position: relative;
        .container {
            .red {
                color: red;
            }
            .blue {
                color: blue;
            }
        }
        .note {
            color: #555;
            font-size: 12px;
        }
    }

    .left {
        width: 15%;
    }

    .middle {
        width: 45%;
        .container {
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;

            .ququ-box {
                height: 100%;
                // background: yellowgreen;
                width: 30%;
                padding: 1vh 1vw;
                .drop-box {
                    background: #777;
                    opacity: 0.8;
                    height: 100%;
                    color: white;
                    padding: 1em;
                    font-size: 18px;
                }
            }

            .button-box {
                display: flex;
                flex-direction: column;
                padding-top: 5vh;
                .ant-btn-group {
                    text-align: center;
                }
            }

            .battle-result {
                padding-top: 40px;
                font-size: 40px;
                text-align: center;
            }
        }
    }

    .right {
        width: 40%;
    }

    .container {
        position: relative;
        margin: 3vh 1vw;
        height: -moz-available; /* WebKit-based browsers will ignore this. */
        height: -webkit-fill-available; /* Mozilla-based browsers will ignore this. */
        height: fill-available;
    }

    .search-box {
        padding: 1vh 0;
        .note {
            margin-top: 1vh;
        }
    }
    .ququ-list {
        overflow-x: hidden;
        overflow-y: auto;
        max-height: 88%;
        .desc {
            line-height: 50px;
            color: #777;
        }
        .ququ-list-item {
            padding: 2px 0;
            &:hover {
                cursor: pointer;
                box-shadow: 1px 0px 1px cyan;
            }
        }
        .ququ-img {
            width: 50px;
            height: 50px;
        }
    }

    .scroll {
        overflow-y: auto;
        -webkit-overflow-scrolling: touch;
        scrollbar-width: none;
        /* Firefox */
        -ms-overflow-style: none;
        /* IE 10+ */
    }

    ::-webkit-scrollbar {
        display: none;
        /* Chrome Safari */
    }
}
</style>
