import VueRouter from 'vue-router'

import Register from '../components/identity/Register'
import Login from '../components/identity/Login'
import MyAccounts from '../components/exchange-account/MyAccounts'
import AccountDetails from '../components/exchange-account/AccountDetails'
import CreateTransaction from '../components/transaction/Create'

import { store } from '../store/store.js'

const router = new VueRouter({
    routes: [
        { path: '/users/register', name: 'register', component: Register },
        { path: '/users/login', name: 'login', component: Login },
        { path: '/exchange-accounts/my', name: 'myAccounts', component: MyAccounts },
        { path: '/exchange-accounts/my/:accountId', name: 'accountTransactions', component: AccountDetails },
        { path: '/transactions/create', name: 'createTransaction', component: CreateTransaction }
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