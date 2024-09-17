<script setup lang="ts">

import type { InputCreateSchedule } from '@/services/schedules/types';
import { useScheduleStore } from '@/stores/schedules';
import { computed, ref, type Ref } from 'vue';
import { useToast } from 'primevue/usetoast';
import type { Item } from '@/services/items/types';
import { useItemStore } from '@/stores/items';
import schedules from '@/services/schedules';

const props = defineProps<{ show: boolean }>();

const scheduleStore = useScheduleStore();
const itemsStore = useItemStore();
const toast = useToast();

const items: Ref<Item[]> | undefined = ref<Item[]>([]);

const errorMessages: Ref<string[]> = ref<string[]>([]);

let request = ref<InputCreateSchedule>({
    item: undefined,
    itemId: '',
    observation: '',
    startAt: new Date(),
    endAt: new Date()
})

const emit = defineEmits(['close', 'submit', 'create:show']);

const showDialog = computed({
    get: () => props.show,
    set: (value) => emit('create:show', value),
});

function hideModalHandler() {
    emit('close');
}

async function searchItems(event: { query: string }) {
    const searchTerm = event.query;
    try {
        const { succeeded, data } = await itemsStore.dispatchSearchItems(searchTerm);
        if (succeeded) {
            items!.value = data!;
        } else {
            throw new Error('Erro ao recuperar items.');
        }
    } catch (error) {
        toast.add({
            severity: 'error',
            summary: 'Erro',
            detail: 'Erro ao recuperar items. Tente novamente.',
            life: 3000
        });
        errorMessages.value.push('Erro ao recuperar items. Tente novamente.');
    }
}

async function handleSubmit() {
    const input = ref<InputCreateSchedule>({
        itemId: request.value.item!.id,
        observation: request.value.observation,
        startAt: request.value.startAt,
        endAt: request.value.endAt
    })

    if (!request.value.item) {
        toast.add({
            severity: 'error',
            summary: 'Erro',
            detail: 'Por favor, selecione um item.',
            life: 3000
        });
        return;
    }

    const { succeeded, errors } = await scheduleStore.dispatchCreateSchedule(input.value);

    if (succeeded) {
        hideModalHandler();
        toast.add({
            severity: 'success',
            summary: 'Sucesso',
            detail: 'Agendamento realizado com sucesso. Aguarde a confirmação de um superior',
            life: 3000
        });
    }
    else {
        errorMessages.value.push(errors![0])
    }
}

</script>

<template>
    <Dialog v-model:visible="showDialog" modal header="Agendar item" :style="{ width: '30rem' }">
        <form @submit.prevent="handleSubmit">
            <div class="flex flex-col gap-2 mb-4">
                <label for="scheduleObservation">
                    Observação
                </label>
                <Textarea id="scheduleObservation" v-model="request.observation"
                    :invalid="request.observation.length < 5" />
            </div>

            <div class="flex flex-col gap-2 mb-4">
                <label for="scheduleObservation">
                    Buscar Item
                </label>
                <AutoComplete v-model="request.item" :suggestions="items" @complete="searchItems" dropdown
                    optionLabel="description" :invalid="request.itemId === null" />
            </div>

            <div class="flex flex-col gap-2 mb-4">
                <label for="startAt">
                    Data do Agendamento
                </label>
                <DatePicker v-model="request.startAt" showIcon fluid iconDisplay="input" />
            </div>

            <div class="flex flex-col gap-2 mb-4">
                <label for="endAt">
                    Data de Encerramento
                </label>
                <DatePicker v-model="request.endAt" showIcon fluid iconDisplay="input" />
            </div>

            <ErrorMessageComponent :messages="errorMessages" />

            <div class="flex justify-end gap-3">
                <Toast />
                <Button size="small" @click="hideModalHandler" label="Cancelar" severity="danger" icon="pi pi-times" />
                <Button size="small" label="Salvar" type="submit" severity="success" icon="pi pi-check" />
            </div>
        </form>
    </Dialog>
</template>