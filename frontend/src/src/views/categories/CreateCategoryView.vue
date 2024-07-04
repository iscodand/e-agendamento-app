<script lang="ts" setup>
import { defineEmits, ref } from 'vue';
import { useCategoryStore } from '@/stores/categories'
import TextInputComponent from "@/components/ui/input/TextInputComponent.vue";
import ActionButton from '@/components/ui/buttons/ActionButton.vue';
import type { InputCreateCategory } from '@/services/categories/types';
import ErrorMessageComponent from '@/components/ui/error/ErrorMessageComponent.vue';

defineProps<{ show: boolean }>();

const categoryStore = useCategoryStore();

const emit = defineEmits(['close', 'submit']);

const errorMessages = ref<string[]>([]);

let request = ref({
    description: '',
})

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
    } else {
        errorMessages.value.push(response.errors![0]);
    }
}
</script>

<template>
    <Teleport to="#modals">
        <div v-if="show" class="">
            <div class="fixed inset-0 bg-gray-900 opacity-40"></div>
            <div class="fixed inset-0 flex items-center justify-center">
                <div class="bg-white text-black p-4 rounded shadow-lg max-w-md w-full">
                    <h2 class="text-xl mb-4">Criar Nova Categoria</h2>
                    <form @submit.prevent="handleSubmit">
                        <div class="mb-4">
                            <label for="itemName" class="block text-sm font-medium text-gray-700">Descrição</label>
                            <TextInputComponent v-model="request.description" placeholder="Descrição da Categoria" />
                        </div>

                        <ErrorMessageComponent :messages="errorMessages" />

                        <div class="flex justify-end gap-3">
                            <ActionButton color="red" @click="hideModalHandler">Cancelar</ActionButton>
                            <ActionButton @click="handleSubmit" color="green">Cadastrar</ActionButton>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </Teleport>
</template>