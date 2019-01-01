<template>
  <form @submit.prevent="login" class="p-2">
    <b-alert
      variant="danger"
      :show="error !== null"
      dismissible
      @dismissed="error = null"
      >{{ error }}</b-alert
    >
    <b-alert variant="success" :show="registered && error === null"
      >Registration successful. Please login to continue.</b-alert
    >
    <p>Login with your email address and password.</p>
    <b-form-group label="E-mail">
      <b-form-input v-model.trim="email" />
    </b-form-group>
    <b-form-group label="Password">
      <b-form-input v-model.trim="password" type="password" />
    </b-form-group>
    <b-form-group>
      <b-button variant="success" type="submit" :disabled="loading"
        >Login</b-button
      >
      <b-button variant="danger" @click="close" :disabled="loading"
        >Cancel</b-button
      >
    </b-form-group>
  </form>
</template>

<script>
export default {
  name: "login-form",
  props: {
    registered: {
      type: Boolean,
      required: false
    }
  },
  data() {
    return {
      email: "",
      password: "",
      error: null
    };
  },
  computed: {
    loading() {
      return this.$store.state.loading;
    }
  },
  methods: {
    login() {
      const payload = {
        email: this.email,
        password: this.password
      };
      this.$store
        .dispatch("login", payload)
        .then(() => {
          this.error = null;
          this.email = "";
          this.password = "";

          if (this.$route.query.redirect) {
            this.$router.push(this.$route.query.redirect);
          }
        })
        .catch(error => {
          console.log(error);
          this.error = error.data;
        });
    },
    close() {
      this.$emit("close");
    }
  }
};
</script>
