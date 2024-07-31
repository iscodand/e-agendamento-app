<script setup lang="ts">
import type { User } from '@/services/user/types';
import { authStore } from '@/stores/auth';
import router from '@/router';
import type { MenuItem } from 'primevue/menuitem';
import Menubar from 'primevue/menubar';
import { PrimeIcons } from '@primevue/core/api'

import Toast from 'primevue/toast';
import { useToast } from 'primevue/usetoast';

const toast = useToast();

const useAuth = authStore();

const props = defineProps<{ user: User }>();

async function handleLogout() {
    useAuth.clear();
    router.push('login/');
    toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Logout realizado com sucesso.', life: 3000 });
}

const items: MenuItem[] = [
    {
        label: `Olá, ${props.user.fullName.split(" ")[0]}!`,
        icon: "@;",
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
                icon: PrimeIcons.SIGN_OUT,
                command: handleLogout
            }
        ]
    }
];

</script>

<template>
    <!-- style="position: fixed; z-index: 1000;" -->
    <Menubar class="fixed h-20 top-0 left-0 w-full z-50" style="position: fixed;">
        <template #start>
            <a class="flex ms-2 md:me-24">
                <img src="https://flowbite.com/docs/images/logo.svg" class="h-8 me-3" alt="FlowBite Logo" />
                <span
                    class="self-center text-xl font-semibold sm:text-2xl whitespace-nowrap text-black">E-Agendamento</span>
            </a>
        </template>
        <template #end>
            <Toast />
            <Menubar :model="items" class="teste">
            </Menubar>
        </template>
    </Menubar>
</template>

<style scoped></style>