<script lang="ts" setup>
import { defineEmits, ref, onMounted, watch } from 'vue';
import { useItemStore } from '@/stores/items'
import type { InputCreateItem } from '@/services/items/types';
import TextInputComponent from "@/components/ui/input/TextInputComponent.vue";
import NumberInputComponent from "@/components/ui/input/NumberInputComponent.vue";
import ActionButton from '@/components/ui/buttons/ActionButton.vue';
import ErrorMessageComponent from '@/components/ui/error/ErrorMessageComponent.vue';

defineProps<{ show: boolean, categories: any }>();

const itemStore = useItemStore();
const emit = defineEmits(['close', 'submit']);

const errorMessages = ref<string[]>([]);

let request = ref({
    name: '',
    description: '',
    categoryId: '',
    totalQuantity: 0,
    quantityAvailable: 0,
})

function hideModalHandler() {
    request = ref({
        name: '',
        description: '',
        categoryId: '',
        totalQuantity: 0,
        quantityAvailable: 0,
    })
    emit('close');
}

watch(
    () => request.value,
    (newVal) => {
        errorMessages.value = [];
        if (newVal.quantityAvailable > newVal.totalQuantity) {
            errorMessages.value.push("A Quantidade total precisa ser maior que a quantidade disponível de itens.");
        }
    },
    { deep: true }
);


async function handleSubmit() {
    errorMessages.value = [];

    if (request.value.quantityAvailable > request.value.totalQuantity) {
        errorMessages.value.push("A Quantidade total precisa ser maior que a quantidade disponível de itens.");
        return;
    }

    const input: InputCreateItem = {
        name: request.value.name,
        description: request.value.description,
        categoryId: request.value.categoryId,
        totalQuantity: request.value.totalQuantity,
        quantityAvailable: request.value.quantityAvailable,
    };

    const response = await itemStore.dispatchCreateItem(input);

    if (response.succeeded) {
        emit('submit', { name: request.value.name });
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
                    <h2 class="text-xl mb-4">Criar Novo Item</h2>
                    <form @submit.prevent="handleSubmit">
                        <div class="mb-4">
                            <label for="itemName" class="block text-sm font-medium text-gray-700">Nome do Item</label>
                            <TextInputComponent required v-model="request.name" placeholder="Nome do Item" />
                        </div>

                        <div class="mb-4">
                            <label for="itemDescription" class="block text-sm font-medium text-gray-700">Descrição do
                                Item</label>
                            <TextInputComponent v-model="request.description" placeholder="Descrição do Item" />
                        </div>

                        <div class="mb-4">
                            <label for="categories"
                                class="block text-sm font-medium text-gray-700 mb-1">Categoria</label>
                            <select for=" categories"
                                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 "
                                v-model="request.categoryId">
                                <option v-for="category in categories" :value="category.id">
                                    {{ category.description }}
                                </option>
                            </select>
                        </div>

                        <div class="mb-4">
                            <label for="quantityAvailable" class="block text-sm font-medium text-gray-700">Quantidade
                                Disponível</label>
                            <NumberInputComponent required v-model="request.quantityAvailable" />
                        </div>

                        <div class="mb-4">
                            <label for="totalQuantity" class="block text-sm font-medium text-gray-700">Quantidade
                                Total</label>
                            <NumberInputComponent required v-model="request.totalQuantity" />
                        </div>

                        <ErrorMessageComponent :messages="errorMessages" />

                        <div class="flex gap-52">
                            <ActionButton color="red" @click="hideModalHandler">Cancelar
                            </ActionButton>
                            <ActionButton class="justify-end" type="submit" color="green">Cadastrar</ActionButton>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </Teleport>
</template>