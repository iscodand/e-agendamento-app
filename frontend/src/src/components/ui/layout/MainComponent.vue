Copiar código
<script lang="ts" setup>
import { authStore } from '@/stores/auth';
import type { User } from '@/services/user/types';
import NavComponent from './NavComponent.vue';
import { onMounted, ref } from 'vue';
import router from '@/router';
import LoadingComponent from './LoadingComponent.vue';

const useAuth = authStore();
let user = ref<User | null>(null);
const isLoading = ref<boolean>(true);

onMounted(async () => {
  try {
    const getUser = await useAuth.dispatchGetAuthenticatedUser();
    user.value = getUser.data!;
  } catch (error) {
    useAuth.clear();
    router.push('/login');
  } finally {
    isLoading.value = false;
  }
});
</script>

<template>
  <div v-if="isLoading">
    <div class="app-container">
      <LoadingComponent :is-loading="isLoading" class="loading-component" />
    </div>
  </div>

  <div v-else>
    <NavComponent :user="user!" />
  </div>
</template>

<style scoped>
.app-container {
  position: relative;
  width: 100%;
  height: 100vh;
}

.loading-component {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: rgba(255, 255, 255, 0.8);
}
</style>