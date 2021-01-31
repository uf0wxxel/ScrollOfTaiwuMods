<template>
  <div :style="styles">
    <img 
      :class="imageClass" 
      src="./../assets/images/0.gif" >
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import * as _ from 'lodash';
import { ICricketData } from '../utils/cricket_data_utils';
import { clone } from './../utils/utils';
import './../crickets.scss';

@Component({
    components: {},
})
export default class CricketImageComponent extends Vue {
    @Prop({ default: '' })
    className!: string;

    @Prop({ default: 150 })
    size!: number;

    @Prop({ default: 0 })
    rotateDeg!: number;

    get imageClass() {
        return `cricket-img ${this.className}`;
    }

    get styles() {
        var style = '';
        var transform = '';
        if (this.rotateDeg != 0) {
            transform = `${transform} rotate(${this.rotateDeg}deg)`;
        }

        if (this.size != 150) {
            var scale = (this.size / 150).toFixed(2);
            transform = `${transform} scale(${scale}) translate(-100%, -100%)`;
            style = `${style}width:${this.size}px;height:${this.size}px;`;
        }

        transform = _.trim(transform);

        return _.trim(style + (transform ? `transform: ${transform};` : ''));
    }
}
</script>
<style lang="scss" scoped>
.cricket-img {
    width: 150px;
    height: 150px;
    background-repeat: no-repeat;
}

// .cricket-img-50 {
//     transform: scale(0.333, 0.333);
// }
</style>
