<template>
  <div v-if="cricket">
    <a-tooltip>
      <template slot="title">
        {{ cricket.name }}
        <br ><br >
        {{ cricket.desc }}<br ><br >
        <a-row>
          <a-col :span="12"> 耐力：{{ cricket.耐力 }} </a-col>
          <a-col :span="12"> 斗性：{{ cricket.斗性 }} </a-col>
        </a-row>
        <a-row>
          <a-col :span="12"> 气势：{{ cricket.气势 }} </a-col>
        <a-col :span="12"> 牙钳：{{ cricket.牙钳 }} </a-col> </a-row
        ><a-row>
          <a-col :span="12"> 角力：{{ cricket.角力 }} </a-col>
        </a-row>
        <br >
        <a-row>
          <a-col :span="12"> 暴击概率：{{ cricket.暴击率 | percent }} </a-col>
          <a-col :span="12"> 暴击增伤：{{ cricket.暴击增伤 }} </a-col>
        </a-row>
        <a-row>
          <a-col :span="12"> 格挡概率：{{ cricket.格挡概率 | percent }} </a-col>
          <a-col :span="12"> 格挡减伤：{{ cricket.格挡值 }} </a-col>
        </a-row>
        <a-row>
          <a-col :span="12"> 反击概率：{{ cricket.反击率 | percent }} </a-col>
          <a-col :span="12"> 击伤概率：{{ (cricket.暴击率 + cricket.击伤概率修正) | percent }} </a-col>
        </a-row>
      </template>
      <slot/>
    </a-tooltip>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import * as _ from 'lodash';
import { ICricketData } from '../utils/cricket_data_utils';
import { clone } from './../utils/utils';

@Component({
    components: {},
})
export default class CricketToolTipComponent extends Vue {
    @Prop({ default: null })
    cricket!: ICricketData;
}
</script>
<style lang="scss" scoped>
</style>
