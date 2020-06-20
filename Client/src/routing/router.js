import VueRouter from 'vue-router'

import Register from '../components/identity/Register'

const router = new VueRouter({
    routes: [
        { path: '/users/register', name: 'register', component: Register }
    ],
    mode: "history"
});

export { router }