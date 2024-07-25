<script setup lang="ts">
import MainComponent from '@/components/ui/layout/MainComponent.vue'
import type { Schedule } from '@/services/schedules/types';
import { useScheduleStore } from '@/stores/schedules'
import type { MenuItem } from 'primevue/menuitem';
import Menubar from 'primevue/menubar';
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

const items: MenuItem[] = [{
    label: "Teste"
}];

</script>

<template>
    <Menubar :model="items">
        <template #start>
            <svg width="35" height="40" viewBox="0 0 35 40" fill="none" xmlns="http://www.w3.org/2000/svg" class="h-8">
                <path d="..." fill="var(--p-primary-color)" />
                <path d="..." fill="var(--p-text-color)" />
            </svg>
        </template>
        <template #item="{ item, props, hasSubmenu, root }">
            <a v-ripple class="flex items-center" v-bind="props.action">
                <span :class="item.icon" />
                <span class="ml-2">{{ item.label }}</span>
                <Badge v-if="item.badge" :class="{ 'ml-auto': !root, 'ml-2': root }" :value="item.badge" />
                <span v-if="item.shortcut"
                    class="ml-auto border border-surface rounded bg-emphasis text-muted-color text-xs p-1">{{
                        item.shortcut }}</span>
                <i v-if="hasSubmenu"
                    :class="['pi pi-angle-down', { 'pi-angle-down ml-2': root, 'pi-angle-right ml-auto': !root }]"></i>
            </a>
        </template>
        <template #end>
            <div class="flex items-center gap-2">
                <InputText placeholder="Search" type="text" class="w-32 sm:w-auto" />
                <Avatar image="/images/avatar/amyelsner.png" shape="circle" />
            </div>
        </template>
    </Menubar>
</template>