<script setup lang="ts">

import { computed, ref } from 'vue';

import Dialog from 'primevue/dialog';
import Button from 'primevue/button';
import Toast from 'primevue/toast';
import { useToast } from 'primevue/usetoast';
import IconField from 'primevue/iconfield';
import InputIcon from 'primevue/inputicon';
import InputText from 'primevue/inputtext';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import NotFoundAnimation from '@/assets/animations/not-found/NotFoundAnimation.vue';
import Tag from 'primevue/tag';

import { useCompanyStore } from '@/stores/companies';
import type { User } from '@/services/user/types';

import { debounce } from 'lodash'

const toast = useToast();
const companyStore = useCompanyStore();

const employees = ref<User[]>([]);

const props = defineProps<{ show: boolean, companyId: string, externalExployees: User[] }>()
const emit = defineEmits(['close', 'submit', 'create:show']);

const searchTerm = ref<string>("");

const errorMessages = ref<string[]>([])

function hideModalHandler() {
    errorMessages.value = [];
    emit('close')
}

async function searchByUser() {
    const { succeeded, data } = await companyStore.dispatchSearchByEmployeeOnCompany(props.companyId, searchTerm.value);

    if (succeeded) {
        employees.value = data!;
    } else {
        errorMessages.value = [];
    }
}

// Função debounce para limitar as chamadas de busca
const onInput = debounce(() => {
    searchByUser();
}, 500);

async function addUserToCompany(userId: string) {
    const { succeeded } = await companyStore.dispatchAddUserToCompany(props.companyId, userId)

    if (succeeded) {
        props.externalExployees.push(employees.value.find(x => x.id == userId)!);
        hideModalHandler();
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Usuário adicionado com sucesso.', life: 3000 });
    }
}

const showDialog = computed({
    get: () => props.show,
    set: (value) => emit('create:show', value),
});
</script>

<template>
    <div>
        <Dialog v-model:visible="showDialog" modal header="Adicionar usuário existente" :style="{ width: '70rem' }">
            <div class="flex col-span-2 m-4 items-center justify-center mb-10">
                <IconField>
                    <div class="flex gap-3">
                        <InputIcon class="pi pi-search" />
                        <InputText v-model="searchTerm" @input="onInput" placeholder="Buscar usuário" />
                        <Button size="small" label="Cancelar" @click="hideModalHandler" severity="danger" />
                    </div>
                </IconField>
            </div>

            <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
                <DataTable :value="employees" paginator :rows="5" :rowsPerPageOptions="[5, 10, 20, 50]"
                    tableStyle="min-width: 50rem">

                    <template #empty>
                        <NotFoundAnimation text="Funcionário não encontrado." />
                    </template>

                    <Column field="fullName" header="Nome" style="width: 30%"></Column>
                    <Column field="email" header="E-mail" style="width: 30%" class="truncate">
                    </Column>
                    <Column field="isActive" header="Ativo" style="width: 20%">
                        <template #body="{ data }">
                            <div v-if="data.isActive">
                                <Tag value="Ativo" severity="success" />
                            </div>
                            <div v-else>
                                <Tag value="Inativo" severity="danger" />
                            </div>
                        </template>
                    </Column>
                    <Column style="width: 20%">
                        <template #body="{ data }">
                            <div v-if="data.alreadyInCompany">
                                <Toast />
                                <span class="italic text-sm text-gray-600">Usuário já adicionado</span>
                            </div>
                            <div v-else>
                                <form @submit.prevent="addUserToCompany(data.id)">
                                    <Button type="submit" label="Adicionar" />
                                </form>
                            </div>
                        </template>
                    </Column>
                </DataTable>
            </div>
        </Dialog>
    </div>
</template>