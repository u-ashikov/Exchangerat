import Vue from 'vue'
import Vuex from 'vuex'
import users from '../../queries/users.js'

Vue.use(Vuex);

const state = {
    userId: null,
    username: null,
    token: null
};

const getters = {
    username: function (state) {
        return state.username;
    },
    isAuthenticated: function (state) {
        return state.username !== null;
    }
};

const mutations = {
    login: function (state, userData) {
        state.userId = userData.id;
        state.username = userData.username;
        state.token = userData.token;
    },
    register: function (state, userData) {
        state.userId = userData.id;
        state.username = userData.username;
        state.token = userData.token;
    },
    clearUserData: function (state) {
        state.userId = null;
        state.username = null;
        state.token = null;
    }
};

const actions = {
    login: function (context, userData) {
        return new Promise(function (resolve, reject) {
            users.login(userData)
                .then(function (response) {
                    if (response && response.status == 200) {
                        context.commit('login', response.data);

                        context.dispatch('setLocalStorageUserData', response.data);
                    }

                    resolve(response);
                })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    tryAutoLogin: function (context) {
        var token = localStorage.getItem('token');

        if (!token) {
            return;
        }

        // TODO: Check if the token has expired.

        var userId = localStorage.getItem('userId');
        var username = localStorage.getItem('username');

        var userData = {
            userId: userId,
            username: username,
            token: token
        };

        context.commit('login', userData);
    },
    register: function (context, userData) {
        return new Promise(function (resolve, reject) {
            users.register(userData)
                .then(function (response) {
                    if (response && response.status == 200) {
                        context.commit('register', response.data);
                    
                        context.dispatch('setLocalStorageUserData', response.data);
                    }
                
                    resolve(response);
                })
                .catch(function (error) {
                    reject(error);
                });
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
        localStorage.setItem('username', userData.username);
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