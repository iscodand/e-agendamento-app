import { authStore } from '@/stores/auth'
import type { NavigationGuardNext, RouteLocationNormalized } from 'vue-router';

export default async function routes(
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) {
  if (to.meta?.auth) {
    const useAuth = authStore();
    const isAuthenticated = await useAuth.isAuthenticated();
    if (isAuthenticated) {
      next();
    } else {
      next({ name: 'login' });
    }
  } else {
    next();
  }
}