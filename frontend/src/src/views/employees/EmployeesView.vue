<script setup lang="ts">
import type { User } from '@/services/user/types';
import { ref, type Ref } from 'vue';
import type { Company } from '@/services/companies/types';
import CreateEmployeeView from './CreateEmployeeView.vue';
import AddExistentUserView from '../companies/AddExistentUserView.vue';

// Animations
import NotFoundAnimation from '@/assets/animations/not-found/NotFoundAnimation.vue'
import DetailEmployeeView from './DetailEmployeeView.vue';
import type { Employee } from '@/services/employees/types';

const props = defineProps<{ company: Company, employees: User[] }>()

let employee: Ref<Employee | undefined> = ref<Employee>(
    {
        id: "",
        email: "",
        fullName: "",
        userName: "",
        roles: [],
        companies: []
    }
);

// show create user modal
const showCreateNewUser = ref(false);
function showCreateNewUserHandler() {
    showCreateNewUser.value = true;
}
function hideCreateItemModalHandler() {
    showCreateNewUser.value = false;
}

// show add existent user modal
const showAddExistentUser = ref(false);
function showAddExistentUserModalHandler() {
    showAddExistentUser.value = true;
}
function hideAddExistentUserModalHandler() {
    showAddExistentUser.value = false;
}

// show detail user modal
const showDetailEmployee: Ref<boolean> = ref(false);
function showDetailEmployeeModalHandler(selectedEmployee: Employee) {
    showDetailEmployee.value = true;
    employee.value = selectedEmployee
}
function hideDetailEmployeeModalHandler() {
    showDetailEmployee.value = false;
}

function formatRoles(roles: string[]) {
    if (roles)
        return roles.join(", ");
}

</script>

<template>
    <div>
        <div class="flex col-span-2 m-4 items-center justify-between">
            <div class="flex justify-start">
                <IconField>
                    <InputIcon class="pi pi-search" />
                    <InputText placeholder="Buscar Funcionário" />
                </IconField>
            </div>
            <div class="flex justify-end gap-4">
                <Button label="Adicionar usuário existente" size="small" severity="info"
                    @click="showAddExistentUserModalHandler" />
                <Button label="Registrar novo usuário" size="small" @click="showCreateNewUserHandler" />
            </div>
        </div>

        <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
            <DataTable :value="employees" paginator :rows="5" :rowsPerPageOptions="[5, 10, 20, 50]"
                tableStyle="min-width: 50rem">

                <template #empty>
                    <NotFoundAnimation text="Essa empresa não possui funcionários cadastrados."
                        buttonLabel="Clique aqui para adicionar" :action="showCreateNewUserHandler" />
                </template>

                <Column field="fullName" header="Nome" style="width: 20%"></Column>
                <Column field="email" header="E-mail" style="width: 20%" class="truncate">
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
                <Column field="roles" header="Cargos" style="width: 20%">
                    <template #body="slotProps">
                        <Tag class="py-0 pl-0 pr-4" :value="formatRoles(slotProps.data.roles)" severity="info" />
                    </template>
                </Column>
                <Column field="actions" header="" style="width: 10%">
                    <template #body="{ data }">
                        <div class="flex gap-4">
                            <Button @click="showDetailEmployeeModalHandler(data)" size="small" label="Ver perfil"
                                severity="info" icon="pi pi-info-circle" />
                        </div>
                    </template>
                </Column>
            </DataTable>
        </div>
    </div>

    <CreateEmployeeView :show="showCreateNewUser" :company-name="company.name" :company-id="company.id"
        @close="hideCreateItemModalHandler" />

    <AddExistentUserView :show="showAddExistentUser" @close="hideAddExistentUserModalHandler" :company-id="company.id"
        :external-exployees="employees!" />

    <DetailEmployeeView :show="showDetailEmployee" @close="hideDetailEmployeeModalHandler" :employee="employee!" />

</template>