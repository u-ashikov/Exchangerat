import VueRouter from 'vue-router'

import Register from '../components/identity/Register'
import Login from '../components/identity/Login'

import { store } from '../store/store.js'

const router = new VueRouter({
    routes: [
        { path: '/users/register', name: 'register', component: Register },
        { path: '/users/login', name: 'login', component: Login }
    ],
    mode: "history"
});

router.beforeEach(function (to, from, next) {
    var isAuthenticated = store.getters.isAuthenticated;

    if (!isAuthenticated
        && to.path !== '/users/login'
        && to.path !== '/users/register'
        && to.path !== '/'
    ) {
        next({ name: 'login' })
    } else {
        if (isAuthenticated && (to.path === '/users/login' || to.path === '/users/register')) {
            next('/');
        } else {
            next();
        }
    }
})

export { router }