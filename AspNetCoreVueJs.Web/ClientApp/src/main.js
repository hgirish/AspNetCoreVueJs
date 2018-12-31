import Vue from "vue";
import axios from "axios";
import BootstrapVue from "bootstrap-vue";
import App from "./App.vue";
import router from "./router.js";
import store from "./store";
import { currency } from "./filters";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

import VueToastr from "@deveodk/vue-toastr";
// You need a specific loader for CSS files like https://github.com/webpack/css-loader
// If you would like custom styling of the toastr the css file can be replaced
import "@deveodk/vue-toastr/dist/@deveodk/vue-toastr.css";

Vue.use(VueToastr, {
  defaultPosition: "toast-top-right"
});

Vue.config.productionTip = false;

Vue.use(BootstrapVue);

Vue.filter("currency", currency);

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
