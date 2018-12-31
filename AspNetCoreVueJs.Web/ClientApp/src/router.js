import Vue from "vue";
import VueRouter from "vue-router";
import Catalogue from "./pages/Catalogue.vue";
import Product from "./pages/Product.vue";
import Cart from "./pages/Cart.vue";
import Checkout from "./pages/Checkout.vue";

import NProgress from "nprogress";
import store from "./store";

Vue.use(VueRouter);

const router = new VueRouter({
  mode: "hash",
  base: process.env.BASE_URL,
  routes: [
    {
      name: "products",
      path: "/products",
      component: Catalogue
    },
    {
      name: "product-detail",
      path: "/products/:slug",
      component: Product
    },
    {
      name: "cart",
      path: "/cart",
      component: Cart
    },
    {
      name: "checkout",
      path: "/checkout",
      component: Checkout,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: "*",
      redirect: "/products"
    }
  ]
});
router.beforeEach((to, from, next) => {
  NProgress.start();
  if (to.matched.some(route => route.meta.requiresAuth)) {
    if (!store.getters.isAuthenticated) {
      store.commit("showAuthModal");
      next({
        path: from.path,
        query: {
          redirect: to.path
        }
      });
    } else {
      next();
    }
  } else {
    next();
  }
});

router.afterEach(() => {
  NProgress.done();
});
export default router;
