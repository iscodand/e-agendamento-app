<script setup lang="ts">

import RouteParamsRaw from 'vue-router';

defineProps({
  link: {
    type: String,
    default: ''
  },
  title: {
    type: String
  },
  icon: {},
  parameters: {
    // type: undefined,
    required: false
  }
});
</script>

<template>
  <div>
    <li>
      <router-link :to="link"
        class="flex items-center p-2 text-gray-900 rounded-lg text-white hover:bg-gray-100 hover:bg-gray-700 group">

        <component :is="icon"
          class="flex-shrink-0 w-5 h-5 text-gray-500 transition duration-75 text-gray-400 group-hover:text-gray-900 group-hover:text-white" />

        <span class="flex-1 ms-3 whitespace-nowrap">{{ title }}</span>

      </router-link>
    </li>
  </div>
</template>