<script setup lang="ts">
import MainComponent from '@/components/ui/layout/MainComponent.vue'
import CreateCompanyView from './CreateCompanyView.vue';
import { useCompanyStore } from '@/stores/companies';
import { BuildingOffice2Icon } from "@heroicons/vue/24/outline";
import { onMounted, ref, type Ref } from 'vue';

//
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Tag from 'primevue/tag'
import Button from 'primevue/button'
import Paginator from 'primevue/paginator';

const companyStore = useCompanyStore();
const companies: any = ref([]);

const showCreateCompanyModal = ref(false);

function showCreateCompanyModalHandler() {
    showCreateCompanyModal.value = true;
}

function hideCreateCompanyModalHandler() {
    showCreateCompanyModal.value = false;
}

const parameters = ref({
    pageSize: 8,
    pageNumber: 1
});

const totalCompanies = ref<number>(0);

const fetchCompanies = async (pageNumber = 1) => {
    parameters.value.pageNumber = pageNumber;
    const { succeeded, data, totalItems } = await companyStore.dispatchGetCompanies(parameters.value);

    if (succeeded) {
        companies.value = data;
        totalCompanies.value = totalItems!;
    } else {
        console.log('deu ruim');
    }
};

const onPageChange = (event: any) => {
    fetchCompanies(event.page + 1);
};

onMounted(async () => {
    await fetchCompanies();
})

</script>

<template>
    <div>
        <MainComponent />
        <div class="p-4 sm:ml-64">
            <div class="p-5">
                <div class="grid grid-cols-2 gap-4">
                    <div class="flex items-center h-24 rounded">
                        <BuildingOffice2Icon class=" flex-shrink-0 w-7 h-7 transition
                            duration-75 text-gray-800 group-hover:text-gray-900" />
                        <p class="text-3xl font-bold text-gray-800 ml-4">Empresas</p>
                    </div>

                    <div class="mb-4">
                        <div class="flex items-center justify-end h-24 rounded">
                            <Button @click="showCreateCompanyModalHandler">
                                Adicionar nova empresa
                            </Button>
                        </div>
                    </div>
                </div>

                <div class="relative overflow-x-auto shadow-md sm:rounded-lg">

                    <DataTable :value="companies" tableStyle="min-width: 50rem">
                        <Column field="name" header="Nome" style="width: 25%"></Column>
                        <Column field="description" header="Descrição" style="width: 25%"></Column>
                        <Column field="cnpj" header="CNPJ" style="width: 25%"></Column>
                        <Column field="isAvailable" header="Situação" style="width: 25%">
                            <template #body="{ data }">
                                <div v-if="data.isActive">
                                    <Tag value="Ativa" severity="success" />
                                </div>
                                <div v-else>
                                    <Tag value="Inativa" severity="danger" />
                                </div>
                            </template>
                        </Column>
                        <!-- <Column>
                            <template #body="{ data }">
                                <ButtonGroup class="flex gap-4">
                                    <Button size="small" label="Editar" severity="info" icon="pi pi-pencil" />
                                </ButtonGroup>
                            </template>
                        </Column> -->
                    </DataTable>
                    <Paginator :rows="parameters.pageSize" :totalRecords="totalCompanies" @page="onPageChange" />

                </div>
            </div>
        </div>
    </div>

    <CreateCompanyView :show="showCreateCompanyModal" @close="hideCreateCompanyModalHandler" />
</template>