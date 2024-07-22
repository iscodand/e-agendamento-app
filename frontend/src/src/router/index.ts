import { createRouter, createWebHistory } from 'vue-router'
import routes from './routes';

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/login',
            name: 'login',
            component: () => import('../views/account/LoginView.vue')
        },
        {
            path: '/meus-agendamentos',
            name: 'my-schedules',
            component: () => import('../views/schedules/MySchedulesView.vue')
        },
        {
            path: '/items',
            name: 'item',
            component: () => import('../views/items/ItemsView.vue'),
            meta: {
                auth: true
            }
        },
        {
            path: '/categorias',
            name: 'category',
            component: () => import('../views/categories/CategoriesView.vue'),
            meta: {
                auth: true
            }
        },
        {
            path: '/funcionarios',
            name: 'employees',
            component: () => import('../views/employees/EmployeesView.vue'),
            meta: {
                auth: true
            }
        },
        {
            path: '/empresas',
            name: 'companies',
            component: () => import('@/views/companies/CompaniesView.vue'),
            meta: {
                auth: true
            }
        }
    ]
})

router.beforeEach(routes);

export default router
