<script setup lang="ts">

import MainComponent from '@/components/ui/layout/MainComponent.vue';
import type { Company, InputUpdateCompany } from '@/services/companies/types';
import { useCompanyStore } from '@/stores/companies';
import { computed, onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import type { User } from '@/services/user/types';
import CardComponent from '@/components/ui/dashboard/CardComponent.vue';
import { useToast } from 'primevue/usetoast';
import { useUserStore } from '@/stores/user';
import ErrorMessageComponent from '@/components/ui/error/ErrorMessageComponent.vue';
import EmployeesView from '../employees/EmployeesView.vue';

const toast = useToast();
const companyStore = useCompanyStore();
const userStore = useUserStore();
const route = useRoute();

const company = ref<Company>({
    id: "",
    name: "",
    description: "",
    cnpj: "",
    isActive: true
});

const employees = ref<User[]>();
const errorMessages = ref<string[]>([]);

let request = ref<InputUpdateCompany>({
    name: company.value.name,
    cnpj: company.value.cnpj,
    description: company.value.description,
    isActive: company.value.isActive
})

const companyId = route.params.id.toString();

onMounted(async () => {
    const { data, succeeded } = await companyStore.dispatchGetCompanyById(companyId);

    if (succeeded) {
        loadEmployees();
        company.value = data!;
        request.value = {
            name: company.value.name,
            cnpj: company.value.cnpj,
            description: company.value.description,
            isActive: company.value.isActive
        }
    } else {
        console.log('deu ruim')
    }
})

async function updateCompany() {
    errorMessages.value = [];

    const response = await companyStore.dispatchUpdateCompany(companyId, request.value);

    if (response.succeeded) {
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Empresa atualizada com sucesso.', life: 3000 });
        enableUpdate()

        company.value = {
            id: companyId,
            name: request.value.name,
            cnpj: request.value.cnpj,
            description: request.value.description,
            isActive: request.value.isActive
        }
    } else {
        errorMessages.value.push(response.errors![0]);
    }
}

async function loadEmployees() {
    const { data, succeeded } = await userStore.dispatchGetEmployeesByCompany(companyId)

    if (succeeded) {
        employees.value = data;
    } else {
        console.log('deu ruim ao carregar empregados porra')
    }
}

const isCompanyLoaded = computed(() => company.value !== null);

const updateDisabled = ref<boolean>(true);

function enableUpdate() {
    errorMessages.value = []
    updateDisabled.value = !updateDisabled.value;
}

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
                        <TabPanel value="0" class="flex flex-col mt-4 mb-8">

                            <section id="companyDetails">
                                <div class="flex gap-10">
                                    <div class="flex flex-col gap-2 mb-2 w-64">
                                        <label for="companyName">
                                            Nome da Empresa
                                        </label>
                                        <InputText id="companyName" v-model="request.name"
                                            :invalid="request.name.length < 5" :disabled=updateDisabled />
                                    </div>

                                    <div class="flex flex-col gap-2 mb-4 w-64">
                                        <label for="companyDescription">
                                            Descrição
                                        </label>
                                        <Textarea id="companyDescription" v-model="request.description"
                                            :disabled=updateDisabled :rows="1" />
                                    </div>
                                </div>

                                <div class="flex gap-10">
                                    <div class="flex flex-col gap-2 mb-4 w-64">
                                        <label for="companyCNPJ">
                                            CNPJ
                                        </label>
                                        <InputText id="companyCNPJ" v-model="request.cnpj" v-mask="'##.###.###/####-##'"
                                            :disabled=updateDisabled />
                                    </div>

                                    <div class="flex flex-col gap-2 mb-4 w-64">
                                        <label for="companyIsActive">
                                            Situação
                                        </label>
                                        <ToggleButton v-model="request.isActive" onLabel="Ativa" offLabel="Inativa"
                                            onIcon="pi pi-check" offIcon="pi pi-times" class="w-full sm:w-40"
                                            aria-label="Confirmation" :disabled=updateDisabled />
                                    </div>
                                </div>

                                <div class="w-96 mt-4">
                                    <ErrorMessageComponent :messages="errorMessages" />
                                </div>

                                <div class="mt-2 mb-8">
                                    <Button v-if="updateDisabled" label="Editar" icon="pi pi-pencil"
                                        @click="enableUpdate" severity="info" />

                                    <Toast />
                                    <div class="flex gap-8">
                                        <Button v-if="!updateDisabled" label="Cancelar" severity="danger"
                                            icon="pi pi-times" @click="enableUpdate" />
                                        <Button v-if="!updateDisabled" label="Salvar" @click="updateCompany"
                                            icon="pi pi-check" />
                                    </div>
                                </div>
                            </section>

                            <hr>

                            <section id="cards">
                                <div class="flex items-center col-span-3 gap-8">
                                    <div class="mt-10">
                                        <CardComponent title="Agendamentos em Aberto" :stat=3
                                            icon="pi-calendar-clock" />
                                    </div>
                                    <div class="mt-10">
                                        <CardComponent title="Itens cadastrados" :stat=17 icon="pi-tag" />
                                    </div>
                                    <div class="mt-10">
                                        <CardComponent title="Funcionários ativos" :stat=3 icon="pi-users" />
                                    </div>
                                </div>
                            </section>
                        </TabPanel>

                        <TabPanel value="1">
                            <EmployeesView :company="company" :employees="employees!" />
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

</template>