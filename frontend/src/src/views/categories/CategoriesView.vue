<script setup lang="ts">
import MainComponent from '@/components/ui/layout/MainComponent.vue'
import { useCategoryStore } from '@/stores/categories';
import { onMounted, ref, type Ref } from 'vue';
import CreateCategoryView from './CreateCategoryView.vue';
import UpdateCategoryView from './UpdateCategoryView.vue';
import type { Category } from '@/services/categories/types';
import { SwatchIcon } from '@heroicons/vue/24/outline';
import NotFoundAnimation from '@/assets/animations/not-found/NotFoundAnimation.vue';

//
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import Toast from 'primevue/toast';

import { useToast } from 'primevue/usetoast';

const toast = useToast();

const categoryStore = useCategoryStore()
const categories = ref<Category[]>([])

// show create category dialog handler
const createCategoryDialog = ref<boolean>(false);

function showCreateCategoryDialogHandler() {
    createCategoryDialog.value = true;
}
function hideCreateCategoryDialogHandler() {
    createCategoryDialog.value = false;
}

// show update category dialog handler
const selectedCategory = ref<Category>();
const updateCategoryDialog = ref<boolean>(false);

function showUpdateCategoryDialogHandler(category: Category) {
    updateCategoryDialog.value = true;
    selectedCategory.value = category;
}
function hideUpdateCategoryDialogHandler() {
    updateCategoryDialog.value = false;
}

async function handleDeleteCategory(categoryId: string): Promise<void> {
    const { succeeded } = await categoryStore.dispatchDeleteCategory(categoryId);

    if (succeeded) {
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Deletado com sucesso', life: 3000 });
    } else {
        toast.add({ severity: 'error', summary: 'Erro', detail: 'Você não pode deletar categorias com itens vinculados.', life: 3000 });
    }
}

onMounted(async () => {
    const { succeeded, data } = await categoryStore.dispatchGetCategories();

    if (succeeded) {
        categories.value = data!;
    } else {
        console.log('Failed to get categories from API.')
    }
})

</script>

<template>
    <div>
        <MainComponent />
        <div class="p-4 sm:ml-64">
            <div class="p-5">
                <div class="grid grid-cols-2 gap-4">
                    <div class="flex items-center h-24 rounded">
                        <SwatchIcon
                            class="flex-shrink-0 w-7 h-7 text-gray-800 transition duration-75  group-hover:text-gray-900" />
                        <p class="text-3xl font-bold text-gray-800 ml-4">Categorias</p>
                    </div>

                    <div class="mb-4">
                        <div class="flex items-center justify-end h-24 rounded">
                            <Button @click="showCreateCategoryDialogHandler">
                                Adicionar nova Categoria
                            </Button>
                        </div>
                    </div>
                </div>

                <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
                    <DataTable :value="categories" paginator :rows="5" :rowsPerPageOptions="[5, 10, 20, 50]"
                        tableStyle="min-width: 50rem">

                        <template #empty>
                            <NotFoundAnimation text="Você não possui categorias cadastradas."
                                buttonLabel="Clique aqui para adicionar" :action="showCreateCategoryDialogHandler" />
                        </template>

                        <Column style="width: 1%"></Column>
                        <Column field="description" header="Descrição" style="width: 25%" class="truncate"></Column>
                        <Column header="Itens cadastrados" style="width: 25%"></Column>
                        <Column style="width: 3%">
                            <template #body="{ data }">
                                <div class="flex gap-3">
                                    <Button @click="showUpdateCategoryDialogHandler(data)" size="small" label="Editar"
                                        severity="info" icon="pi pi-pencil" />

                                    <Toast />
                                    <Button @click="handleDeleteCategory(data.id)" size="small" label="Deletar"
                                        severity="danger" icon="pi pi-trash" />
                                </div>
                            </template>
                        </Column>
                    </DataTable>
                </div>
            </div>
        </div>
    </div>

    <CreateCategoryView :show="createCategoryDialog" @close="hideCreateCategoryDialogHandler" />
    <UpdateCategoryView :category="selectedCategory!" :show="updateCategoryDialog"
        @close="hideUpdateCategoryDialogHandler" />
</template>