import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/account/LoginView.vue')
    },
    {
      path: '/items',
      name: 'item',
      component: () => import('../views/items/ItemsView.vue')
    }
  ]
})

export default router
