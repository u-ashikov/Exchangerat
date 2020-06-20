<template>
  <div id="app">
    <app-header></app-header>
    <h1 class="text-center mt-4">{{msg}}</h1>
    <app-footer></app-footer>
  </div>
</template>

<script>
import Header from './components/shared/Header'
import Footer from './components/shared/Footer'

export default {
  name: 'app',
  components: {
    appHeader: Header,
    appFooter: Footer
  },
  data: function () {
    return {
      msg: 'Welcome to the Exchangerat'
    }
  }
}
</script>
