import Vue from 'vue';
import VueRouter from 'vue-router';
import Catalogue from './pages/Catalogue.vue'
import Product from './pages/Product.vue'
import NProgress from 'nprogress'

Vue.use(VueRouter);

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [


    {
      name: 'products',
      path: '/products',
      component: Catalogue,

    },
    {
      name: 'product-detail',
      path: '/products/:slug',
      component: Product
    },
    {
      path: '*',
      redirect: '/products'
    }
  ]
});
router.beforeEach((to, from, next) => {
  NProgress.start();
  next();
});

router.afterEach((to, from) => {
  NProgress.done();
});
export default router;
