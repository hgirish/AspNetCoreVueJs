<template>
  <div>
    <h3 class="float-left">Products</h3>
    <b-button variant="primary" to="/admin/products/create" class="float-right mb-4">
      <i class="fas fa-plus"></i>
      Add new Product
    </b-button>
    <table class="table table-striped table-hover">
      <thead>
        <tr>
          <th>#</th>
          <th>Name</th>
          <th>Short Description</th>
          <th>Price</th>
        </tr>
      </thead>
      <tbody>
        <template v-if="products && producs.length > 0">
          <tr v-for="product in products" :key="product.id">
            <td>{{product.id}}</td>
            <td>{{product.name}}</td>
            <td>{{product.shortDescription}}</td>
            <td>{{product.price|currency}}</td>
          </tr>
        </template>
        <tr v-else>
          <td colspan="4">There are no products to display.</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "products",
  data() {
    return {
      products: null
    };
  },
  methods: {
    setData(products) {
      this.products = products;
    }
  },
  beforeRouteEnter(to, from, next) {
    // eslint-disable-next-line no-unused-vars
    const vm = this;
    axios.get("/api/products").then(response => {
      next(vm => vm.setData(response.data));
    });
  }
};
</script>
