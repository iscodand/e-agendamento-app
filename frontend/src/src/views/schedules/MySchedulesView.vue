<script setup lang="ts">
import { useScheduleStore } from '@/stores/schedules'
import { computed, onMounted, ref } from 'vue';
import CreateScheduleView from './CreateScheduleView.vue'
import { ClockIcon } from '@heroicons/vue/24/outline';
import formatDate from '@/utils/formatDate';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from "primevue/useconfirm";
import { ScheduleStatus, type Schedule } from '@/services/schedules/types';
import ManageScheduleView from './ManageScheduleView.vue';
import NotFoundAnimation from '@/assets/animations/not-found/NotFoundAnimation.vue';
import router from '@/router';
import { useRoute } from 'vue-router';

let schedules = ref<Schedule[]>([]);
const schedulesStore = useScheduleStore();
const toast = useToast();
const confirm = useConfirm();
const route = useRoute();

const selectedSchedule = ref<Schedule>();

const scheduleStatus = ref<string>(route.query.status as string || 'all');

schedules = computed(() => {
    return schedulesStore.schedules.map(schedule => ({
        ...schedule,
        formattedStartAt: formatDate(schedule.startedAt),
        formattedEndAt: formatDate(schedule.endAt)
    }));
});

// show create schedule modal
const showCreateScheduleModal = ref(false);
function showCreateScheduleModalHandler() {
    showCreateScheduleModal.value = true;
}
function hideCreateScheduleModalHandler() {
    showCreateScheduleModal.value = false;
}

// show manage schedule modal
const showManageScheduleModal = ref(false);
function showManageScheduleModalHandler(schedule: Schedule) {
    selectedSchedule.value = schedule;
    showManageScheduleModal.value = true;
}
function hideManageScheduleModalHandler() {
    showManageScheduleModal.value = false;
}

async function getSchedules(scheduleStatus: string): Promise<void> {
    const { succeeded, data, status } = await schedulesStore.dispatchGetSchedules(scheduleStatus);

    if (succeeded) {
        schedules.value = data!;
    } else {
        console.log('Failed to get items from API. Status', status);
    }
}

onMounted(async () => {
    await getSchedules("all");
})

async function deleteSchedule(scheduleId: string) {
    const { succeeded } = await schedulesStore.dispatchDeleteSchedule(scheduleId);

    if (succeeded) {
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
        case 'Canceled':
            return 'Cancelado';
        default:
            return 'Desconhecido';
    }
}

const confirmDelete = (event: any, scheduleId: string) => {
    confirm.require({
        target: event.currentTarget,
        message: 'Você tem certeza que quer deletar esse item?',
        icon: 'pi pi-exclamation-triangle',
        acceptLabel: 'Deletar',
        rejectLabel: 'Voltar',
        acceptClass: 'p-button-danger',
        rejectClass: 'p-button-info',
        accept: () => {
            deleteSchedule(scheduleId);
            console.log('Item deleted');
        },
        reject: () => {
            console.log('Delete action cancelled');
        }
    });
};

async function getClosedSchedules() {
    router.push({
        name: "my-schedules",
        query: { "status": "Canceled" }
    })
    await getSchedules("Canceled");
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
                        <p class="text-3xl font-bold text-gray-800 ml-4">Meus Agendamentos</p>
                    </div>

                    <div class="mb-4">
                        <div class="flex items-center justify-end h-24 rounded gap-10">
                            <IconField>
                                <InputIcon class="pi pi-search" />
                                <InputText placeholder="Buscar agendamento" />
                            </IconField>
                            <Button @click="getClosedSchedules">click me</Button>
                            <Button @click="showCreateScheduleModalHandler">
                                Realizar agendamento
                            </Button>
                        </div>
                    </div>
                </div>

                <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
                    <DataTable :value="schedules" paginator :rows="5" :rowsPerPageOptions="[5, 10, 20, 50]"
                        tableStyle="min-width: 50rem">

                        <template #empty>
                            <NotFoundAnimation text="Você não possui agendamentos cadastrados."
                                buttonLabel="Clique aqui para adicionar" :action=showCreateScheduleModalHandler />
                        </template>

                        <Column field="item.name" header="Item" style="width: 20%"></Column>
                        <Column field="requestedBy.fullName" header="Solicitado por" style="width: 20%"></Column>
                        <Column field="formattedStartAt" header="Data de Solicitação" style="width: 20%"></Column>
                        <Column field="formattedEndAt" header="Data de Devolução" style="width: 20%"></Column>
                        <Column field="status" header="Status" style="width: 20%">
                            <template #body="{ data }">
                                <Tag v-if="data.status === 'Pending'" value="Pendente" severity="warn" />
                                <Tag v-if="data.status === 'Open'" value="Aberto" severity="success" />
                                <Tag v-if="data.status === 'Closed'" value="Fechado" severity="danger" />
                                <Tag v-if="data.status === 'Canceled'" value="Cancelado" severity="danger" />
                                <Tag v-if="!['Pending', 'Open', 'Closed', 'Canceled'].includes(data.status)"
                                    value="Desconhecido" severity="info" />
                            </template>
                        </Column>
                        <Column style="width: 10%">
                            <template #body="{ data }">
                                <div class="flex gap-4">
                                    <Toast />
                                    <ConfirmPopup></ConfirmPopup>
                                    <Button @click="showManageScheduleModalHandler(data)" size="small" label="Gerenciar"
                                        severity="info" icon="pi pi-pencil" />
                                    <Button @click="confirmDelete($event, data.id)" size="small" label="Excluir"
                                        severity="danger" icon="pi pi-trash"></Button>
                                </div>
                            </template>
                        </Column>
                    </DataTable>
                </div>
            </div>
        </div>
    </div>

    <CreateScheduleView :show="showCreateScheduleModal" @close="hideCreateScheduleModalHandler" />
    <ManageScheduleView :schedule="selectedSchedule!" :show="showManageScheduleModal"
        @close="hideManageScheduleModalHandler" />

</template>