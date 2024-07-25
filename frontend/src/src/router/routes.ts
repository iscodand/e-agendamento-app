import { authStore } from '@/stores/auth'
import type { NavigationGuardNext, RouteLocationNormalized } from 'vue-router';

export default async function routes(
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) {
  console.log('to.meta.auth:', to.meta.auth);  // Verificação
  if (to.meta?.auth) {
    const useAuth = authStore();
    const isAuthenticated = await useAuth.isAuthenticated;
    console.log('isAuthenticated:', isAuthenticated);  // Verificação
    if (isAuthenticated) {
      next();
    } else {
      next({ name: 'login' });
    }
  } else {
    next();
  }
}