<script setup lang="ts">
import { type Category, type InputUpdateCategory } from '@/services/categories/types';
import { useCategoryStore } from '@/stores/categories';
import { ref, watch } from 'vue';
import TextInputComponent from '@/components/ui/input/TextInputComponent.vue';
import ErrorMessageComponent from '@/components/ui/error/ErrorMessageComponent.vue';
import ActionButton from '@/components/ui/buttons/ActionButton.vue';

const props = defineProps<{ category: Category, show: boolean }>()
const emit = defineEmits(['close', 'submit'])

const localCategory = ref<Category>({ ...props.category })
const errorMessages = ref<string[]>([]);

const categoryStore = useCategoryStore();

watch(() => props.category, (newCategory) => {
    if (newCategory) {
        localCategory.value = { ...newCategory };
    }
});

function hideModalHandler() {
    emit('close');
}

async function handleSubmit() {
    const input: InputUpdateCategory = {
        id: localCategory.value.id,
        description: localCategory.value.description
    };

    console.log(input.id)

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
    <div>
        <Teleport to="#modals">
            <div v-if="show" class="">
                <div class="fixed inset-0 bg-gray-900 opacity-40"></div>
                <div class="fixed inset-0 flex items-center justify-center">
                    <div class="bg-white text-black p-4 rounded shadow-lg max-w-md w-full">
                        <h2 class="text-xl mb-4">Atualizar Categoria</h2>
                        <form>
                            <div class="mb-4">
                                <label for="categoryDescription"
                                    class="block text-sm font-medium text-gray-700">Descrição</label>
                                <TextInputComponent v-model="localCategory.description"
                                    placeholder="Descrição da Categoria" />
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