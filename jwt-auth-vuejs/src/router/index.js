import { createRouter, createWebHistory } from 'vue-router' 
import LoginView from "@/pages/LoginView";
import RegisterView from "@/pages/RegisterView"; 
import HomeView from "@/pages/HomeView"; 
import ProductGridView from "@/pages/ProductGridView";

const routes = [ 
    { path: '/', component: HomeView },
    { path: '/login', component: LoginView }, 
    { path: '/productgrid', component: ProductGridView },
    { path: '/register', component: RegisterView },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
