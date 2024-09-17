<script setup lang="ts">

import { ScheduleStatus, type Schedule } from '@/services/schedules/types';
import { useScheduleStore } from '@/stores/schedules';
import { useToast } from 'primevue/usetoast';
import { computed } from 'vue';

const props = defineProps<{ show: boolean, schedule: Schedule }>();
const emit = defineEmits(['close', 'submit', 'update:show']);
const scheduleStore = useScheduleStore();
const toast = useToast();

const showDialog = computed({
    get: () => props.show,
    set: (value) => emit('update:show', value),
});

async function handleSubmit() { }

async function handleCancelSchedule() {
    const response = await scheduleStore.dispatchCancelSchedule(props.schedule.id);

    if (response.succeeded) {
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Agendamento cancelado com sucesso.', life: 3000 });
        hideModalHandler();
    }
}

function hideModalHandler() {
    emit('close');
}

</script>

<template>
    <div>
        <Dialog v-model:visible="showDialog" modal header="Gerenciar agendamento" :style="{ width: '30rem' }">
            <form @submit.prevent="handleSubmit">
                <div class="flex flex-col gap-2 mb-4">
                    <label for="itemName">
                        Nome
                    </label>
                    <InputText id="itemName" />
                </div>

                <div class="flex flex-col gap-2 mb-4">
                    <label for="itemDescription">
                        Descrição
                    </label>
                    <InputText id="itemDescription" />
                </div>

                <div class="flex justify-end gap-3">
                    <Toast />


                    <div v-if="!['Closed', 'Canceled'].includes(schedule.status.toString())">
                        <Button size="small" @click="handleCancelSchedule" label="Cancelar agendamento" severity="warn"
                            icon="pi pi-times-circle" />
                    </div>

                    <Button size="small" @click="hideModalHandler" label="Cancelar" severity="danger"
                        icon="pi pi-times" />
                    <Button size="small" label="Salvar" type="submit" severity="success" icon="pi pi-check" />
                </div>
            </form>
        </Dialog>
    </div>
</template>
