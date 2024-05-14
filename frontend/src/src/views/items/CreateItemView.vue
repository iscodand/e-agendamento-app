<script setup lang="ts">
import { defineProps, defineEmits, ref } from 'vue';

defineProps<{ show: boolean }>();

const emit = defineEmits(['close', 'submit']);

const itemName = ref('');

function hideModalHandler() {
    emit('close');
}

function handleSubmit() {
    emit('submit', { name: itemName.value });
    hideModalHandler();
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
                            <input type="text" id="itemName" v-model="itemName" class="mt-1 block w-full" required>
                        </div>
                        <div class="mb-4">
                            <label for="itemName" class="block text-sm font-medium text-gray-700">Descrição do
                                Item</label>
                            <input type="text" id="itemName" v-model="itemName" class="mt-1 block w-full" required>
                        </div>
                        <div class="mb-4">
                            <label for="itemName" class="block text-sm font-medium text-gray-700">Nome do Item</label>
                            <input type="text" id="itemName" v-model="itemName" class="mt-1 block w-full" required>
                        </div>
                        <div class="flex justify-end">
                            <button type="button" @click="hideModalHandler" class="mr-2">Cancelar</button>
                            <button type="submit" class="bg-blue-600 text-white px-4 py-2 rounded">Criar</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </Teleport>
</template>