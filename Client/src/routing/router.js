import VueRouter from 'vue-router'
import NProgress from 'nprogress'

import Index from '../components/home/Index'
import Register from '../components/identity/Register'
import Login from '../components/identity/Login'
import MyAccounts from '../components/exchange-account/MyAccounts'
import AccountDetails from '../components/exchange-account/AccountDetails'
import CreateTransaction from '../components/transaction/Create'
import RequestForm from '../components/request/RequestForm'
import AddFunds from '../components/funds/AddFunds'
import MyFunds from '../components/funds/MyFunds'
import MyRequests from '../components/request/MyRequests'

import { store } from '../store/store.js'

const router = new VueRouter({
    routes: [
        { path: '/', name: 'home', component: Index },
        { path: '/users/register', name: 'register', component: Register },
        { path: '/users/login', name: 'login', component: Login },
        { path: '/exchange-accounts/my', name: 'myAccounts', component: MyAccounts },
        { path: '/exchange-accounts/my/:accountId', name: 'accountTransactions', component: AccountDetails },
        { path: '/transactions/create', name: 'createTransaction', component: CreateTransaction },
        { path: '/exchange-accounts/request', name: 'createRequest', component: RequestForm },
        { path: '/funds/add', name: 'add-funds', component: AddFunds },
        { path: '/funds/my', name: 'my-funds', component: MyFunds },
        { path: '/requests/my', name: 'my-requests', component: MyRequests }
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
  
router.afterEach((to, from) => {
    NProgress.done();
})

router.beforeResolve((to, from, next) => {
    if (to.name) {
        NProgress.start()
    }
    next();
})

export { router }