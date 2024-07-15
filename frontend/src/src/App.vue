<script setup lang="ts">
import { RouterLink, RouterView } from "vue-router";
import { authStore } from "./stores/auth";

const useAuth = authStore();
useAuth.refresh();

</script>

<template>
  <RouterView />
  <div id="modals"></div>
</template>
