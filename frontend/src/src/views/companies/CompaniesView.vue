<script setup lang="ts">
import MainComponent from '@/components/ui/layout/MainComponent.vue'
import CreateCompanyView from './CreateCompanyView.vue';
import { useCompanyStore } from '@/stores/companies';
import { BuildingOffice2Icon } from "@heroicons/vue/24/outline";
import { onMounted, ref, type Ref } from 'vue';
import PaginationComponent from '@/components/ui/pagination/PaginationComponent.vue';
import type { RequestParameters } from '@/services/types';

const companyStore = useCompanyStore();
const companies: any = ref([]);

const showCreateCompanyModal = ref(false);

function showCreateCompanyModalHandler() {
    showCreateCompanyModal.value = true;
}

function hideCreateCompanyModalHandler() {
    showCreateCompanyModal.value = false;
}

const parameters: RequestParameters = {
    pageSize: 8,
    pageNumber: 1
}

const totalPages: Ref<number> = ref<number>(0);

const fetchCompanies = async (pageNumber: number = 1) => {
    parameters.pageNumber = pageNumber;
    const { succeeded, data, totalItems } = await companyStore.dispatchGetCompanies(parameters);

    if (succeeded) {
        companies.value = data!;
        totalPages.value = Math.ceil(totalItems! / parameters.pageSize);
    } else {
        console.log('deu ruim');
    }
};

onMounted(async () => {
    await fetchCompanies();
})

</script>

<template>
    <div>
        <MainComponent />
        <div class="p-4 sm:ml-64">
            <div class="p-5 mt-14">
                <div class="grid grid-cols-2 gap-4">
                    <div class="flex items-center h-24 rounded">
                        <BuildingOffice2Icon class=" flex-shrink-0 w-7 h-7 transition
                            duration-75 text-gray-400 group-hover:text-gray-900" />
                        <p class="text-3xl font-bold text-gray-400 ml-4">Empresas</p>
                    </div>

                    <div class="mb-4">
                        <div class="flex items-center justify-end h-24 rounded">
                            <button @click="showCreateCompanyModalHandler"
                                class="text-white bg-green-800 hover:bg-green-700 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center me-2">
                                Adicionar nova empresa
                            </button>
                        </div>
                    </div>
                </div>

                <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
                    <table class="w-full text-sm text-left rtl:text-right text-gray-500">
                        <thead class="text-xs text-gray-700 uppercase bg-gray-50">
                            <tr>
                                <th scope="col" class="px-8 py-3">Nome</th>
                                <th scope="col" class="px-8 py-3">CNPJ</th>
                                <th scope="col" class="px-8 py-3">Descrição</th>
                                <th scope="col" class="px-8 py-3">Ativa</th>
                                <th scope="col" class="py-3"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr class=" bg-white border-b border-gray-700" v-for="(company, index) in companies"
                                :key="index">
                                <td class="px-8 py-4">{{ company.name }}</td>
                                <td class="px-8 py-4">{{ company.cnpj }}</td>
                                <td v-if="company.description" class="px-8 py-4">{{ company.description }}</td>
                                <td v-else class="px-8 py-4">Sem descrição</td>
                                <td class="px-8 py-4">
                                    <div v-if="company.isActive" class="flex items-center">
                                        <div class="h-2.5 w-2.5 rounded-full bg-green-500 me-2"></div>
                                        <p>Ativa</p>
                                    </div>
                                    <div v-else class="flex items-center">
                                        <div class="h-2.5 w-2.5 rounded-full bg-red-500 me-2"></div>
                                        <p>Inativa</p>
                                    </div>
                                </td>
                                <td class="py-4">
                                    <!-- Modal toggle -->
                                    <a
                                        class="text-white bg-blue-700 hover:bg-blue-800 focus:outline-none focus:ring-4
                                        focus:ring-blue-300 font-medium rounded-full text-sm px-5 py-2.5 text-center me-2 mb-2">
                                        Detalhes
                                    </a>
                                    <!-- <a @click="showUpdateItemModalHandler(item)" type="button" ">
                                        Editar
                                    </a>
                                    <button @click="handleDeleteItem(item.id)" type="button"
                                        class="text-white bg-red-700 hover:bg-red-800 focus:outline-none focus:ring-4 focus:ring-red-300 font-medium rounded-full text-sm px-5 py-2.5 text-center me-2 mb-2 bg-red-600 hover:bg-red-700 focus:ring-red-900">
                                        Deletar
                                    </button> -->
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <div class="text-center">
                <PaginationComponent :total-pages="totalPages" :current-page="parameters.pageNumber"
                    @page-changed="fetchCompanies" />
            </div>
        </div>
    </div>

    <CreateCompanyView :show="showCreateCompanyModal" @close="hideCreateCompanyModalHandler" />
</template>