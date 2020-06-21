import VueRouter from 'vue-router'

import Register from '../components/identity/Register'
import Login from '../components/identity/Login'

const router = new VueRouter({
    routes: [
        { path: '/users/register', name: 'register', component: Register },
        { path: '/users/login', name: 'login', component: Login }
    ],
    mode: "history"
});

export { router }