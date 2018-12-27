import Vue from 'vue';
import axios from 'axios';
import BootstrapVue from 'bootstrap-vue'
import App from './App.vue';
import router from './router.js';

Vue.config.productionTip = false;

new Vue({
  router,
  render: h => h(App)
}).$mount('#app');
