<script setup lang="ts">
import { RouterLink, RouterView, useRouter } from "vue-router";
import { authStore } from "./stores/auth";
import { onMounted, onUnmounted, ref } from "vue";
import LoadingComponent from '@/components/ui/layout/LoadingComponent.vue'

const useAuth = authStore();
useAuth.refresh();

const loading = ref<Boolean>(false);
const router = useRouter();

const startLoading = () => { loading.value = true; };
const stopLoading = () => { loading.value = false; };

onMounted(() => {
  router.beforeEach((to, from, next) => {
    startLoading();
    next();
  });

  router.afterEach(() => {
    stopLoading();
  });
});

onUnmounted(() => {
  router.beforeEach(() => { });
  router.afterEach(() => { });
});
</script>

<template>
  <RouterView />
  <LoadingComponent :isLoading="loading" />
  <div id="modals"></div>
</template>
