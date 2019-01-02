import Vue from "vue";
import VueRouter from "vue-router";
import Catalogue from "./pages/Catalogue.vue";
import Product from "./pages/Product.vue";
import Cart from "./pages/Cart.vue";
import Checkout from "./pages/Checkout.vue";
import Account from "./pages/Account.vue";

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
      component: Cart,
      meta: {
        role: "Customer"
      }
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
      name: "account",
      path: "/account",
      component: Account,
      meta: {
        requiresAuth: true,
        role: "Customer"
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
      if (
        to.matched.some(
          route => route.meta.role && store.getters.isInRole(route.meta.role)
        )
      ) {
        next();
      } else if (!to.matched.some(route => route.meta.role)) {
        next();
      } else {
        next({
          path: "/"
        });
      }
    }
  } else {
    if (
      to.matched.some(
        route =>
          route.meta.role &&
          (!store.getters.isAuthenticated ||
            store.getters.isInRole(route.meta.role))
      )
    ) {
      next();
    } else {
      if (to.matched.some(route => route.meta.role)) {
        next({
          path: "/"
        });
      } else {
        next();
      }
    }
  }
});

router.afterEach(() => {
  NProgress.done();
});
export default router;
