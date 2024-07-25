<script lang="ts" setup>
import type { Item, InputUpdateItem } from '@/services/items/types';
import ActionButton from '@/components/ui/buttons/ActionButton.vue';
import ErrorMessageComponent from '@/components/ui/error/ErrorMessageComponent.vue';
import { useItemStore } from '@/stores/items';
import { ref, watch, defineProps, defineEmits, computed } from 'vue';
import type { Category } from '@/services/categories/types';

//
import ToggleButton from 'primevue/togglebutton';
import InputText from 'primevue/inputtext';
import InputNumber from 'primevue/inputnumber';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import Toast from 'primevue/toast';
import { useToast } from 'primevue/usetoast';

const toast = useToast();

const props = defineProps<{ item: Item, show: boolean, categories: Category[] }>();
const emit = defineEmits(['close', 'submit', 'update:show']);

// dialog
const showDialog = computed({
    get: () => props.show,
    set: (value) => emit('update:show', value),
});

function hideModalHandler() {
    emit('close');
}

const errorMessages = ref<string[]>([]);

const itemStore = useItemStore();
const localItem = ref<Item>({ ...props.item });
watch(() => props.item, (newItem) => {
    if (newItem) {
        localItem.value = { ...newItem };
    }
});

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
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Item atualizado com sucesso.', life: 3000 });
    } else {
        errorMessages.value.push(errors![0]);
    }
}
</script>

<template>
    <Dialog v-model:visible="showDialog" modal header="Atualizar item" :style="{ width: '30rem' }">
        <form @submit.prevent="handleSubmit">
            <div class="flex flex-col gap-2 mb-4">
                <label for="itemName">
                    Nome
                </label>
                <InputText id="itemName" v-model="localItem.name" />
            </div>

            <div class="flex flex-col gap-2 mb-4">
                <label for="itemDescription">
                    Descrição
                </label>
                <InputText id="itemDescription" v-model="localItem.description" />
            </div>

            <div class="flex flex-col gap-2 mb-4">
                <label for="categories" class="block text-sm font-medium text-gray-700 mb-1">Categoria</label>
                <!-- <select for=" categories"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 "
                    v-model="localItem.categoryId">
                    <option v-for="category in categories" :value="category.id">
                        {{ category.description }}
                    </option>
                </select> -->
            </div>

            <div class="flex flex-col gap-2 mb-4">
                <label class="block text-sm font-medium text-gray-700" for="isAvailable">
                    Disponibilidade
                </label>
                <ToggleButton v-model="localItem.isAvailable" onLabel="Disponível" offLabel="Indisponível"
                    onIcon="pi pi-check" offIcon="pi pi-times" :invalid="!localItem.isAvailable" class="w-full sm:w-40"
                    aria-label="Confirmation" />
            </div>

            <div class="flex flex-col gap-2 mb-4">
                <label for="quantityAvailable" class="block text-sm font-medium text-gray-700">
                    Quantidade Disponível
                </label>
                <InputNumber v-model="localItem.quantityAvailable" inputId="withoutgrouping" :useGrouping="false"
                    fluid />
            </div>

            <div class="flex flex-col gap-2 mb-4">
                <label for="totalQuantity" class="block text-sm font-medium text-gray-700">
                    Quantidade Total
                </label>
                <InputNumber v-model="localItem.totalQuantity" inputId="withoutgrouping" :useGrouping="false" fluid />
            </div>

            <ErrorMessageComponent :messages="errorMessages" />

            <div class="flex justify-end gap-3">
                <Button size="small" @click="hideModalHandler" label="Cancelar" severity="danger" icon="pi pi-times" />
                <Button size="small" label="Salvar" type="submit" severity="success" icon="pi pi-check" />
            </div>
        </form>
    </Dialog>
</template>