<template>
  <div id="app">
    <app-header></app-header>
    <router-view></router-view>
    <app-footer></app-footer>
  </div>
</template>

<script>
import Header from "./components/shared/Header";
import Footer from "./components/shared/Footer";

export default {
  name: "app",
  components: {
    appHeader: Header,
    appFooter: Footer
  },
  data: function() {
    return {
      msg: "Welcome to the Exchangerat"
    };
  }
};
</script>

<style>
    [v-cloak] {
      display: none;
    }

    input.invalid,
    textarea.invalid {
        border: 1px solid #fd4864;
        background-color: #FFEEEE;
    }
</style>
