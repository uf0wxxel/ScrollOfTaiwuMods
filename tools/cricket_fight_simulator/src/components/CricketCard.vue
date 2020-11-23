<template>
    <div class="ququ-card">
        <div v-if="cricket" :class="side">
            <div class="ququ-side">
                {{ sideText }}
            </div>
            <div class="ququ-img">
                <CricketImageComponent
                    :className="cricket.imageUrl"
                    :rotateDeg="rotateDeg"
                    :size="150"
                ></CricketImageComponent>
            </div>
            <div class="ququ-name">
                {{ cricket.name }}
            </div>
            <!-- <div class="ququ-desc">{{ cricket.desc }}</div> -->
            <div>
                <a-row>
                    <a-col :span="24"> 耐久：{{ cricket.durability }} / {{ cricketCopy.durability }} </a-col>
                </a-row>
                <a-row>
                    <a-col :span="24"> 耐力：{{ cricket.耐力 }} / {{ cricketCopy.耐力 }} </a-col>
                </a-row>
                <a-row>
                    <a-col :span="24"> 斗性：{{ cricket.斗性 }} / {{ cricketCopy.斗性 }} </a-col>
                </a-row>
                <br />
                <a-row>
                    <a-col :span="24"> 气势：{{ cricket.气势 }} </a-col>
                </a-row>
                <a-row>
                    <a-col :span="24"> 牙钳：{{ cricket.牙钳 }} </a-col> </a-row
                ><a-row>
                    <a-col :span="24"> 角力：{{ cricket.角力 }} </a-col>
                </a-row>
                <br />
                <a-row>
                    <a-col :span="24"> 暴击概率：{{ cricket.暴击率 | percent }} </a-col></a-row
                >
                <a-row>
                    <a-col :span="24"> 暴击增伤：{{ cricket.暴击增伤 }} </a-col>
                </a-row>
                <a-row>
                    <a-col :span="24"> 格挡概率：{{ cricket.格挡概率 | percent }} </a-col></a-row
                >
                <a-row>
                    <a-col :span="24"> 格挡减伤：{{ cricket.格挡值 }} </a-col>
                </a-row>
                <a-row>
                    <a-col :span="24"> 反击概率：{{ cricket.反击率 | percent }} </a-col></a-row
                >
                <a-row>
                    <a-col :span="24"> 击伤概率：{{ (cricket.暴击率 + cricket.击伤概率修正) | percent }} </a-col>
                </a-row>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import * as _ from 'lodash';
import CricketImageComponent from './CricketImage.vue';
import { ICricketData } from '../utils/cricket_data_utils';
import { clone } from './../utils/utils';
import emptyImgRaw from './../assets/images/0.gif';

@Component({
    components: { CricketImageComponent },
})
export default class CricketCardComponent extends Vue {
    @Prop({ default: null })
    cricket!: ICricketData;

    @Prop({ default: 'red' })
    side!: 'red' | 'blue';

    @Prop({ default: '' })
    refreshToken!: string;

    emptyImg = emptyImgRaw;

    cricketCopy: ICricketData | null = null;

    get sideText() {
        return this.side === 'red' ? '红方' : '蓝方';
    }

    get rotateDeg() {
        return this.side === 'red' ? 0 : 180;
    }

    @Watch('cricket')
    onCricketChanged(value: ICricketData, oldValue: ICricketData) {
        if (value) {
            value.side = this.sideText;
            if (!this.cricketCopy) {
                this.cricketCopy = value;
            }
        }
    }

    @Watch('refreshToken')
    onRefreshTokenChanged(value: string, oldValue: string) {
        this.$forceUpdate();
    }

    mounted() {
        this.onCricketChanged(this.cricket, this.cricket);
    }
}
</script>
<style lang="scss" scoped>
.ququ-card {
    .red {
        color: red;
    }

    .blue {
        color: blue;
    }

    .ququ-img {
        text-align: center;
    }
    .ququ-side {
        text-align: center;
        font-size: 20px;
    }
    .ququ-name {
        text-align: center;
        font-size: 20px;
        margin-bottom: 20px;
    }
}
</style>
