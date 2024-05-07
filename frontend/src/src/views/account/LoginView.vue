<script setup lang="ts"></script>

<template>
  <section class="h-screen flex justify-center items-center">
    <form class="flex flex-col gap-4 card p-16 bg-white rounded-3xl">
      <div class="mb-4">
        <span class="flex ms-2">
          <img
            src="https://flowbite.com/docs/images/logo.svg"
            class="h-8 me-3"
            alt="FlowBite Logo"
          />
          <span
            class="self-center text-xl font-semibold sm:text-2xl whitespace-nowrap dark:text-gray-800"
            >E-Agendamento</span
          >
        </span>
      </div>

      <div>
        <label>
          <span
            class="after:content-['*'] after:ml-0.5 after:text-red-500 block text-sm font-medium text-slate-700"
          >
            Username
          </span>
          <input
            type="text"
            name="username"
            required
            class="mt-1 px-3 py-2 bg-white border shadow-sm border-slate-300 placeholder-slate-400 focus:outline-none focus:border-sky-500 focus:ring-sky-500 block w-full rounded-md sm:text-sm focus:ring-1"
            placeholder="Nome de Usuário"
          />
        </label>
      </div>

      <div>
        <label>
          <span
            class="after:content-['*'] after:ml-0.5 after:text-red-500 block text-sm font-medium text-slate-700"
          >
            Senha
          </span>
          <input
            type="password"
            name="password"
            required
            class="mt-1 px-3 py-2 bg-white border shadow-sm border-slate-300 placeholder-slate-400 focus:outline-none focus:border-sky-500 focus:ring-sky-500 block w-full rounded-md sm:text-sm focus:ring-1"
            placeholder="********"
          />
        </label>
      </div>

      <partial name="../Shared/Partials/_Messages.cshtml" />

      <a href="/teste" class="text-sm text-end dark:text-gray-800 hover:text-gray-700"
        >Esqueceu sua senha?</a
      >

      <div class="text-center">
        <button
          class="dark:bg-gray-800 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded-full"
          type="submit"
        >
          Login
        </button>
      </div>
    </form>
  </section>
</template>

<style scoped></style>
