<script lang="ts" setup>
import { computed, defineEmits, ref } from 'vue';
import { useCategoryStore } from '@/stores/categories'
import TextInputComponent from "@/components/ui/input/TextInputComponent.vue";
import ActionButton from '@/components/ui/buttons/ActionButton.vue';
import type { InputCreateCategory } from '@/services/categories/types';
import ErrorMessageComponent from '@/components/ui/error/ErrorMessageComponent.vue';

//
import Dialog from 'primevue/dialog';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Toast from 'primevue/toast';
import { useToast } from 'primevue/usetoast';

const toast = useToast();

const props = defineProps<{ show: boolean }>();

const categoryStore = useCategoryStore();

const emit = defineEmits(['close', 'submit', 'create:show']);

const errorMessages = ref<string[]>([]);

let request = ref({
    description: '',
});

const showDialog = computed({
    get: () => props.show,
    set: (value) => emit('create:show', value),
});

function hideModalHandler() {
    request = ref({
        description: '',
    })
    emit('close');
}

async function handleSubmit() {
    errorMessages.value = [];

    const input: InputCreateCategory = {
        description: request.value.description,
        companyId: ''
    };

    const response = await categoryStore.dispatchCreateCategory(input);

    if (response.succeeded) {
        emit('submit', { name: request.value.description });
        hideModalHandler();
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Categoria cadastrada com sucesso.', life: 3000 });
    } else {
        errorMessages.value.push(response.errors![0]);
    }
}
</script>

<template>
    <Dialog v-model:visible="showDialog" modal header="Criar Nova Categoria" :style="{ width: '30rem' }">
        <form @submit.prevent="handleSubmit">
            <div class="flex flex-col gap-2 mb-4">
                <label for="categoryDescription">
                    Descrição
                </label>
                <InputText id="categoryDescription" v-model="request.description"
                    :invalid="request.description.length < 5" />
            </div>

            <ErrorMessageComponent :messages="errorMessages" />

            <div class="flex justify-end gap-3">
                <div class="flex justify-end gap-3">
                    <Button size="small" @click="hideModalHandler" label="Cancelar" severity="danger"
                        icon="pi pi-times" />

                    <Toast />
                    <Button size="small" label="Salvar" type="submit" severity="success" icon="pi pi-check" />
                </div>
            </div>
        </form>
    </Dialog>
</template>