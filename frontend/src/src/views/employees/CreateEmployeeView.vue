<script setup lang="ts">

import type { InputCreateUser } from '@/services/user/types';
import { useUserStore } from '@/stores/user';
import { computed, onMounted, ref } from 'vue';
import ErrorMessageComponent from '@/components/ui/error/ErrorMessageComponent.vue';

import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import Password from 'primevue/password';
import InputMask from 'primevue/inputmask';
import Toast from 'primevue/toast';
import { useToast } from 'primevue/usetoast';
import AutoComplete from 'primevue/autocomplete'
import type { Role } from '@/stores/user/types';

const userStore = useUserStore();
const props = defineProps<{ show: boolean, companyName: string, companyId: string }>();
const errorMessages = ref<string[]>([]);

const toast = useToast();

const roles = ref<Role[]>([
    {
        "id": "Admin",
        "name": "Admin",
        "description": "Administrador",
    },
    {
        "id": "Basic",
        "name": "Basic",
        "description": "Comum"
    }
])

const emit = defineEmits(['close', 'submit', 'createUser:show']);

const showDialog = computed({
    get: () => props.show,
    set: (value) => emit('createUser:show', value),
});

const request = ref<InputCreateUser>({
    userName: "",
    fullName: "",
    phoneNumber: "",
    email: "",
    roles: [],
    companyId: props.companyId,
    password: "",
    confirmPassword: ""
});

function hideModalHandler() {
    errorMessages.value = [];
    emit('close')
}

async function handleSubmit() {
    errorMessages.value = [];

    const input = ref<InputCreateUser>({
        userName: request.value.userName,
        fullName: request.value.fullName,
        phoneNumber: request.value.phoneNumber,
        email: request.value.email,
        roles: request.value.roles,
        companyId: props.companyId,
        password: request.value.password,
        confirmPassword: request.value.confirmPassword
    });

    const { succeeded, errors } = await userStore.dispatchCreateUser(input.value);

    console.log(succeeded)

    if (succeeded) {
        hideModalHandler();
        emit('submit')
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Usuário cadastrado com sucesso.', life: 3000 });
    } else {
        errorMessages.value.push(`Erro ao cadastrar usuário: ${errors![0]}`);
    }
}

const filteredRoles = ref<string[]>([]);
const searchRole = (event: { query: string }) => {
    const query = event.query.toLowerCase();

    filteredRoles.value = roles.value.filter((role: Role) =>
        role.description.includes(query)
    ).map(x => x.id);
};
</script>

<template>
    <div>
        <Dialog v-model:visible="showDialog" modal header="Cadastrar novo Usuário" :style="{ width: '40rem' }">
            <form @submit.prevent="handleSubmit">

                <div class="flex gap-5">
                    <div class="mb-4">
                        <label for="userFullName" class="block text-sm font-medium text-gray-700 mb-2">
                            Nome Completo
                        </label>
                        <InputText v-model="request.fullName" class="w-72" required="true" />
                    </div>

                    <div class="mb-4">
                        <label for="userUserName" class="block text-sm font-medium text-gray-700 mb-2">
                            Username
                        </label>
                        <InputText v-model="request.userName" class="w-72" required="true" />
                    </div>
                </div>

                <div class="flex gap-5">
                    <div class="mb-4">
                        <label for="userEmail" class="block text-sm font-medium text-gray-700 mb-2">E-mail</label>
                        <InputText type="email" v-model="request.email" class="w-72" required="true" />
                    </div>

                    <div class="mb-4">
                        <label for="itemName" class="block text-sm font-medium text-gray-700 mb-2">
                            Número de Telefone
                        </label>
                        <InputMask v-model="request.phoneNumber" mask="(99) 9 9999-9999" class="w-72" required="true" />
                    </div>
                </div>

                <div class="flex gap-5">
                    <div class="mb-4">
                        <label for="userPassword" class="block text-sm font-medium text-gray-700 mb-2">
                            Senha
                        </label>
                        <Password v-model="request.password" weakLabel="Fraca" mediumLabel="Aceitável"
                            strongLabel="Forte" promptLabel="Nível de Segurança" toggleMask />
                    </div>

                    <div class="mb-4">
                        <label for="itemName" class="block text-sm font-medium text-gray-700 mb-2">
                            Confirme sua senha
                        </label>
                        <Password v-model="request.confirmPassword" :feedback="false" toggleMask />
                    </div>
                </div>

                <div class="flex gap-5">
                    <div class="mb-4">
                        <label for="userPassword" class="block text-sm font-medium text-gray-700 mb-2">
                            Cargo
                        </label>
                        <AutoComplete v-model="request.roles" inputId="multiple-ac-1" multiple fluid
                            :suggestions="filteredRoles" @complete="searchRole" dropdown />
                    </div>

                    <div class="mb-4">
                        <label for="itemName" class="block text-sm font-medium text-gray-700 mb-2">
                            Empresa
                        </label>
                        <InputText :placeholder="companyName" disabled="true" class="w-72" required="true" />
                    </div>
                </div>

                <ErrorMessageComponent :messages="errorMessages" />

                <div class="flex justify-end gap-3 mt-6">
                    <Button size="small" @click="hideModalHandler" label="Cancelar" severity="danger"
                        icon="pi pi-times" />

                    <Toast />
                    <Button size="small" label="Salvar" type="submit" severity="success" icon="pi pi-check" />
                </div>
            </form>
        </Dialog>
    </div>
</template>