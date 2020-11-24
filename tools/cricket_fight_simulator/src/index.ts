import Vue from 'vue';
import { Button, Input, List, Tooltip, Row, Col, Timeline, Divider, Popconfirm, Checkbox, Popover, message } from 'ant-design-vue';
// import VueVirtualScroller from 'vue-virtual-scroller'
import VueDraggable from 'vue-draggable'
// import { disableBodyScroll } from 'body-scroll-lock';
import 'ant-design-vue/dist/antd.css';
// import 'vue-virtual-scroller/dist/vue-virtual-scroller.css';
import App from './App.vue';

Vue.config.productionTip = false;

[VueDraggable, Button, Input, List, Tooltip, Row, Col, Timeline, Divider, Popconfirm, Checkbox, Popover,].forEach(e => {
  Vue.use(e);
});

Vue.prototype.$message = message;

Vue.config.productionTip = false;

Vue.filter('percent', function (value) {
  if (typeof value !== "number") {
    return value;
  }

  return `${(value * 100).toFixed(0)}%`;
});

// disableBodyScroll(window.document.body);

new Vue({
  render: h => h(App),
}).$mount('#app');
