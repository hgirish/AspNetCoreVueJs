<template>
  <h2>Logout</h2>
</template>

<script>
export default {
  name: "logout",
  mounted() {
    this.$store.dispatch("logout").then(() => {
      this.$router.push("/");
    });
  }
};
</script>
