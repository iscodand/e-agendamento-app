<script lang="ts" setup>
import { ref } from 'vue';
import { authStore } from '@/stores/auth'
import { type Login } from '@/services/auth/types'
import ActionButton from '@/components/ui/buttons/ActionButton.vue'
import TextInputComponent from '@/components/ui/input/TextInputComponent.vue'
import ErrorMessageComponent from '@/components/ui/error/ErrorMessageComponent.vue'
import router from '@/router';

const useAuthStore = authStore();

const credentials = ref({
  username: '',
  password: ''
})

const errorMessages = ref<string[]>([]);

async function handleSubmit() {
  const input: Login = {
    username: credentials.value.username,
    password: credentials.value.password
  };

  const response = await useAuthStore.dispatchAuthenticate(input);

  if (response.succeeded) {
    router.replace('items/');
    router.push('items/')
  } else {
    errorMessages.value = [];
    errorMessages.value.push('Nome de usuário ou senha incorretos.')
  }
}
</script>

<template>
  <div>
    <section class="h-screen flex justify-center items-center">
      <form class="flex flex-col gap-4 card p-16 bg-white rounded-3xl" @submit.prevent="handleSubmit">
        <div class="mb-4">
          <span class="flex ms-2">
            <img src="https://flowbite.com/docs/images/logo.svg" class="h-8 me-3" alt="FlowBite Logo" />
            <span
              class="self-center text-xl font-semibold sm:text-2xl whitespace-nowrap dark:text-gray-800">E-Agendamento</span>
          </span>
        </div>

        <div>
          <label>
            <span class="after:content-['*'] after:ml-0.5 after:text-red-500 block text-sm font-medium text-slate-700">
              Username
            </span>
            <TextInputComponent required placeholder="Nome de Usuário" v-model="credentials.username" />
          </label>
        </div>

        <div>
          <label>
            <span class="after:content-['*'] after:ml-0.5 after:text-red-500 block text-sm font-medium text-slate-700">
              Senha
            </span>
            <TextInputComponent required type="password" placeholder="Senha" v-model="credentials.password" />
          </label>
        </div>

        <ErrorMessageComponent :messages="errorMessages" />

        <a href="/teste" class="text-sm text-end dark:text-gray-800 hover:text-gray-700">Esqueceu sua senha?</a>

        <div class="text-center">
          <ActionButton type="submit" color="blue">Login</ActionButton>
        </div>
      </form>
    </section>
  </div>
</template>

<style scoped></style>