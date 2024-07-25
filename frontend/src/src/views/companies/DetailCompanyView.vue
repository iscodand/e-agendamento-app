<script setup lang="ts">

import MainComponent from '@/components/ui/layout/MainComponent.vue';
import type { Company } from '@/services/companies/types';
import { useCompanyStore } from '@/stores/companies';
import { computed, onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import type { User } from '@/services/user/types';
import CardComponent from '@/components/ui/dashboard/CardComponent.vue';
import NotFoundAnimation from '@/assets/animations/not-found/NotFoundAnimation.vue';

//
import Tab from 'primevue/tab';
import Tabs from 'primevue/tabs';
import TabList from 'primevue/tablist';
import TabPanel from 'primevue/tabpanel';
import TabPanels from 'primevue/tabpanels';
import InputText from 'primevue/inputtext';

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';

import Tag from 'primevue/tag';
import Button from 'primevue/button';

const companyStore = useCompanyStore();
const route = useRoute();

const company = ref<Company>({
    id: "",
    name: "Teste",
    description: "",
    cnpj: "",
    isActive: true
});

const employees = ref<User[]>();

const companyId = route.params.id.toString();

onMounted(async () => {
    const { data, succeeded } = await companyStore.dispatchGetCompanyById(companyId);

    if (succeeded) {
        loadEmployees();
        company.value = data;
    } else {
        console.log('deu ruim')
    }
})

async function loadEmployees() {
    const { data, succeeded } = await companyStore.dispatchGetEmployeesByCompany(companyId)

    if (succeeded) {
        employees.value = data;
    } else {
        console.log('deu ruim ao carregar empregados porra')
    }
}

const isCompanyLoaded = computed(() => company.value !== null);

</script>

<template>
    <div v-if="isCompanyLoaded">

        <MainComponent />

        <div class="p-4 sm:ml-64">
            <div class="p-5 ml-4">
                <div class="grid grid-cols-2">
                    <div class="flex flex-col items-start mt-8 gap-2">
                        <div class="items-center">
                            <div v-if="company.isActive" class="mb-2">
                                <Tag value="Ativa" severity="success" />
                            </div>
                            <div v-else class="mb-2">
                                <Tag value="Inativa" severity="danger" />
                            </div>
                            <p class="text-3xl font-bold text-gray-800">{{ company.name }}</p>
                        </div>
                        <div>
                            <p class="ml-1 mt-1"><b>CNPJ:</b> {{ company.cnpj }}</p>
                        </div>
                    </div>
                </div>


                <Tabs value="0" class="mt-4">
                    <TabList>
                        <Tab value="0">
                            <i class="pi pi-info-circle mr-2"></i>
                            Detalhes
                        </Tab>
                        <Tab value="2">
                            <i class="pi pi-calendar-clock mr-2"></i>
                            Agendamentos
                        </Tab>
                        <Tab value="1">
                            <i class="pi pi-users mr-2"></i>
                            Funcionários
                        </Tab>
                    </TabList>

                    <br>

                    <TabPanels>
                        <TabPanel value="0" class="flex flex-col items-center mt-4 mb-8">
                            <div class="flex gap-8 items-center">
                                <div class="flex flex-col gap-2 mb-4 w-64">
                                    <label for="companyName">
                                        Nome da Empresa
                                    </label>
                                    <InputText id="companyName" v-model="company.name"
                                        :invalid="company.name.length < 5" :disabled=true />
                                </div>

                                <div class="flex flex-col gap-2 mb-4 w-64">
                                    <label for="companyDescription">
                                        Descrição
                                    </label>
                                    <InputText id="companyDescription" v-model="company.description" :disabled=true />
                                </div>

                                <div class="flex flex-col gap-2 mb-4 w-64">
                                    <label for="companyCNPJ">
                                        CNPJ
                                    </label>
                                    <InputText id="companyCNPJ" v-model="company.cnpj" v-mask="'##.###.###/####-##'"
                                        :disabled=true />
                                </div>

                                <div class="mt-3">
                                    <Button label="Editar" icon="pi pi-pencil" />
                                </div>
                            </div>

                            <hr>

                            <div class="flex gap-8">
                                <div class="mt-10">
                                    <CardComponent title="Agendamentos em Aberto" :stat=3 icon="pi-calendar-clock" />
                                </div>
                                <div class="mt-10">
                                    <CardComponent title="Itens cadastrados" :stat=17 icon="pi-tag" />
                                </div>
                                <div class="mt-10">
                                    <CardComponent title="Funcionários ativos" :stat=3 icon="pi-users" />
                                </div>
                            </div>
                        </TabPanel>

                        <TabPanel value="1">
                            <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
                                <DataTable :value="employees" paginator :rows="5" :rowsPerPageOptions="[5, 10, 20, 50]"
                                    tableStyle="min-width: 50rem">

                                    <template #empty>
                                        <!-- TODO => desenvolver view para cadastrar novo usuario -->
                                        <NotFoundAnimation text="Essa empresa não possui funcionários cadastrados."
                                            buttonLabel="Clique aqui para adicionar" />
                                    </template>

                                    <Column field="fullName" header="Nome" style="width: 25%"></Column>
                                    <Column field="email" header="E-mail" style="width: 25%" class="truncate">
                                    </Column>
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
                        </TabPanel>

                        <TabPanel value="2">
                            <p class="m-0">
                                Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium
                                doloremque
                                laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi
                                architecto beatae vitae dicta sunt explicabo. Nemo enim
                                ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia
                                consequuntur
                                magni dolores eos qui ratione voluptatem sequi nesciunt. Consectetur, adipisci
                                velit, sed
                                quia non numquam eius modi.
                            </p>
                        </TabPanel>
                    </TabPanels>
                </Tabs>
            </div>
        </div>
    </div>

    <div v-else>
        <span>FOSASE</span>
    </div>


    <!-- DIVIDIR EM TABS -->

    <!-- Detalhes da empresa -->

    <!-- Estatísticas da empresa (quantidade de itens, agendamentos, etc...) -->

    <!-- Funcionários registrados na empresa -->

    <!-- Editar dados da empresa -->

    <!-- Desativar empresa -->

</template>