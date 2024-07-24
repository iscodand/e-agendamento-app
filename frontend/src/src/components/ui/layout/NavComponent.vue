<script setup lang="ts">
import type { User } from '@/services/user/types';
import { authStore } from '@/stores/auth';
import { UsersIcon, TagIcon, BuildingOffice2Icon, ClockIcon, SwatchIcon, HomeIcon } from "@heroicons/vue/24/outline";
import ItemListComponent from '@/components/ui/layout/navbar/ItemListComponent.vue';
import router from '@/router';
import ProfileDropdownComponent from './navbar/ProfileDropdownComponent.vue';

const useAuth = authStore();

const props = defineProps<{ user: User }>();

const isSuperAdmin: Boolean = props.user.roles.includes('SuperAdmin');
const isAdmin: Boolean = props.user.roles.includes('Admin');
</script>

<template>
  <div>
    <ProfileDropdownComponent :user="user" />

    <aside id="logo-sidebar"
      class="fixed left-0 z-40 w-64 h-screen pt-10 transition-transform -translate-x-full border-r border-gray-200 sm:translate-x-0 bg-gray-800 border-gray-700"
      aria-label="Sidebar">
      <div class="h-full px-3 pb-4 overflow-y-auto bg-gray-800">
        <ul class="space-y-2 font-medium">
          <ItemListComponent link="/home" title="Home" :icon="HomeIcon" />
          <ItemListComponent link="/meus-agendamentos" title="Meus Agendamentos" :icon="ClockIcon" />
          <ItemListComponent v-if="isAdmin || isSuperAdmin" link="/agendamentos" title="Todos os Agendamentos"
            :icon="ClockIcon" />
          <ItemListComponent link="/categorias" title="Categorias" :icon="SwatchIcon" />
          <ItemListComponent link="/items" title="Itens" :icon="TagIcon" />
          <ItemListComponent v-if="isAdmin || isSuperAdmin" link="/funcionarios" title="Funcionários"
            :icon="UsersIcon" />
          <ItemListComponent v-if="isAdmin || isSuperAdmin" link="/empresas" title="Empresas"
            :icon="BuildingOffice2Icon" />
        </ul>
      </div>
    </aside>
  </div>
</template>