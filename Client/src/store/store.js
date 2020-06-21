import Vue from 'vue'
import Vuex from 'vuex'
import userAuthModule from './modules/user-auth.js'

Vue.use(Vuex);

export const store = new Vuex.Store({
    modules: [userAuthModule]
});