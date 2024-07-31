<script setup lang="ts">
import type { User } from '@/services/user/types';
import { UsersIcon, TagIcon, BuildingOffice2Icon, ClockIcon, SwatchIcon, HomeIcon } from "@heroicons/vue/24/outline";
import ItemListComponent from '@/components/ui/layout/navbar/ItemListComponent.vue';
import ProfileDropdownComponent from './navbar/ProfileDropdownComponent.vue';

const props = defineProps<{ user: User }>();

const isSuperAdmin: Boolean = props.user.roles.includes('SuperAdmin');
const isAdmin: Boolean = props.user.roles.includes('Admin');
</script>

<template>
  <div>
    <ProfileDropdownComponent :user="user" />
  </div>

  <div class="mt-20">
    <aside id="logo-sidebar"
      class="fixed left-0 z-40 w-64 pt-8 h-screen transition-transform -translate-x-full border-r border-gray-200 sm:translate-x-0 bg-gray-800"
      aria-label="Sidebar">
      <div class="px-3">
        <ul class="space-y-2 font-medium">
          <ItemListComponent link="/home" title="Home" :icon="HomeIcon" />
          <ItemListComponent link="/meus-agendamentos" title="Meus Agendamentos" :icon="ClockIcon" />
          <ItemListComponent v-if="isAdmin || isSuperAdmin" link="/agendamentos" title="Todos os Agendamentos"
            :icon="ClockIcon" />
          <ItemListComponent link="/categorias" title="Categorias" :icon="SwatchIcon" />
          <ItemListComponent link="/items" title="Itens" :icon="TagIcon" />
          <!-- <ItemListComponent v-if="isAdmin || isSuperAdmin" link="/funcionarios" title="Funcionários"
        :icon="UsersIcon" /> -->
          <ItemListComponent v-if="isAdmin || isSuperAdmin" link="/empresas" title="Empresas"
            :icon="BuildingOffice2Icon" />
        </ul>
      </div>
    </aside>
  </div>
</template>