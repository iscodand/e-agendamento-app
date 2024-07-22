<script setup lang="ts">
import { FwbPagination } from 'flowbite-vue'
import { ref, watch, defineProps, defineEmits } from 'vue'

const props = defineProps({
    totalPages: {
        type: Number,
        default: 5
    },
    currentPage: {
        type: Number,
        default: 1
    }
});

const emits = defineEmits(['page-changed']);

const currentPage = ref(props.currentPage);

watch(currentPage, (newPage) => {
    emits('page-changed', newPage);
});

function onPageChange(page: number) {
    currentPage.value = page;
}
</script>

<template>
    <fwb-pagination v-model="currentPage" :total-pages="totalPages" @change="onPageChange" previous-label="<< Anterior"
        next-label="Próxima >>"></fwb-pagination>
</template>
