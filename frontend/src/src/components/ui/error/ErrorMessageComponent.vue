<template>
    <div v-if="messages.length" class="error-messages p-4 mb-4 text-sm text-red-700 bg-red-100 rounded-lg" role="alert">
        <ul>
            <li v-for="(message, index) in messages" :key="index">{{ message }}</li>
        </ul>
    </div>
</template>

<script lang="ts" setup>
import { defineProps } from 'vue';

const props = defineProps<{ messages: string[] }>();
</script>

<style scoped>
.error-messages {
    border: 1px solid #f5c6cb;
}
</style>