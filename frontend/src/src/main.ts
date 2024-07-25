import './assets/main.css'
import 'primeicons/primeicons.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import './index.css'
import VueTheMask from 'vue-the-mask'

import PrimeVue from 'primevue/config';
import Aura from '@primevue/themes/aura';
import ToastService from 'primevue/toastservice';

import Vue3Lottie from "vue3-lottie";
// import "vue3-lottie/dist/style.css";

const app = createApp(App)

app.use(PrimeVue, {
    theme: {
        preset: Aura
    }
});

app.use(ToastService);
app.use(createPinia())
app.use(Vue3Lottie);
app.use(router)
app.use(VueTheMask)

app.mount('#app')
