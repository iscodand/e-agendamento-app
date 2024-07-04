<script lang="ts" setup>
import { onMounted, ref, defineProps, defineExpose } from 'vue';

// Definir as propriedades
const props = defineProps({
    placeholder: {
        type: String,
        required: false,
        default: ''
    },
    type: {
        type: String,
        default: 'text'
    },
    required: {
        type: Boolean,
        default: false
    }
});

const model = defineModel({
    type: String,
    required: true
});

const input = ref(null);

onMounted(() => {
    if (input.value.hasAttribute('autofocus')) {
        input.value.focus();
    }
});

defineExpose({ focus: () => input.value.focus() });
</script>

<template>
    <div>
        <input
            class="mt-1 px-3 py-2 bg-white border shadow-sm border-slate-300 placeholder-slate-400 focus:outline-none focus:border-sky-500 focus:ring-sky-500 block w-full rounded-md sm:text-sm focus:ring-1"
            v-model="model" :placeholder="placeholder" :type="type" :required="required" ref="input" />
    </div>
</template>