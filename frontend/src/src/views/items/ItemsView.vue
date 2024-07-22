<script lang="ts" setup>
import { ref, onMounted, type Ref } from 'vue'
import { useItemStore } from '@/stores/items'
import { useCategoryStore } from '@/stores/categories'
import type { Item } from '@/services/items/types'
import MainComponent from '@/components/ui/layout/MainComponent.vue'
import CreateItemView from './CreateItemView.vue';
import UpdateItemView from './UpdateItemView.vue';
import { TagIcon } from '@heroicons/vue/24/outline'

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
      <div class="p-5 mt-14">
        <div class="grid grid-cols-2 gap-4">
          <div class="flex items-center h-24 rounded">
            <TagIcon
              class="flex-shrink-0 w-7 h-7 text-gray-800 transition duration-75 text-gray-400 group-hover:text-gray-900 group-hover:text-white" />
            <p class="text-3xl font-bold text-gray-400 text-gray-800 ml-4">Items</p>
          </div>

          <div class="mb-4">
            <div class="flex items-center justify-end h-24 rounded">
              <button
                class="text-white bg-green-800 hover:bg-green-700 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center me-2 bg-green-600 hover:bg-green-700 focus:ring-green-800"
                @click="showCreateItemModalHandler">
                Adicionar Item
              </button>
            </div>
          </div>
        </div>

        <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
          <table class="w-full text-sm text-left rtl:text-right text-gray-500 text-gray-400">
            <thead class="text-xs text-gray-700 uppercase bg-gray-50 bg-gray-700 text-gray-400">
              <tr>
                <th scope="col" class="px-8 py-3">Nome</th>
                <th scope="col" class="px-8 py-3">Descrição</th>
                <th scope="col" class="px-8 py-3">Itens Disponíveis</th>
                <th scope="col" class="px-8 py-3">Disponível</th>
                <th scope="col" class="py-3"></th>
              </tr>
            </thead>
            <tbody>
              <tr class=" bg-white border-b bg-gray-800 border-gray-700 hover:bg-gray-50 hover:bg-gray-600"
                v-for="(item, index) in items" :key="index">
                <td class="px-8 py-4">{{ item.name }}</td>
                <td v-if="item.description" class="px-8 py-4">{{ item.description }}</td>
                <td v-else class="px-8 py-4">Sem descrição</td>
                <td class="px-8 py-4">{{ item.quantityAvailable }} itens</td>
                <td class="px-8 py-4">
                  <div v-if="item.isAvailable" class="flex items-center">
                    <div class="h-2.5 w-2.5 rounded-full bg-green-500 me-2"></div>
                    <p>Disponível</p>
                  </div>
                  <div v-else class="flex items-center">
                    <div class="h-2.5 w-2.5 rounded-full bg-red-500 me-2"></div>
                    <p>Indisponível</p>
                  </div>
                </td>
                <td class="py-4">
                  <!-- Modal toggle -->
                  <a @click="showUpdateItemModalHandler(item)" type="button" class="text-white bg-blue-700 hover:bg-blue-800 focus:outline-none focus:ring-4
                    focus:ring-blue-300 font-medium rounded-full text-sm px-5 py-2.5 text-center me-2 mb-2
                    bg-blue-600 hover:bg-blue-700 focus:ring-blue-800">
                    Editar
                  </a>
                  <button @click="handleDeleteItem(item.id)" type="button"
                    class="text-white bg-red-700 hover:bg-red-800 focus:outline-none focus:ring-4 focus:ring-red-300 font-medium rounded-full text-sm px-5 py-2.5 text-center me-2 mb-2 bg-red-600 hover:bg-red-700 focus:ring-red-900">
                    Deletar
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>

  <CreateItemView :show="showCreateItemModal" :categories="categories" @close="hideCreateItemModalHandler" />
  <UpdateItemView :item="selectedItem" :show="showUpdateItemModal" :categories="categories"
    @close="closeUpdateItemModalHandler" />
</template>

<style scoped></style>