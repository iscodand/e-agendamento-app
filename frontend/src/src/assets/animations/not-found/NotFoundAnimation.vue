<script setup lang="ts">

import notFoundJSON from "./not-found.json";
import { defineProps } from "vue";

const props = defineProps<{ text: string }>();

</script>

<template>
    <div class="animation text-center">
        <Vue3Lottie :animationData="notFoundJSON" :height="200" :width="200" />
        <span class="text">{{ text }}</span>
    </div>
</template>

<style scoped>
.animation {
    margin-bottom: 1rem;
}
</style>