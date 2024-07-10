<script lang="ts" setup>
import { authStore } from '@/stores/auth';
import type { User } from '@/services/user/types';
import NavComponent from './NavComponent.vue'

const useAuth = authStore();

const user: User = {
  username: useAuth.userName,
  email: "",
  roles: useAuth.getRoles,
  company: 'Empresa Foda'
};

console.log(user);

</script>

<template>
  <div>
    <NavComponent :user="user" />
  </div>
</template>
