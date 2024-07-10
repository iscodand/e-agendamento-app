<script setup lang="ts">
import MainComponent from '@/components/ui/layout/MainComponent.vue'
import { useCompanyStore } from '@/stores/companies';
import { BuildingOffice2Icon } from "@heroicons/vue/24/outline";
import { onMounted, ref } from 'vue';

const companyStore = useCompanyStore();
const companies: any = ref([]);

onMounted(async () => {
    const { succeeded, data } = await companyStore.dispatchGetCompanies();

    if (succeeded) {
        companies.value = data;
    } else {
        console.log("Falha ao tentar recuperar empresas. Verifique o desenvolvedor.")
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
                        <BuildingOffice2Icon class=" flex-shrink-0 w-7 h-7 dark:text-gray-800 transition
                            duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
                        <p class="text-3xl font-bold text-gray-400 dark:text-gray-800 ml-4">Empresas</p>
                    </div>
                    <div class="flex items-center justify-end h-24 rounded">
                        <button
                            class="text-white dark:bg-green-800 dark:hover:bg-green-700 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center me-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800">
                            Adicionar nova empresa
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>