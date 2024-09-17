<script setup lang="ts">
import { useScheduleStore } from '@/stores/schedules'
import { onMounted, ref } from 'vue';
import CreateScheduleView from './CreateScheduleView.vue'
import { ClockIcon } from '@heroicons/vue/24/outline';
import formatDate from '@/utils/formatDate';
import { useToast } from 'primevue/usetoast';

const schedules = ref<any>([]);
const schedulesStore = useScheduleStore();
const toast = useToast();

// show create schedule modal
const showCreateScheduleModal = ref(false);
function showCreateScheduleModalHandler() {
    showCreateScheduleModal.value = true;
}
function hideCreateScheduleModalHandler() {
    showCreateScheduleModal.value = false;
}

onMounted(async () => {
    const { succeeded, data, status } = await schedulesStore.dispatchGetSchedulesByCompany("7090367c-715f-4ccd-aec0-e9eaa4532726");

    if (succeeded) {
        const formatedSchedules = data!.map(schedule => ({
            ...schedule,
            formattedStartAt: formatDate(schedule.startedAt),
            formattedEndAt: formatDate(schedule.endAt)
        }));

        schedules.value = formatedSchedules;
    } else {
        console.log('Failed to get items from API. Status', status);
    }
})

async function deleteSchedule(scheduleId: string) {
    console.log(scheduleId)

    const { succeeded, data } = await schedulesStore.dispatchDeleteSchedule(scheduleId);

    if (succeeded) {
        schedules.value = data;
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Item deletado com sucesso.', life: 3000 });
    } else {
        toast.add({ severity: 'error', summary: 'Erro', detail: 'Ops. Erro ao deletar item', life: 3000 });
    }
}

function getStatusLabel(status: string): string {
    switch (status) {
        case 'Pending':
            return 'Pendente';
        case 'Open':
            return 'Aberto';
        case 'Closed':
            return 'Fechado';
        default:
            return 'Desconhecido';
    }
}

</script>

<template>
    <div>
        <MainComponent />
        <div class="p-4 sm:ml-64">
            <div class="p-5">
                <div class="grid grid-cols-2 gap-4">
                    <div class="flex items-center h-24 rounded">
                        <ClockIcon
                            class="flex-shrink-0 w-7 h-7 text-gray-800 transition duration-75 group-hover:text-gray-900" />
                        <p class="text-3xl font-bold text-gray-800 ml-4">Lista de Agendamentos</p>
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
                        <Column field="observation" header="Observação" style="width: 20%"></Column>
                        <Column field="requestedBy.fullName" header="Solicitado por" style="width: 20%"></Column>
                        <Column field="formattedStartAt" header="Data de Solicitação" style="width: 20%"></Column>
                        <Column field="formattedEndAt" header="Data de Devolução" style="width: 20%"></Column>
                        <Column field="status" header="Status" style="width: 20%">
                            <template #body="{ data }">
                                <Tag v-if="data.status === 'Pending'" value="Pendente" severity="warn" />
                                <Tag v-if="data.status === 'Open'" value="Aberto" severity="success" />
                                <Tag v-if="data.status === 'Closed'" value="Fechado" severity="danger" />
                                <Tag v-if="!['Pending', 'Open', 'Closed'].includes(data.status)" value="Desconhecido"
                                    severity="info" />
                            </template>
                        </Column>
                        <Column style="width: 10%">
                            <template #body="{ data }">
                                <div class="flex gap-4">
                                    <Button size="small" label="Gerenciar" severity="info" icon="pi pi-pencil" />
                                    <Toast />
                                    <Button size="small" @click="deleteSchedule(data.id)" label="Excluir"
                                        severity="danger" icon="pi pi-trash" />
                                </div>
                            </template>
                        </Column>
                    </DataTable>
                </div>
            </div>
        </div>
    </div>

    <CreateScheduleView :show="showCreateScheduleModal" @close="hideCreateScheduleModalHandler" />
    <UpdateScheduleView />

</template>