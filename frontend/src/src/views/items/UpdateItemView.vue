<script lang="ts" setup>
import type { Item, InputUpdateItem } from '@/services/items/types';
import TextInputComponent from '@/components/ui/input/TextInputComponent.vue';
import NumberInputComponent from '@/components/ui/input/NumberInputComponent.vue';
import ActionButton from '@/components/ui/buttons/ActionButton.vue';
import ErrorMessageComponent from '@/components/ui/error/ErrorMessageComponent.vue';
import { useItemStore } from '@/stores/items';
import { ref, watch, defineProps, defineEmits } from 'vue';
import type { Category } from '@/services/categories/types';

const props = defineProps<{ item: Item, show: boolean, categories: Category[] }>();
const emit = defineEmits(['close', 'submit']);

const localItem = ref<Item>({ ...props.item });
const errorMessages = ref<string[]>([]);

const itemStore = useItemStore();

watch(() => props.item, (newItem) => {
    if (newItem) {
        localItem.value = { ...newItem };
    }
});

function hideModalHandler() {
    emit('close');
}

watch(
    () => localItem.value,
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

    if (localItem.value.quantityAvailable > localItem.value.totalQuantity) {
        errorMessages.value.push("A Quantidade total precisa ser maior que a quantidade disponível de itens.");
        return;
    }

    const input: InputUpdateItem = {
        id: localItem.value.id,
        name: localItem.value.name,
        description: localItem.value.description,
        categoryId: localItem.value.categoryId,
        totalQuantity: localItem.value.totalQuantity,
        quantityAvailable: localItem.value.quantityAvailable,
        isAvailable: localItem.value.isAvailable
    }

    const { succeeded, errors } = await itemStore.dispatchUpdateItem(input);

    if (succeeded) {
        emit('submit', { name: localItem.value.name });
        hideModalHandler();
    } else {
        errorMessages.value.push(errors![0]);
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
                        <h2 class="text-xl mb-4">Atualizar Categoria</h2>
                        <form>
                            <div class="mb-4">
                                <label for="itemName" class="block text-sm font-medium text-gray-700">Nome</label>
                                <TextInputComponent v-model="localItem.name" placeholder="Nome do Item" />
                            </div>

                            <div class="mb-4">
                                <label for="itemDescription"
                                    class="block text-sm font-medium text-gray-700">Descrição</label>
                                <TextInputComponent v-model="localItem.description" placeholder="Descrição do Item" />
                            </div>

                            <div class="mb-4">
                                <label for="categories"
                                    class="block text-sm font-medium text-gray-700 mb-1">Categoria</label>
                                <select for=" categories"
                                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 "
                                    v-model="localItem.categoryId">
                                    <option v-for="category in categories" :value="category.id">
                                        {{ category.description }}
                                    </option>
                                </select>
                            </div>

                            <div class="mb-4">
                                <label class="block text-sm font-medium text-gray-700"
                                    for="isAvailable">Disponível</label>
                                <input type="checkbox" v-model="localItem.isAvailable">
                            </div>

                            <div class="mb-4">
                                <label for="quantityAvailable"
                                    class="block text-sm font-medium text-gray-700">Quantidade
                                    Disponível</label>
                                <NumberInputComponent v-model="localItem.quantityAvailable" />
                            </div>

                            <div class="mb-4">
                                <label for="totalQuantity" class="block text-sm font-medium text-gray-700">Quantidade
                                    Total</label>
                                <NumberInputComponent v-model="localItem.totalQuantity" />
                            </div>

                            <ErrorMessageComponent :messages="errorMessages" />

                            <div class="flex justify-end gap-3">
                                <ActionButton color="red" @click="hideModalHandler">Cancelar</ActionButton>
                                <ActionButton color="green" @click="handleSubmit">Salvar</ActionButton>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </Teleport>
    </div>
</template>