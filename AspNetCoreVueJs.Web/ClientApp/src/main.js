import Vue from "vue";
import axios from "axios";
import BootstrapVue from "bootstrap-vue";
import App from "./App.vue";
import router from "./router.js";
import store from "./store";
import { currency, date } from "./filters";
import VeeValidate from "vee-validate";

import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

import VueToastr from "@deveodk/vue-toastr";
import "@deveodk/vue-toastr/dist/@deveodk/vue-toastr.css";

import "./helpers/validation";
import "./helpers/interceptors";

Vue.use(VueToastr, {
  defaultPosition: "toast-top-center"
});

Vue.config.productionTip = false;

Vue.use(BootstrapVue);

Vue.use(VeeValidate, {
  errorBagName: "vErrors"
});

Vue.filter("currency", currency);
Vue.filter("date", date);

const initialStore = localStorage.getItem("store");

if (initialStore) {
  store.commit("initialize", JSON.parse(initialStore));

  if (store.getters.isAuthenticated) {
    axios.defaults.headers.common["Authorization"] = `Bearer ${
      store.state.auth.access_token
    }`;
  }
}
new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
