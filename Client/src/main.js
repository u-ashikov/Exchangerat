import Vue from 'vue'
import App from './App.vue'
import VueRouter from 'vue-router'
import { router } from './routing/router'
import Vuelidate from 'vuelidate'
import { store } from './store/store.js'

import 'bootstrap'
import 'bootstrap/dist/css/bootstrap.min.css' 
import '@fortawesome/fontawesome-free/css/all.min.css'
import 'nprogress/nprogress.css';
import 'nprogress/nprogress';

Vue.use(VueRouter);
Vue.use(Vuelidate);

store.dispatch('tryAutoLogin');

new Vue({
  el: '#app',
  router,
  store,
  render: h => h(App)
});