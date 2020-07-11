import Vue from 'vue'
import Vuex from 'vuex'
import identityService from '../../services/identityService.js';

Vue.use(Vuex);

const state = {
    clientId: null,
    username: null,
    token: null
};

const getters = {
    username: function (state) {
        return state.username;
    },
    clientId: function (state) {
        return state.clientId;
    },
    isAuthenticated: function (state) {
        return state.username !== null;
    }
};

const mutations = {
    login: function (state, userData) {
        state.username = userData.username;
        state.token = userData.token;
    },
    register: function (state, userData) {
        state.username = userData.username;
        state.token = userData.token;
    },
    clearUserData: function (state) {
        state.username = null;
        state.token = null;
    },
    clearClientData: function (state) {
        state.clientId = null;
    },
    setClientData: function (state, clientId) {
        state.clientId = clientId;
    }
};

const actions = {
    login: function (context, userData) {
        return new Promise(function (resolve, reject) {
            identityService.login(userData)
                .then(function (response) {
                    if (response && response.status == 200) {
                        context.commit('login', response.data);
                        context.dispatch('setLocalStorageUserData', response.data);

                        context.dispatch('setLogoutTimer', 86400);
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
        
        var tokenExpirationDate = localStorage.getItem('expirationDate');

        if (!tokenExpirationDate || tokenExpirationDate < new Date()) {
            return;
        }

        var username = localStorage.getItem('username');
        var clientId = localStorage.getItem('clientId');

        var userData = {
            username: username,
            token: token
        };

        context.commit('login', userData);
        context.commit('setClientData', { id: clientId });
    },
    register: function (context, userData) {
        return new Promise(function (resolve, reject) {
            identityService.register(userData)
                .then(function (response) {
                    if (response && response.status == 200) {
                        context.commit('register', response.data);              
                        context.dispatch('setLocalStorageUserData', response.data);

                        context.dispatch('setLogoutTimer', 86400);
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
        context.commit('clearClientData');

        localStorage.clear();
    },
    setLocalStorageUserData: function (context, userData) {
        var now = new Date();
        var expirationDate = new Date(now.getTime() + (86400 * 1000));

        localStorage.setItem('username', userData.username);
        localStorage.setItem('token', userData.token);
        localStorage.setItem('expirationDate', expirationDate);
    },
    setLocalStorageClientData: function (context, clientId) {
        localStorage.setItem('clientId', clientId);
    },
    setLogoutTimer: function (context, expirationTime) {
        setTimeout(() => {
            context.commit('clearUserData');
            context.commit('clearClientData');

            localStorage.clear();
        }, expirationTime * 1000);
    }
};

export default {
    state,
    getters,
    mutations,
    actions
}