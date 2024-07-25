<script lang="ts" setup>
import { defineProps } from 'vue';
import Message from 'primevue/message';

defineProps<{ messages: string[] }>();
</script>

<template>
    <div v-if="messages.length" v-for="(message, index) in messages" class="small mb-4">
        <Message :key="index" severity="error" icon="pi pi-times-circle">
            {{ message }}
        </Message>
    </div>
</template>