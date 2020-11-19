import Vue from 'vue'
import Router from 'vue-router'
import auth from './auth'
import Home from './views/Home.vue'
import Login from './views/Login.vue'
import Register from './views/Register.vue'
import Review from './views/Review.vue'
import Detail from './views/Detail.vue'
import Report from './views/Report.vue'
import Claim from './views/Claim.vue'
import ViewClaims from './views/AllClaims.vue'

Vue.use(Router)

/**
 * The Vue Router is used to "direct" the browser to render a specific view component
 * inside of App.vue depending on the URL.
 *
 * It also is used to detect whether or not a route requires the user to have first authenticated.
 * If the user has not yet authenticated (and needs to) they are redirected to /login
 * If they have (or don't need to) they're allowed to go about their way.
 */

const router = new Router({
    mode: 'history',
    base: process.env.BASE_URL,
    routes: [{
            path: '/',
            name: 'home',
            component: Home,
            meta: {
                requiresAuth: false
            }
        },
        {
            path: "/login",
            name: "login",
            component: Login,
            meta: {
                requiresAuth: false
            }
        },
        {
            path: "/register",
            name: "register",
            component: Register,
            meta: {
                requiresAuth: false
            }
        },
        {
            path: "/review",
            name: "review",
            component: Review,
            meta: {
                requiresAuth: true,
                requiresRole: "Employee"
            }
        },
        {
            path: "/detail/:id",
            name: "detail",
            component: Detail,
            meta: {
                requiresAuth: true,
            }
        },
        {
            path: "/report",
            name: "report",
            component: Report,
            meta: {
                requiresAuth: true
            }
        },
        {
            path: "/claim/:id",
            name: "claim",
            component: Claim,
            meta: {
                requiresAuth: true
            }
        },
        {
            path: "/claim/viewAllClaims",
            name: "viewClaims",
            component: ViewClaims,
            meta: {
                requiresAuth: true,
                requiresRole: "Employee"
            }
        }

    ]
})

router.beforeEach((to, from, next) => {
    // Determine if the route requires Authentication
    const requiresAuth = to.matched.some(x => x.meta.requiresAuth);
    const requiresRole = to.matched.some(x => x.meta.requiresRole);

    const user = auth.getUser();

    if (!requiresAuth) {
        next();
        return;
    }
    if (!user) {
        next("/login");
        return;
    }
    if (!requiresRole) {
        next();
        return;
    }
    if (user.rol === to.meta.requiresRole) {
        next();
        return;
    }
    next("/");

    // If it does and they are not logged in, send the user to "/login"
    // if (requiresAuth && !user) {
    //   next("/login");
    // } else {
    //   // Else let them go to their next destination
    //   next();
    // }
});


export default router;