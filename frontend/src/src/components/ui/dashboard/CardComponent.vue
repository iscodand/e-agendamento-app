<script setup lang="ts">

import Card from 'primevue/card';

defineProps<{ title: string, subtitle?: string, stat: number, icon: string }>()

</script>

<template>
    <Card style="min-width: 17rem; overflow: hidden; background-color: #10b981;"
        class="transition ease-in-out delay-50 shadow hover:shadow-2xl">
        <template #title>
            <div class="text-center">
                <i :class="['pi', icon, 'm-2']" style="font-size: 1.5rem; color: #e5e7eb;"></i>
            </div>
            <div class="text-center">
                <p class="text-white">{{ title }}</p>
            </div>
        </template>

        <template v-if="subtitle" #subtitle>
            <p class="text-white">{{ subtitle }}</p>
        </template>

        <template #content>
            <div class="text-center">
                <h1 style="font-size: 3rem; margin-bottom: -0.5rem;" class="text-white">{{ stat }}</h1>
            </div>
        </template>
    </Card>
</template>