<script setup lang="ts">
import MainComponent from '@/components/ui/layout/MainComponent.vue'
import CreateCompanyView from './CreateCompanyView.vue';
import { useCompanyStore } from '@/stores/companies';
import { BuildingOffice2Icon } from "@heroicons/vue/24/outline";
import { onMounted, ref, type Ref } from 'vue';
import NotFoundAnimation from '@/assets/animations/not-found/NotFoundAnimation.vue';
import InputText from 'primevue/inputtext';
import IconField from 'primevue/iconfield';
import InputIcon from 'primevue/inputicon';

//
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Tag from 'primevue/tag'
import Button from 'primevue/button'
import Paginator from 'primevue/paginator';
import router from '@/router';

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

function goToCompanyDetails(companyId: string): void {
    router.push({ name: 'company-details', params: { id: companyId } });
}

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
                        <div class="flex items-center justify-end h-24 gap-10 ">
                            <IconField>
                                <InputIcon class="pi pi-search" />
                                <InputText placeholder="Buscar empresa" />
                            </IconField>
                            <Button @click="showCreateCompanyModalHandler">
                                Adicionar nova empresa
                            </Button>
                        </div>
                    </div>
                </div>

                <div class="relative overflow-x-auto shadow-md sm:rounded-lg">

                    <DataTable :value="companies" tableStyle="min-width: 50rem">

                        <template #empty>
                            <NotFoundAnimation text="Não há empresas cadastradas." />
                        </template>

                        <Column field="name" header="Nome" style="width: 20%;"></Column>
                        <Column field="description" header="Descrição" style="width: 20%"
                            class="text-clip overflow-hidden text-justify">
                        </Column>
                        <Column field="cnpj" header="CNPJ" style="width: 20%"></Column>
                        <Column field="isAvailable" header="Situação" style="width: 15%">
                            <template #body="{ data }">
                                <div v-if="data.isActive">
                                    <Tag value="Ativa" severity="success" />
                                </div>
                                <div v-else>
                                    <Tag value="Inativa" severity="danger" />
                                </div>
                            </template>
                        </Column>
                        <Column style="width: 10%">
                            <template #body="{ data }">
                                <Button @click="goToCompanyDetails(data.id)" size="small" label="Detalhes"
                                    severity="info" icon="pi pi-info-circle" />
                            </template>
                        </Column>
                    </DataTable>
                    <Paginator :rows="parameters.pageSize" :totalRecords="totalCompanies" @page="onPageChange" />

                </div>
            </div>
        </div>
    </div>

    <CreateCompanyView :show="showCreateCompanyModal" @close="hideCreateCompanyModalHandler" />
</template>