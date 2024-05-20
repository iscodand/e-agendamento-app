<script lang="ts" setup>
import { defineProps, defineEmits, ref } from 'vue';
import { useItemStore } from '@/stores/items'
import type { InputCreateItem } from '@/services/items/types';
import TextInputComponent from "@/components/ui/input/TextInputComponent.vue";
import NumberInputComponent from "@/components/ui/input/NumberInputComponent.vue";
import ActionButton from '@/components/ui/buttons/ActionButton.vue';

defineProps<{ show: boolean }>();

const itemStore = useItemStore();

const emit = defineEmits(['close', 'submit']);

let request = ref({
    name: '',
    description: '',
    categoryId: '0bb34802-422b-4b93-b783-b2ecfa98da00',
    totalQuantity: 0,
    quantityAvailable: 0,
    companyId: 'd62e9c28-dcb7-4633-adc3-0bd82678a363'
})

function hideModalHandler() {
    request = ref({
        name: '',
        description: '',
        categoryId: '0bb34802-422b-4b93-b783-b2ecfa98da00',
        totalQuantity: 0,
        quantityAvailable: 0,
        companyId: 'd62e9c28-dcb7-4633-adc3-0bd82678a363'
    })
    emit('close');
}

async function handleSubmit() {
    const input: InputCreateItem = {
        name: request.value.name,
        description: request.value.description,
        categoryId: request.value.categoryId,
        totalQuantity: request.value.totalQuantity,
        quantityAvailable: request.value.quantityAvailable,
        companyId: request.value.companyId
    };

    const response = await itemStore.dispatchCreateItem(input);

    if (response.succeeded) {
        emit('submit', { name: request.value.name });
        hideModalHandler();
    } else {
        console.error('Failed to create item', response.status);
    }
}
</script>

<template>
    <Teleport to="#modals">
        <div v-if="show" class="">
            <div class="fixed inset-0 bg-gray-900 opacity-40"></div>
            <div class="fixed inset-0 flex items-center justify-center">
                <div class="bg-white text-black p-4 rounded shadow-lg max-w-md w-full">
                    <h2 class="text-xl mb-4">Criar Novo Item</h2>
                    <form @submit.prevent="handleSubmit">
                        <div class="mb-4">
                            <label for="itemName" class="block text-sm font-medium text-gray-700">Nome do Item</label>
                            <TextInputComponent v-model="request.name" placeholder="Nome do Item" />
                        </div>

                        <div class="mb-4">
                            <label for="itemDescription" class="block text-sm font-medium text-gray-700">Descrição do
                                Item</label>
                            <TextInputComponent v-model="request.description" placeholder="Descrição do Item" />
                        </div>

                        <div class="mb-4">
                            <label for="quantityAvailable" class="block text-sm font-medium text-gray-700">Quantidade
                                Disponível</label>
                            <NumberInputComponent v-model="request.totalQuantity" />
                        </div>

                        <div class="mb-4">
                            <label for="totalQuantity" class="block text-sm font-medium text-gray-700">Quantidade
                                Total</label>
                            <NumberInputComponent v-model="request.quantityAvailable" />
                        </div>

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