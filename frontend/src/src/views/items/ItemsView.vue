<script lang="ts" setup>
import { ref, onMounted, type Ref } from 'vue'
import { useItemStore } from '@/stores/items'
import { useCategoryStore } from '@/stores/categories'
import type { Item } from '@/services/items/types'
import MainComponent from '@/components/ui/layout/MainComponent.vue'
import CreateItemView from './CreateItemView.vue';
import UpdateItemView from './UpdateItemView.vue';
import { TagIcon } from '@heroicons/vue/24/outline'

// PrimeVue
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Tag from 'primevue/tag'
import Button from 'primevue/button'


// Create item modal
const showCreateItemModal = ref(false);

const selectedItem = ref();

function showCreateItemModalHandler() {
  showCreateItemModal.value = true;
}

function hideCreateItemModalHandler() {
  showCreateItemModal.value = false;
}

// Update item modal
const showUpdateItemModal = ref(false);

function showUpdateItemModalHandler(item: Item) {
  selectedItem.value = item;
  showUpdateItemModal.value = true;
}

function closeUpdateItemModalHandler() {
  showUpdateItemModal.value = false;
}

async function handleDeleteItem(itemId: string) {
  await itemsStore.dispatchDeleteItem(itemId);
}

const itemsStore = useItemStore()
const items: any = ref([])

const categoryStore = useCategoryStore()
const categories: any = ref([])

onMounted(async () => {
  const { succeeded, data, status } = await itemsStore.dispatchGetItems();

  if (succeeded) {
    items.value = data;
  } else {
    console.log('Failed to get items from API. Status:', status)
  }
})

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
      <div class="p-5">
        <div class="grid grid-cols-2 gap-4">
          <div class="flex items-center h-24 rounded">
            <TagIcon class="flex-shrink-0 w-7 h-7 text-gray-800 transition duration-75 group-hover:text-gray-900" />
            <p class="text-3xl font-bold text-gray-800 ml-4">Itens</p>
          </div>

          <div class="mb-4">
            <div class="flex items-center justify-end h-24 rounded">
              <Button @click="showCreateItemModalHandler">
                Adicionar Item
              </Button>
            </div>
          </div>
        </div>

        <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
          <DataTable :value="items" paginator :rows="5" :rowsPerPageOptions="[5, 10, 20, 50]"
            tableStyle="min-width: 50rem">
            <Column field="name" header="Nome" style="width: 22%"></Column>
            <Column field="description" header="Descrição" style="width: 22%" class="truncate"></Column>
            <Column field="quantityAvailable" header="Qtd. Disponível" style="width: 22%"></Column>
            <Column field="isAvailable" header="Disponível" style="width: 23%">
              <template #body="{ data }">
                <div v-if="data.isAvailable">
                  <Tag value="Disponível" severity="success" />
                </div>
                <div v-else>
                  <Tag value="Indisponível" severity="danger" />
                </div>
              </template>
            </Column>
            <Column>
              <template #body="{ data }">
                <ButtonGroup class="flex gap-4">
                  <Button @click="showUpdateItemModalHandler(data)" size="small" label="Editar" severity="info"
                    icon="pi pi-pencil" />
                </ButtonGroup>
              </template>
            </Column>
          </DataTable>
        </div>
      </div>
    </div>
  </div>

  <CreateItemView :show="showCreateItemModal" :categories="categories" @close="hideCreateItemModalHandler" />
  <UpdateItemView :item="selectedItem" :show="showUpdateItemModal" :categories="categories"
    @close="closeUpdateItemModalHandler" />
</template>

<style scoped></style>