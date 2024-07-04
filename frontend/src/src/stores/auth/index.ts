import router from "@/router";
import { API } from "@/services";
import type { Login, LoginResponse, VerifyToken } from "@/services/auth/types";
import type { APIResponse } from "@/services/types";
import type { AxiosError } from "axios";
import { defineStore } from "pinia";
import { computed, ref, type Ref } from "vue";

export const authStore = defineStore("authStore", () => {
    const token = ref(localStorage.getItem('token'));
    const storedUser = localStorage.getItem('user');
    const company = localStorage.getItem('company');
    const user = ref(storedUser ? JSON.parse(storedUser) : {});

    const isAuth: Ref<boolean> = ref(false);

    async function dispatchAuthenticate(input: Login): Promise<APIResponse<LoginResponse>> {
        try {
            const { status, data } = await API.auth.authenticate(input);

            if (status === 200) {
                setToken(data.data!.accessToken)
                setUser(data.data!.userName)
                setCompany(data.data!.companies[0])
            }

            return {
                succeeded: true,
                data: data.data,
                status: status
            }
        } catch (error) {
            const _error = error as AxiosError<string>;
            return {
                succeeded: false,
                status: _error.response?.status,
                data: undefined
            };
        }
    }

    async function dispatchVerifyToken(): Promise<APIResponse<string>> {
        try {
            const { status, data } = await API.auth.verifyToken(token.value!);

            return {
                succeeded: true,
                data: data.data,
                status: status
            }
        } catch (error) {
            const _error = error as AxiosError<string>;

            clear();
            router.push('/login')
            console.log('error', _error.response?.data)

            return {
                succeeded: false,
                status: _error.response?.status,
                data: undefined
            };
        }
    }

    function setIsAuthenticated() {
        isAuth.value = true;
    }

    function setToken(tokenValue: string): void {
        localStorage.setItem('token', tokenValue);
        token.value = tokenValue;
    }

    function setUser(userValue: string): void {
        localStorage.setItem('user', JSON.stringify(userValue));
        user.value = userValue;
    }

    function setCompany(companyId: string): void {
        localStorage.setItem('company', JSON.stringify(companyId));
    }

    const isAuthenticated = computed(async () => {
        return (await dispatchVerifyToken()).succeeded;
    })

    const userName = computed(() => {
        if (user.value) {
            return user.value;
        }
        return '';
    })

    const getCompany = computed(() => {

    })

    function clear() {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        isAuth.value = false;
        token.value = '';
        user.value = '';
    }

    return {
        token,
        user,
        isAuth,
        dispatchAuthenticate,
        dispatchVerifyToken,
        setIsAuthenticated,
        isAuthenticated,
        userName,
        clear,
        setToken,
        setUser
    }
})