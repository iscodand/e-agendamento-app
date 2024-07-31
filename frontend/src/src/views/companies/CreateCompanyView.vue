<script setup lang="ts">
import TextInputComponent from "@/components/ui/input/TextInputComponent.vue";
import ErrorMessageComponent from "@/components/ui/error/ErrorMessageComponent.vue";
import { computed, ref } from "vue";
import type { InputCreateCompany } from "@/services/companies/types";
import { useCompanyStore } from "@/stores/companies";

//
import Button from "primevue/button";
import Dialog from "primevue/dialog";

const props = defineProps<{ show: boolean }>();
const errorMessages = ref<string[]>([]);

const companyStore = useCompanyStore();
const emit = defineEmits(['close', 'submit', 'create:show']);

const showDialog = computed({
    get: () => props.show,
    set: (value) => emit('create:show', value),
});

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
    <Dialog v-model:visible="showDialog" modal header="Criar nova Empresa" :style="{ width: '30rem' }">
        <form @submit.prevent="handleSubmit">
            <div class="mb-4">
                <label for="itemName" class="block text-sm font-medium text-gray-700">Nome da
                    Empresa</label>
                <TextInputComponent required v-model="request.name" placeholder="Nome da Empresa" />
            </div>

            <div class="mb-4">
                <label for="itemName" class="block text-sm font-medium text-gray-700">Descrição</label>
                <TextInputComponent required v-model="request.description" placeholder="Descrição da Empresa" />
            </div>

            <div class="mb-4">
                <label for="itemName" class="block text-sm font-medium text-gray-700">CNPJ</label>
                <TextInputComponent v-mask="'##.###.###/####-##'" required v-model="request.cnpj" placeholder="CNPJ" />
            </div>

            <ErrorMessageComponent :messages="errorMessages" />

            <div class="flex justify-end gap-3">
                <Button size="small" @click="hideModalHandler" label="Cancelar" severity="danger" icon="pi pi-times" />
                <Button size="small" label="Salvar" type="submit" severity="success" icon="pi pi-check" />
            </div>
        </form>
    </Dialog>
</template>