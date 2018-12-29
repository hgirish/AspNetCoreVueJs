import Vue from "vue";
//import axios from 'axios';
import BootstrapVue from "bootstrap-vue";
import App from "./App.vue";
import router from "./router.js";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-vue/dist/bootstrap-vue.css";
import store from "./store";
import { currency } from "./filters";

Vue.config.productionTip = false;

Vue.use(BootstrapVue);

Vue.filter("currency", currency);

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
