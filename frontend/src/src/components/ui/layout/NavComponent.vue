<script setup lang="ts">
import type { User } from '@/services/user/types';
import { authStore } from '@/stores/auth';
import { UsersIcon, TagIcon, BuildingOffice2Icon, ClockIcon, SwatchIcon } from "@heroicons/vue/24/outline";
import ItemListComponent from '@/components/ui/layout/navbar/ItemListComponent.vue'
import router from '@/router';

const useAuth = authStore();

const props = defineProps<{ user: User }>();

async function handleLogout() {
  useAuth.clear();
  router.push('login/');
}

const isSuperAdmin: Boolean = props.user.roles.includes('SuperAdmin');
const isAdmin: Boolean = props.user.roles.includes('Admin');
</script>

<template>
  <div>
    <nav class="fixed top-0 z-50 w-full border-b border-gray-200 bg-gray-800 border-gray-700">
      <div class="px-3 py-3 lg:px-5 lg:pl-3">
        <div class="flex items-center justify-between">
          <div class="flex items-center justify-start rtl:justify-end">
            <button data-drawer-target="logo-sidebar" data-drawer-toggle="logo-sidebar" aria-controls="logo-sidebar"
              type="button"
              class="inline-flex items-center p-2 text-sm text-gray-500 rounded-lg sm:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 text-gray-400 hover:bg-gray-700 focus:ring-gray-600">
              <span class="sr-only">Open sidebar</span>
              <svg class="w-6 h-6" aria-hidden="true" fill="currentColor" viewBox="0 0 20 20"
                xmlns="http://www.w3.org/2000/svg">
                <path clip-rule="evenodd" fill-rule="evenodd"
                  d="M2 4.75A.75.75 0 012.75 4h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 4.75zm0 10.5a.75.75 0 01.75-.75h7.5a.75.75 0 010 1.5h-7.5a.75.75 0 01-.75-.75zM2 10a.75.75 0 01.75-.75h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 10z">
                </path>
              </svg>
            </button>
            <a class="flex ms-2 md:me-24">
              <img src="https://flowbite.com/docs/images/logo.svg" class="h-8 me-3" alt="FlowBite Logo" />
              <span
                class="self-center text-xl font-semibold sm:text-2xl whitespace-nowrap text-white">E-Agendamento</span>
            </a>
          </div>
          <div class="flex items-center">
            <div class="flex items-center ms-3">
              <span class="username mr-4 text-white">Olá, {{ user.username }}</span>
              <div>
                <button type="button"
                  class="flex text-sm bg-gray-800 rounded-full focus:ring-4 focus:ring-gray-300 focus:ring-gray-600"
                  aria-expanded="false" data-dropdown-toggle="dropdown-user">
                  <span class="sr-only">Open user menu</span>
                  <img class="w-8 h-8 rounded-full" src="https://flowbite.com/docs/images/people/profile-picture-5.jpg"
                    alt="user photo" />
                </button>
              </div>
              <div
                class="z-50 hidden my-4 text-base list-none divide-y divide-gray-100 rounded shadow bg-gray-700 divide-gray-600"
                id="dropdown-user">
                <div class="px-4 py-3" role="none">
                  <p class="text-sm text-gray-900 text-white" role="none">{{ user.roles }}</p>
                  <p class="text-sm font-medium text-gray-900 truncate text-gray-300" role="none">
                    {{ user.email }}
                  </p>
                </div>
                <ul class="py-1" role="none">
                  <li>
                    <a href="#"
                      class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 text-gray-300 hover:bg-gray-600 hover:text-white"
                      role="menuitem">Meu Perfil</a>
                  </li>
                  <li>
                    <button @click="handleLogout" type="submit"
                      class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 text-gray-300 hover:bg-gray-600 hover:text-white"
                      role="menuitem">
                      Sair
                    </button>
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </div>
      </div>
    </nav>

    <aside id="logo-sidebar"
      class="fixed top-0 left-0 z-40 w-64 h-screen pt-20 transition-transform -translate-x-full border-r border-gray-200 sm:translate-x-0 bg-gray-800 border-gray-700"
      aria-label="Sidebar">
      <div class="h-full px-3 pb-4 overflow-y-auto bg-gray-800">
        <ul class="space-y-2 font-medium">

          <ItemListComponent link="/home" title="Home" />
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