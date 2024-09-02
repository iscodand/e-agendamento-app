<script setup lang="ts">

import { defineProps } from 'vue';

defineProps({
    isLoading: {
        type: Boolean,
        required: true
    }
})

</script>

<template>
    <div class="">
        <ProgressSpinner style="width: 50px; height: 50px" strokeWidth="8" fill="transparent" animationDuration=".5s"
            aria-label="Custom ProgressSpinner" />
    </div>
</template>


<style scoped></style>