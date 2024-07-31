<script setup lang="ts">

import notFoundJSON from "./not-found.json";
import { defineProps } from "vue";
import Button from 'primevue/button'

const props = defineProps<{ text: string, buttonLabel?: string, buttonLink?: string, action?: any }>();

</script>

<template>
    <div class="animation text-center">
        <Vue3Lottie :animationData="notFoundJSON" :height="200" :width="200" />
        <span class="text">{{ text }}</span>
        <div v-if="buttonLabel" class="mt-4">
            <Button :label="buttonLabel" :link="buttonLink" size="small" @click=action />
        </div>
    </div>
</template>

<style scoped>
.animation {
    margin-bottom: 1rem;
}
</style>