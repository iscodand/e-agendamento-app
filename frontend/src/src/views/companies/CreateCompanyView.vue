<script setup lang="ts">
import TextInputComponent from "@/components/ui/input/TextInputComponent.vue";
import ErrorMessageComponent from "@/components/ui/error/ErrorMessageComponent.vue";
import ActionButton from "@/components/ui/buttons/ActionButton.vue";
import { ref } from "vue";
import type { InputCreateCompany } from "@/services/companies/types";
import { useCompanyStore } from "@/stores/companies";

//
import Button from "primevue/button";
import ButtonGroup from "primevue/buttongroup";

defineProps<{ show: boolean }>();

const companyStore = useCompanyStore();
const emit = defineEmits(['close', 'submit']);
const errorMessages = ref<String[]>([]);

let request = ref({
    name: '',
    description: '',
    cnpj: ''
});

function hideModalHandler() {
    request = ref({
        name: '',
        description: '',
        cnpj: ''
    })
    errorMessages.value = [];
    emit('close');
}

async function handleSubmit() {
    const input: InputCreateCompany = {
        name: request.value.name,
        cnpj: request.value.cnpj,
        description: request.value.description
    }

    const response = await companyStore.dispatchCreateCompany(input);

    if (response.succeeded) {
        emit('submit', { name: request.value.name });
        hideModalHandler();
    } else {
        errorMessages.value.push(response.errors![0]);
    }
}

</script>

<template>
    <div>
        <Teleport to="#modals">
            <div v-if="show" class="">
                <div class="fixed inset-0 bg-gray-900 opacity-40"></div>
                <div class="fixed inset-0 flex items-center justify-center">
                    <div class="bg-white text-black p-4 rounded shadow-lg max-w-md w-full">
                        <h2 class="text-xl mb-4">Cadastrar Empresa</h2>
                        <form @submit.prevent="handleSubmit">
                            <div class="mb-4">
                                <label for="itemName" class="block text-sm font-medium text-gray-700">Nome da
                                    Empresa</label>
                                <TextInputComponent required v-model="request.name" placeholder="Nome da Empresa" />
                            </div>

                            <div class="mb-4">
                                <label for="itemName" class="block text-sm font-medium text-gray-700">Descrição</label>
                                <TextInputComponent required v-model="request.description"
                                    placeholder="Descrição da Empresa" />
                            </div>

                            <div class="mb-4">
                                <label for="itemName" class="block text-sm font-medium text-gray-700">CNPJ</label>
                                <TextInputComponent v-mask="'##.###.###/####-##'" required v-model="request.cnpj"
                                    placeholder="CNPJ" />
                            </div>

                            <ErrorMessageComponent :messages="errorMessages" />

                            <div class="flex gap-52 mt-4">
                                <Button size="small" @click="hideModalHandler" label="Cancelar" severity="danger"
                                    icon="pi pi-times" />
                                <Button size="small" label="Cadastrar" type="submit" severity="success"
                                    icon="pi pi-check" />
                                <!-- <ActionButton @click="hideModalHandler" color="red">Cancelar</ActionButton>
                                <ActionButton class="justify-end" type="submit" color="green">Cadastrar</ActionButton> -->
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </Teleport>
    </div>
</template>