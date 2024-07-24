<script setup lang="ts">
import type { User } from '@/services/user/types';
import { authStore } from '@/stores/auth';
import router from '@/router';
import type { MenuItem } from 'primevue/menuitem';
import Menubar from 'primevue/menubar';
import { PrimeIcons } from '@primevue/core/api'

const useAuth = authStore();

const props = defineProps<{ user: User }>();

async function handleLogout() {
    useAuth.clear();
    router.push('login/');
}

const items: MenuItem[] = [
    {
        label: `Olá, ${props.user.fullName.split(" ")[0]}!`,
        // icon: PrimeIcons.USER,
        items: [
            {
                label: "Meu Perfil",
                icon: PrimeIcons.USER
            },
            {
                label: "Configurações",
                icon: PrimeIcons.COG
            },
            {
                label: "Logout",
                icon: PrimeIcons.SIGN_OUT
            }
        ]
    }
];

</script>

<template>
    <Menubar class="h-20">
        <template #start>
            <a class="flex ms-2 md:me-24">
                <img src="https://flowbite.com/docs/images/logo.svg" class="h-8 me-3" alt="FlowBite Logo" />
                <span
                    class="self-center text-xl font-semibold sm:text-2xl whitespace-nowrap text-black">E-Agendamento</span>
            </a>
        </template>
        <template #end>
            <Menubar :model="items">
            </Menubar>
        </template>
    </Menubar>
</template>

<style scoped></style>