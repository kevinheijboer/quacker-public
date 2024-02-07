import Vue from 'vue';
import VueRouter from 'vue-router';
import Quacks from '@/components/Quacks.vue';
import store from '../store';
import Login from '@/views/Login.vue';
import Profile from '@/components/Profile.vue';
import Following from '@/components/Following.vue';
import Followers from '@/components/Followers.vue';

Vue.use(VueRouter);

const routes = [{
        path: '/',
        alias: '/home',
        name: 'Home',
        component: Quacks,
        beforeEnter(to, from, next) {
            const user = store.state.auth.user;
            if (user) {
                next();
            } else {
                next('/login');
            }
        },
    },
    {
        path: '/profile/:username',
        name: 'Profile',
        component: Profile,
        beforeEnter(to, from, next) {
            const user = store.state.auth.user;
            if (user) {
                next();
            } else {
                next('/login');
            }
        },
    },
    {
        path: '/profile/:username/following',
        name: 'Following',
        component: Following,
        beforeEnter(to, from, next) {
            const user = store.state.auth.user;
            if (user) {
                next();
            } else {
                next('/login');
            }
        },
    },
    {
        path: '/profile/:username/followers',
        name: 'Followers',
        component: Followers,
        beforeEnter(to, from, next) {
            const user = store.state.auth.user;
            if (user) {
                next();
            } else {
                next('/login');
            }
        },
    },
    {
        path: '/login',
        name: 'Login',
        component: Login,
        beforeEnter(to, from, next) {
            const user = store.state.auth.user;
            if (user) {
                next('/home');
            } else {
                next();
            }
        },
    },
];

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes,
});

export default router;