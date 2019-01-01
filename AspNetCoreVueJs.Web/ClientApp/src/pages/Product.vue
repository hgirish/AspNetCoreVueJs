<template>
  <div class="page"><product-details :product="product" v-if="product" /></div>
</template>

<script>
import ProductDetails from "../components/product/Details.vue";
import axios from "axios";

export default {
  name: "product",
  components: {
    ProductDetails
  },
  data() {
    return {
      product: null
    };
  },
  beforeRouteEnter(to, from, next) {
    const slug = to.params.slug;

    axios.get(`/api/products/${slug}`).then(response => {
      next(vm => vm.setData(response.data));
    });
  },
  methods: {
    setData(product) {
      this.product = product;
    }
  }
};
</script>
