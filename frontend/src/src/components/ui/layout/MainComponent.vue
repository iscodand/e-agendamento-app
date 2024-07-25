<script lang="ts" setup>
import { authStore } from '@/stores/auth';
import type { User } from '@/services/user/types';
import NavComponent from './NavComponent.vue';
import { onMounted, ref } from 'vue';

const useAuth = authStore();
let user: User | null = null;
const isLoading = ref<boolean>(true);

onMounted(async () => {
  try {
    const getUser = await useAuth.dispatchGetAuthenticatedUser();
    user = getUser.data!;
  } catch (error) {
    console.error('Failed to load user data:', error);
  } finally {
    isLoading.value = false;
  }
});
</script>

<template>
  <div>
    <NavComponent v-if="!isLoading" :user="user!" />
  </div>
</template>
