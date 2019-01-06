<template>
  <div id="app">
    <link
      rel="stylesheet"
      href="https://use.fontawesome.com/releases/v5.6.3/css/all.css"
      integrity="sha384-UHRtZLI+pbxtHCWp1t77Bi1L4ZtiqrqD80Kn4Z8NTSRyMA2Fd33n5dQ8lWUE00s/"
      crossorigin="anonymous"
    />

    <div class="app">
      <b-navbar toggleable="md" type="dark" variant="dark">
        <b-navbar-toggle target="nav_collapse"></b-navbar-toggle>
        <b-navbar-brand href="/">PhoneShop</b-navbar-brand>
        <b-collapse is-nav id="nav_collapse">
          <b-navbar-nav>
            <b-nav-item to="/products">Products</b-nav-item>
            <b-nav-item v-if="isAdmin" to="/admin">Admin</b-nav-item>
          </b-navbar-nav>
          <b-navbar-nav class="ml-auto mr-4">
            <cart-summary v-if="isCustomer" />
            <auth-nav-item />
          </b-navbar-nav>
        </b-collapse>
      </b-navbar>
      <transition name="fade" mode="out-in"> <router-view /> </transition>
    </div>
    <auth-modal :show="showAuthModal" />
  </div>
</template>

<script>
import CartSummary from "./components/cart/CartSummary.vue";
import AuthNavItem from "./components/app/AuthNavItem.vue";
import AuthModal from "./components/app/AuthModal.vue";

export default {
  name: "app",
  components: {
    AuthNavItem,
    AuthModal,
    CartSummary
  },

  computed: {
    showAuthModal() {
      return this.$store.state.showAuthModal;
    },
    isCustomer() {
      return (
        this.$store.getters.isInRole("Customer") ||
        !this.$store.getters.isAuthenticated
      );
    },
    isAdmin() {
      return this.$store.getters.isInRole("Administrator");
    }
  },
  created() {
    this.$store.dispatch("setStripeKey");
  }
};
</script>

<style lang="scss">
html,
body {
  height: 100vh;
}

div.app,
div.page {
  height: 100% !important;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease-in-out;
}

.fade-enter,
.fade-leave-to {
  opacity: 0;
}
</style>
