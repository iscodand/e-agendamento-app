<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';
import { debounce } from 'lodash';

const props = defineProps<{ searchTerm: string, searchMethod: () => void }>();

const emit = defineEmits<{
    (e: 'update:searchTerm', value: string): void;
}>();

const updateSearchTerm = (event: Event) => {
    const target = event.target as HTMLInputElement;
    const value = target.value;
    emit('update:searchTerm', value);
    onInput();
};

const onInput = debounce(() => {
    props.searchMethod();
}, 500);

</script>

<template>
    <IconField>
        <div class="flex gap-3">
            <InputIcon class="pi pi-search" />
            <InputText :value="props.searchTerm" @input="updateSearchTerm" placeholder="Buscar usuário" />
        </div>
    </IconField>
</template>