import Main from "@/pages/Main.vue";
import Calendar from "@/pages/Calendar.vue"
import Subscriptions from "@/pages/Subscriptions.vue"
import GroupsManaging from "@/pages/GroupsManaging.vue"
import EventsManaging from "@/pages/EventsManaging.vue"
import Settings from "@/pages/Settings.vue"
import Events from "@/pages/Events.vue"
import About from "@/pages/About.vue"
import EventDetails from "@/pages/EventDetails.vue"
import GroupDetails from "@/pages/GroupDetails.vue"
import Editor from "@/pages/Editor.vue"
import Preview from "@/pages/Preview.vue"
import Admin from "@/pages/Admin.vue"
import {createRouter, createWebHistory} from "vue-router";

const routes = [
    {
        path: '/',
        component: Events
    },
    {
        path: '/calendar',
        component: Calendar
    },
    {
        path: '/subscriptions',
        component: Subscriptions
    },
    {
        path: '/groups/my',
        component: GroupsManaging
    },
    {
        path: '/events/my',
        component: EventsManaging
    },
    {
        path: '/settings',
        component: Settings
    },
    {
        path: '/events',
        component: Events
    },
    {
        path: '/about',
        component: About
    },
    {
        path: '/events/:id',
        component: EventDetails
    },
    {
        path: '/groups/:id',
        component: GroupDetails
    },
    {
        path: '/events/:id/editor',
        component: Editor
    },
    {
        path: '/events/:id/preview',
        component: Preview
    },
    {
        path: '/admin',
        component: Admin
    }

]

const router = createRouter({
    routes,
    history: createWebHistory(process.env.BASE_URL)
});
const publicPages = [
    /^\/about$/,
    /^\/events$/,
    /^\/events\/\d+$/ // Соответствует /events/1, /events/2 и т.д.
];
router.beforeEach((to, from, next) => {
    const isPublicPage = publicPages.some(regex => regex.test(to.path));
    const loggedIn = localStorage.getItem('user');

    if (!isPublicPage && !loggedIn) {
        next('/about');
    } else {
        next();
    }
});
// router.beforeEach((to, from, next) => {
//     const publicPages = ['/about', '/events', '/events/:id'];
//     const authRequired = !publicPages.includes(to.path);
//     const loggedIn = localStorage.getItem('user');
//
//     if (authRequired && !loggedIn) {
//         next('/about');
//     } else {
//         next();
//     }
// });

export default router;