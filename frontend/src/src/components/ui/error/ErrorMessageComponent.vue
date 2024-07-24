<template>
    <div v-if="messages.length" v-for="(message, index) in messages" class="small">
        <Message :key="index" severity="error" icon="pi pi-times-circle">
            {{ message }}
        </Message>
    </div>
</template>

<script lang="ts" setup>
import { defineProps } from 'vue';
import Message from 'primevue/message';

const props = defineProps<{ messages: string[] }>();
</script>