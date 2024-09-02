<script setup lang="ts">
import type { Schedule } from '@/services/schedules/types';
import { useScheduleStore } from '@/stores/schedules'
import { onMounted, ref } from 'vue';
import CreateScheduleView from './CreateScheduleView.vue'

const schedules = ref<Schedule[]>([]);
const schedulesStore = useScheduleStore();

// show create schedule modal
const showCreateScheduleModal = ref(false);
function showCreateScheduleModalHandler() {
    showCreateScheduleModal.value = true;
}
function hideCreateScheduleModalHandler() {
    showCreateScheduleModal.value = false;
}

onMounted(async () => {
    const { succeeded, data, status } = await schedulesStore.dispatchGetSchedules();

    if (succeeded) {
        schedules.value = data!;
    } else {
        console.log('Failed to get items from API. Status', status);
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
                        <p class="text-3xl font-bold text-gray-800 ml-4">Meus Agendamentos</p>
                    </div>

                    <div class="mb-4">
                        <div class="flex items-center justify-end h-24 rounded gap-10">
                            <IconField>
                                <InputIcon class="pi pi-search" />
                                <InputText placeholder="Buscar agendamento" />
                            </IconField>
                            <Button @click="showCreateScheduleModalHandler">
                                Realizar agendamento
                            </Button>
                        </div>
                    </div>
                </div>

                <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
                    <DataTable :value="schedules" paginator :rows="5" :rowsPerPageOptions="[5, 10, 20, 50]"
                        tableStyle="min-width: 50rem">

                        <Column field="observation" header="Observação"></Column>

                    </DataTable>
                </div>
            </div>
        </div>
    </div>

    <CreateScheduleView :show="showCreateScheduleModal" @hide="hideCreateScheduleModalHandler" />

</template>