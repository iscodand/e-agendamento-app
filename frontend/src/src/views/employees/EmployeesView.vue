<script setup lang="ts">
import type { User } from '@/services/user/types';
import { useCompanyStore } from '@/stores/companies';
import { onMounted, ref } from 'vue';
import MainComponent from '@/components/ui/layout/MainComponent.vue';
import { UsersIcon } from '@heroicons/vue/24/outline';
import type { Company } from '@/services/companies/types';

// PrimeVue
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Tag from 'primevue/tag';
import AutoComplete from 'primevue/autocomplete';

// Animations
import NotFoundAnimation from '@/assets/animations/not-found/NotFoundAnimation.vue'

const employees = ref<User[]>([]);
const companies = ref<Company[]>([]);
const selectedCompanyId = ref<Company>();
const companyStore = useCompanyStore();

const fetchEmployees = async () => {
    const { succeeded, data } = await companyStore.dispatchGetEmployeesByCompany(selectedCompanyId.value!.id);

    if (succeeded) {
        employees.value = data!;
    } else {
        console.log('deu ruim 1');
    }
};

onMounted(async () => {
    const { succeeded, data } = await companyStore.dispatchGetCompanies({ pageSize: 999, pageNumber: 1 });


    if (succeeded) {
        companies.value = data!;
        if (companies.value.length > 0) {
            selectedCompanyId.value!.id = companies.value[0].id.toString();
            // fetchEmployees();
        }
    } else {
        console.log('deu ruim 2');
    }
});

const filteredCompanies = ref<Company[]>([]);

const searchCompany = (event: { query: string }) => {
    const query = event.query.toLowerCase();

    filteredCompanies.value = companies.value.filter((company: Company) =>
        company.name.toLowerCase().includes(query)
    );
};

</script>

<template>
    <div>
        <MainComponent />

        <div class="p-4 sm:ml-64">
            <div class="p-5">
                <div class="grid grid-cols-2 gap-4 justify-between items-center">
                    <div class="flex items-center h-24 rounded">
                        <UsersIcon
                            class="flex-shrink-0 w-7 h-7 text-gray-800 transition duration-75 group-hover:text-gray-900" />
                        <p class="text-3xl font-bold text-gray-800 ml-4">Funcionários</p>
                    </div>

                    <div class="mb-4 items-center">
                        <div class="flex justify-end">
                            <div class="flex flex-col">
                                <div class="flex gap-4 items-center mb-2">
                                    <i class="pi pi-search"></i>
                                    <span>Buscar por Empresa</span>
                                </div>
                                <AutoComplete v-model="selectedCompanyId" dropdown :suggestions="filteredCompanies"
                                    @complete="searchCompany" optionLabel="name" @option-select="fetchEmployees"
                                    class="w-96" />
                            </div>
                        </div>
                    </div>
                </div>


                <!-- <div class="p-4 sm:ml-64">
            <div class="p-5">
                <div class="flex p-5 justify-between items-center">
                    <div class="flex items-center h-24 rounded">
                        <UsersIcon
                            class="flex-shrink-0 w-7 h-7 text-gray-800 transition duration-75 group-hover:text-gray-900" />
                        <p class="text-3xl font-bold text-gray-800 ml-4">Funcionários</p>
                    </div>

                    <div class="flex flex-col rounded">
                        <label for="companies" class="block text-sm font-medium text-gray-700 mb-1 ml-8">
                            Pesquisar Empresa
                        </label>
                        <div class="flex gap-4 items-center">
                            <i class="pi pi-search"></i>
                            <AutoComplete v-model="selectedCompanyId" dropdown :suggestions="filteredCompanies"
                                @complete="searchCompany" optionLabel="name" @option-select="fetchEmployees"
                                class="w-96" />
                        </div>
                    </div>
                </div> -->

                <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
                    <DataTable :value="employees" paginator :rows="5" :rowsPerPageOptions="[5, 10, 20, 50]"
                        tableStyle="min-width: 50rem">

                        <template #empty>
                            <NotFoundAnimation text="Funcionários não encontrados" />
                        </template>

                        <Column field="fullName" header="Nome" style="width: 25%"></Column>
                        <Column field="email" header="E-mail" style="width: 25%" class="truncate"></Column>
                        <Column field="isActive" header="Ativo" style="width: 25%">
                            <template #body="{ data }">
                                <div v-if="data.isActive">
                                    <Tag value="Ativo" severity="success" />
                                </div>
                                <div v-else>
                                    <Tag value="Inativo" severity="danger" />
                                </div>
                            </template>
                        </Column>
                        <Column field="roles" header="Cargos" style="width: 25%"></Column>
                    </DataTable>
                </div>
            </div>
        </div>
    </div>
</template>