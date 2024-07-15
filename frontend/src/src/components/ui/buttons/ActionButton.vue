<template>
    <div>
        <button :type="type" :class="buttonClasses">
            <slot />
        </button>
    </div>
</template>

<script setup lang="ts">
import { defineProps, computed } from 'vue';

// Define as propriedades e seus tipos
const props = defineProps({
    color: {
        type: String as () => 'blue' | 'green' | 'light-green' | 'red' | 'yellow' | 'disabled' | 'default',
        default: 'default'
    },
    type: {
        type: String as () => 'button' | 'submit',
        default: 'button'
    }
});

// Computed property para calcular as classes com base na propriedade color
const buttonClasses = computed(() => {
    return {
        'text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 bg-blue-600 hover:bg-blue-700 focus:ring-blue-800': props.color === 'blue',
        'focus:outline-none text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 bg-green-600 hover:bg-green-700 focus:ring-green-800': props.color === 'green',
        'focus:outline-none text-white bg-green-500 hover:bg-green-600 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 bg-green-600 hover:bg-green-700 focus:ring-green-800': props.color === 'light-green',
        'focus:outline-none text-white bg-red-700 hover:bg-red-800 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 bg-red-600 hover:bg-red-700 focus:ring-red-900': props.color === 'red',
        'focus:outline-none text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 focus:ring-yellow-900': props.color === 'yellow',
        'cursor-disabled text-gray-500 bg-gray-200 focus:outline-none font-medium rounded-lg text-sm px-5 py-2.5 mr-2 bg-gray-800 text-white border-gray-600 hover:bg-gray-700 hover:border-gray-600 focus:ring-gray-700': props.color === 'disabled',
        'border border-none focus:outline-none hover:bg-gray-200 focus:ring-4 focus:ring-gray-200 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 bg-gray-800 text-white border-gray-600 hover:bg-gray-700 hover:border-gray-600 focus:ring-gray-700': props.color === 'default',
    };
});
</script>