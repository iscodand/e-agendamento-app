<script setup lang="ts">
import { type Category, type InputUpdateCategory } from '@/services/categories/types';
import { useCategoryStore } from '@/stores/categories';
import { computed, ref, watch } from 'vue';
import ErrorMessageComponent from '@/components/ui/error/ErrorMessageComponent.vue';

//
import Dialog from 'primevue/dialog';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';

const props = defineProps<{ category: Category, show: boolean }>()
const emit = defineEmits(['close', 'submit', 'update:show']);

const localCategory = ref<Category>({ ...props.category })
const errorMessages = ref<string[]>([]);

const categoryStore = useCategoryStore();

watch(() => props.category, (newCategory) => {
    if (newCategory) {
        localCategory.value = { ...newCategory };
    }
});

const showDialog = computed({
    get: () => props.show,
    set: (value) => emit('update:show', value),
});

function hideModalHandler() {
    emit('close');
}

async function handleSubmit() {
    const input: InputUpdateCategory = {
        id: localCategory.value.id,
        description: localCategory.value.description
    };

    const { succeeded, errors } = await categoryStore.dispatchUpdateCategory(input);

    if (succeeded) {
        emit('submit', { name: localCategory.value.description });
        hideModalHandler();
    } else {
        errorMessages.value.push(errors![0]);
    }
}

</script>

<template>
    <Dialog v-model:visible="showDialog" modal header="Atualizar Categoria" :style="{ width: '30rem' }">
        <form @submit.prevent="handleSubmit">
            <div class="flex flex-col gap-2 mb-4">
                <label for="categoryDescription">
                    Descrição
                </label>
                <InputText id="categoryDescription" v-model="localCategory.description"
                    :invalid="localCategory.description.length < 5" />
            </div>

            <ErrorMessageComponent :messages="errorMessages" />

            <div class="flex justify-end gap-3">
                <div class="flex justify-end gap-3">
                    <Button size="small" @click="hideModalHandler" label="Cancelar" severity="danger"
                        icon="pi pi-times" />
                    <Button size="small" label="Salvar" type="submit" severity="success" icon="pi pi-check" />
                </div>
            </div>
        </form>
    </Dialog>
</template>