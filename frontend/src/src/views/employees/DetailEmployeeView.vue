<script setup lang="ts">
import type { Employee, UpdateEmployeeRequest } from '@/services/employees/types';
import { computed, ref, watch } from 'vue';

const props = defineProps<{ show: boolean, employee: Employee }>()

const emit = defineEmits(['close', 'submit', 'create:show']);

const showDialog = computed({
    get: () => props.show,
    set: (value) => emit('create:show', value),
});

function hideModalHandler() {
    emit('close');
}

const updateDisabled = ref<boolean>(true);

function enableUpdate() {
    updateDisabled.value = !updateDisabled.value;
}

const request = ref<UpdateEmployeeRequest>({
    id: '',
    userName: '',
    fullName: '',
    email: '',
    phoneNumber: ''
});

// Watch to update request when props.employee changes
watch(() => props.employee, (newEmployee) => {
    if (newEmployee) {
        request.value = {
            id: newEmployee.id,
            userName: newEmployee.userName,
            fullName: newEmployee.fullName,
            email: newEmployee.email,
            phoneNumber: newEmployee.phoneNumber
        };
    }
}, { immediate: true });

</script>

<template>
    <div>
        <Dialog v-model:visible="showDialog" modal :header="props.employee.fullName" :style="{ width: '55rem' }">
            <Tag value="Ativa" severity="success" />

            <section id="companyDetails">
                <div class="flex gap-10">
                    <div class="flex flex-col gap-2 mb-2 w-64">
                        <label for="fullName">
                            Nome Completo
                        </label>
                        <InputText id="fullName" v-model="request.fullName" :disabled="updateDisabled" />
                    </div>

                    <div class="flex flex-col gap-2 mb-4 w-64">
                        <label for="userName">
                            Nome de Usuário
                        </label>
                        <InputText id="userName" v-model="request.userName" :disabled="updateDisabled" />
                    </div>
                </div>

                <div class="flex gap-10">
                    <div class="flex flex-col gap-2 mb-4 w-64">
                        <label for="email">
                            E-mail
                        </label>
                        <InputText id="email" v-model="request.email" type="email" :disabled="updateDisabled" />
                    </div>

                    <div class="flex flex-col gap-2 mb-4 w-64">
                        <label for="phoneNumber">
                            Número de Telefone
                        </label>
                        <InputText id="phoneNumber" v-model="request.phoneNumber" :disabled="updateDisabled"
                            v-mask="'(##) # ####-####'" placeholder="(00) 0 0000-0000" />
                    </div>
                </div>

                <div class=" mt-2 mb-8">
                    <Button v-if="updateDisabled" label="Editar" icon="pi pi-pencil" @click="enableUpdate"
                        severity="info" />

                    <div class="flex gap-8">
                        <Button v-if="!updateDisabled" label="Cancelar" severity="danger" icon="pi pi-times"
                            @click="enableUpdate" />
                    </div>
                </div>
            </section>
            <Button size="small" @click="hideModalHandler" label="Cancelar" severity="danger" icon="pi pi-times" />
        </Dialog>
    </div>
</template>