import Vue from 'vue'
import Vuex from 'vuex'
import users from '../../queries/users.js'

Vue.use(Vuex);

const state = {
    user: null,
    token: null
};

const getters = {
    user: function (state) {
        return state.user;
    },
    isAuthenticated: function (state) {
        return state.user !== null;
    }
};

const mutations = {
    login: function (state, userData) {
        state.user = userData;
        state.token = userData.token;
    },
    register: function (state, userData) {
        state.user = userData.user;
        state.token = userData.token;
    },
    clearUserData: function (state) {
        state.user = null;
        state.token = null;
    }
};

const actions = {
    login: function (context, userData) {
        users.login(userData)
            .then(function (response) {
                if (response && response.status == 200) {
                    console.log(response.data);
                    context.commit('login', response.data);

                    context.dispatch('setLocalStorageUserData', response.data);
                }
            })
            .catch(function (error) {
                console.log(error);
            });
    },
    register: function (context, userData) {
        users.register(userData)
            .then(function (response) {
                if (response && response.status == 200) {
                    console.log(response.data);
                    context.commit('register', response.data);

                    context.dispatch('setLocalStorageUserData', response.data);
                }
            })
            .catch(function (error) {
                // TODO: Display general error.
                console.log(error);
            });
    },
    logout: function (context) {
        if (context.state.user === null) {
            return;
        }

        context.commit('clearUserData');

        localStorage.clear();
    },
    setLocalStorageUserData: function (context, userData) {
        // TODO: Set expiration date.
        // var now = new Date();
        // var expirationDate = new Date(now.getTime() + )

        localStorage.setItem('userId', userData.id);
        localStorage.setItem('token', userData.token);

        // TODO: Store also the expiration.
        // localStorage.setItem('userId', response.data.id);
    }
};

export default {
    state,
    getters,
    mutations,
    actions
}