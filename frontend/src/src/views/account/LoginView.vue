<script lang="ts" setup>
import { ref } from 'vue';
import { authStore } from '@/stores/auth'
import { type Login } from '@/services/auth/types'
import ErrorMessageComponent from '@/components/ui/error/ErrorMessageComponent.vue'
import router from '@/router';

//
import Password from 'primevue/password';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import Toast from 'primevue/toast';
import { useToast } from 'primevue/usetoast';

const toast = useToast()

toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Logout realizado com sucesso.', life: 3000 });


const useAuthStore = authStore();

const credentials = ref({
  username: '',
  password: ''
})

const errorMessages = ref<string[]>([]);

async function handleSubmit() {
  errorMessages.value = [];

  const input: Login = {
    username: credentials.value.username,
    password: credentials.value.password
  };

  const response = await useAuthStore.dispatchAuthenticate(input);

  if (response.succeeded) {
    router.push({ name: "home" })
  } else {
    errorMessages.value.push('Nome de usuário ou senha incorretos.')
  }
}
</script>

<template>
  <div>
    <Toast />
    <section class="h-screen flex justify-center items-center">
      <form class="flex flex-col gap-4 card p-16 bg-white rounded-3xl" @submit.prevent="handleSubmit">
        <span class="flex mb-4 self-center text-xl font-semibold sm:text-2xl whitespace-nowrap text-gray-800">
          <img src="https://flowbite.com/docs/images/logo.svg" class="h-8 me-3" alt="FlowBite Logo" />
          E-Agendamento
        </span>

        <div class="flex flex-col gap-2 mb-2">
          <label for="password" class="block text-sm font-medium text-gray-700">
            Usuário
          </label>
          <InputText v-model="credentials.username" required />
        </div>

        <div class="flex flex-col gap-2">
          <label for="password" class="block text-sm font-medium text-gray-700">
            Senha
          </label>
          <Password v-model="credentials.password" toggleMask :feedback="false" />
        </div>

        <ErrorMessageComponent :messages="errorMessages" />

        <a href="/teste" class="text-sm text-end text-gray-800 hover:text-gray-700">Esqueceu sua senha?</a>

        <div class="text-center">
          <Button type="submit" label="Entrar" icon="pi pi-sign-in" />
        </div>
      </form>
    </section>
  </div>
</template>

<style scoped></style>