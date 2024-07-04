<script setup lang="ts">
import MainComponent from '@/components/ui/layout/MainComponent.vue'
import { useCategoryStore } from '@/stores/categories';
import { onMounted, ref, type Ref } from 'vue';
import CreateCategoryView from './CreateCategoryView.vue';
import UpdateCategoryView from './UpdateCategoryView.vue';
import type { Category } from '@/services/categories/types';

const categoryStore = useCategoryStore()
const categories: any = ref([])

const selectedCategory = ref();

const showCreateCategoryModal = ref(false);
const showUpdateCategoryModal = ref(false);

function showCreateCategoryModalHandler() {
    showCreateCategoryModal.value = true;
}

function hideCreateCategoryModalHandler() {
    showCreateCategoryModal.value = false;
}

function showUpdateCategoryModalHandler(category: Category) {
    selectedCategory.value = category;
    showUpdateCategoryModal.value = true;
}

function hideUpdateCategoryModalHandler() {
    showUpdateCategoryModal.value = false;
}

async function handleDeleteCategory(categoryId: string): Promise<void> {
    await categoryStore.dispatchDeleteCategory(categoryId);
}

onMounted(async () => {
    const { succeeded, data, status } = await categoryStore.dispatchGetCategories();

    if (succeeded) {
        categories.value = data;
    } else {
        console.log('Failed to get categories from API. Status:', status)
    }
})

</script>

<template>
    <div>
        <MainComponent />
        <div class="p-4 sm:ml-64">
            <div class="p-5 mt-14">
                <div class="grid grid-cols-2 gap-4">
                    <div class="flex items-center h-24 rounded">
                        <svg class="flex-shrink-0 w-7 h-7 dark:text-gray-800 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white"
                            aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor"
                            viewBox="0 0 384 512">
                            <path
                                d="M40 48C26.7 48 16 58.7 16 72v48c0 13.3 10.7 24 24 24H88c13.3 0 24-10.7 24-24V72c0-13.3-10.7-24-24-24H40zM192 64c-17.7 0-32 14.3-32 32s14.3 32 32 32H480c17.7 0 32-14.3 32-32s-14.3-32-32-32H192zm0 160c-17.7 0-32 14.3-32 32s14.3 32 32 32H480c17.7 0 32-14.3 32-32s-14.3-32-32-32H192zm0 160c-17.7 0-32 14.3-32 32s14.3 32 32 32H480c17.7 0 32-14.3 32-32s-14.3-32-32-32H192zM16 232v48c0 13.3 10.7 24 24 24H88c13.3 0 24-10.7 24-24V232c0-13.3-10.7-24-24-24H40c-13.3 0-24 10.7-24 24zM40 368c-13.3 0-24 10.7-24 24v48c0 13.3 10.7 24 24 24H88c13.3 0 24-10.7 24-24V392c0-13.3-10.7-24-24-24H40z" />
                        </svg>
                        <p class="text-3xl font-bold text-gray-400 dark:text-gray-800 ml-4">Categorias</p>
                    </div>

                    <div class="flex items-center justify-end h-24 rounded">
                        <button
                            class="text-white dark:bg-green-800 dark:hover:bg-green-700 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center me-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800"
                            @click="showCreateCategoryModalHandler">
                            Adicionar Categoria
                        </button>
                    </div>
                </div>
            </div>

            <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
                <table class="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                    <thead class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                        <tr>
                            <th scope="col" class="px-8 py-3">Descrição</th>
                            <th scope="col" class="py-3">Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class=" bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600"
                            v-for="(category, index) in categories" :key="index">
                            <td class="px-8 py-4">{{ category.description }}</td>
                            <td class="py-4">
                                <!-- Modal toggle -->
                                <a @click="showUpdateCategoryModalHandler(category)" type="button"
                                    class="text-white bg-blue-700 hover:bg-blue-800 focus:outline-none focus:ring-4
                                                        focus:ring-blue-300 font-medium rounded-full text-sm px-5 py-2.5 text-center me-2 mb-2
                                                        dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
                                    Editar
                                </a>
                                <button type="button" @click="handleDeleteCategory(category.id)"
                                    class="text-white bg-red-700 hover:bg-red-800 focus:outline-none focus:ring-4 focus:ring-red-300 font-medium rounded-full text-sm px-5 py-2.5 text-center me-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900">
                                    Deletar
                                </button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <CreateCategoryView :show="showCreateCategoryModal" @close="hideCreateCategoryModalHandler" />
    <UpdateCategoryView :category="selectedCategory" :show="showUpdateCategoryModal"
        @close="hideUpdateCategoryModalHandler" />
</template>