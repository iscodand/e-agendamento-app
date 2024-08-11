<script lang="ts" setup>
import { defineEmits, ref, onMounted, watch, computed } from 'vue';
import { useItemStore } from '@/stores/items'
import type { InputCreateItem } from '@/services/items/types';
import ErrorMessageComponent from '@/components/ui/error/ErrorMessageComponent.vue';

//
import AutoComplete from 'primevue/autocomplete';
import type { Category } from '@/services/categories/types';
import InputText from 'primevue/inputtext';
import InputNumber from 'primevue/inputnumber';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';

const props = defineProps<{ show: boolean, categories: Category[] }>();

const itemStore = useItemStore();

const emit = defineEmits(['close', 'submit', 'create:show']);

const showDialog = computed({
    get: () => props.show,
    set: (value) => emit('create:show', value),
});

const errorMessages = ref<string[]>([]);

let request = ref<InputCreateItem>({
    name: '',
    description: '',
    categoryId: '',
    category: undefined,
    totalQuantity: 0,
    quantityAvailable: 0,
})

function hideModalHandler() {
    request = ref({
        name: '',
        description: '',
        categoryId: '',
        category: undefined,
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
        categoryId: request.value.category!.id,
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

const filteredCategories = ref<Category[]>([]);

const searchCategory = (event: { query: string }) => {
    const query = event.query.toLowerCase();

    filteredCategories.value = props.categories.filter((category: Category) =>
        category.description.toLowerCase().includes(query)
    );
};
</script>

<template>
    <Dialog v-model:visible="showDialog" modal header="Criar novo item" :style="{ width: '30rem' }">
        <form @submit.prevent="handleSubmit">
            <div class="flex flex-col gap-2 mb-4">
                <label for="itemName">
                    Nome do Item
                </label>
                <InputText id="itemName" v-model="request.name" :invalid="request.name.length < 5" />
            </div>

            <div class="flex flex-col gap-2 mb-4">
                <label for="itemDescription">
                    Descrição do Item
                </label>
                <InputText id="itemDescription" v-model="request.description" />
            </div>

            <div class="flex flex-col gap-2 mb-4">
                <label for="category" class="block text-sm font-medium text-gray-700">
                    Categoria
                </label>
                <AutoComplete v-model="request.category" dropdown :suggestions="filteredCategories"
                    @complete="searchCategory" optionLabel="description" :invalid="request.category === null" />
            </div>

            <div class="flex flex-col gap-2 mb-4">
                <label for="quantityAvailable" class="block text-sm font-medium text-gray-700">
                    Quantidade Disponível
                </label>
                <InputNumber v-model="request.quantityAvailable" :invalid="request.quantityAvailable === 0"
                    inputId="withoutgrouping" :useGrouping="false" fluid />
            </div>

            <div class="flex flex-col gap-2 mb-4">
                <label for="totalQuantity" class="block text-sm font-medium text-gray-700">
                    Quantidade Total
                </label>
                <InputNumber v-model="request.totalQuantity" :invalid="request.totalQuantity === 0"
                    inputId="withoutgrouping" :useGrouping="false" fluid />
            </div>

            <ErrorMessageComponent :messages="errorMessages" />

            <div class="flex justify-end gap-3">
                <Button size="small" @click="hideModalHandler" label="Cancelar" severity="danger" icon="pi pi-times" />
                <Button size="small" label="Salvar" type="submit" severity="success" icon="pi pi-check" />
            </div>
        </form>
    </Dialog>
</template>