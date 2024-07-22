<script setup lang="ts">
import type { User } from '@/services/user/types';
import { useCompanyStore } from '@/stores/companies';
import { onMounted, ref } from 'vue';
import MainComponent from '@/components/ui/layout/MainComponent.vue';
import { UsersIcon } from '@heroicons/vue/24/outline';
import type { Company } from '@/services/companies/types';

const employees = ref<User[]>([]);
const companies = ref<Company[]>([]);
const selectedCompanyId = ref('');
const companyStore = useCompanyStore();


console.log(selectedCompanyId)

const fetchEmployees = async () => {
    const { succeeded, data, status } = await companyStore.dispatchGetEmployeesByCompany(selectedCompanyId.value);

    if (succeeded) {
        employees.value = data!;
    } else {
        console.log('deu ruim');
    }
};

onMounted(async () => {
    const { succeeded, data, status } = await companyStore.dispatchGetCompanies({ pageSize: 999, pageNumber: 1 });

    if (succeeded) {
        companies.value = data!;
        if (companies.value.length > 0) {
            selectedCompanyId.value = companies.value[0].id.toString();
            fetchEmployees();
        }
    } else {
        console.log('deu ruim');
    }
});

</script>

<template>
    <div>
        <MainComponent />
        <div class="p-4 sm:ml-64">
            <div class="p-5 mt-14">
                <div class="grid grid-cols-2 gap-4">
                    <div class="flex items-center h-24 rounded">
                        <UsersIcon
                            class="flex-shrink-0 w-7 h-7 text-gray-800 transition duration-75 text-gray-400 group-hover:text-gray-900 group-hover:text-white" />
                        <p class="text-3xl font-bold text-gray-400 text-gray-800 ml-4">Funcionários</p>
                    </div>

                    <div class="mb-4">
                        <label for="companies" class="block text-sm font-medium text-gray-700 mb-1">Empresa</label>
                        <select
                            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
                            v-model="selectedCompanyId" @change="fetchEmployees">
                            <option v-for="company in companies" :index="company.id" :value="company.id">
                                {{ company.name }}
                            </option>
                        </select>
                    </div>
                </div>
            </div>

            <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
                <table class="w-full text-sm text-left rtl:text-right text-gray-500 text-gray-400">
                    <thead class="text-xs text-gray-700 uppercase bg-gray-50 bg-gray-700 text-white">
                        <tr>
                            <th scope="col" class="px-8 py-3">Nome</th>
                            <th scope="col" class="px-8 py-3">E-mail</th>
                            <th scope="col" class="px-8 py-3">Ativo</th>
                            <th scope="col" class="px-8 py-3">Cargos</th>
                            <th scope="col" class="py-3"></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class=" bg-white border-b bg-gray-800 border-gray-700 hover:bg-gray-50 hover:bg-gray-600"
                            v-for="(employee, index) in employees" :key="index">
                            <td class="px-8 py-4">{{ employee.fullName }}</td>
                            <td class="px-8 py-4">{{ employee.email }}</td>
                            <td class="px-8 py-4">
                                <div v-if="employee.isActive" class="flex items-center">
                                    <div class="h-2.5 w-2.5 rounded-full bg-green-500 me-2"></div>
                                    <p>Ativo</p>
                                </div>
                                <div v-else class="flex items-center">
                                    <div class="h-2.5 w-2.5 rounded-full bg-red-500 me-2"></div>
                                    <p>Inativo</p>
                                </div>
                            </td>
                            <td class="px-8 py-4">{{ employee.roles }}</td>
                            <td class="py-4">
                                <!-- Modal toggle -->
                                <!-- <a @click="showUpdateItemModalHandler(item)" type="button" class="text-white bg-blue-700 hover:bg-blue-800 focus:outline-none focus:ring-4
                    focus:ring-blue-300 font-medium rounded-full text-sm px-5 py-2.5 text-center me-2 mb-2
                    bg-blue-600 hover:bg-blue-700 focus:ring-blue-800">
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
    </div>
</template>