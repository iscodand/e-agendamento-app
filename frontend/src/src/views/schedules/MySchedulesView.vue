<script setup lang="ts">
import MainComponent from '@/components/ui/layout/MainComponent.vue'
import type { Schedule } from '@/services/schedules/types';
import { useScheduleStore } from '@/stores/schedules'
import { onMounted, ref } from 'vue';

const schedules = ref<Schedule[]>([]);
const schedulesStore = useScheduleStore();

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
    <MainComponent />

</template>