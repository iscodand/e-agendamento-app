import router from "@/router";
import { API } from "@/services";
import type { Login, LoginResponse, VerifyToken } from "@/services/auth/types";
import type { APIResponse } from "@/services/types";
import type { User } from "@/services/user/types";
import type { AxiosError } from "axios";
import { defineStore } from "pinia";
import { ref, computed, type Ref } from "vue";

export const authStore = defineStore("authStore", () => {
    const token = ref(localStorage.getItem('token'));
    const storedUser = localStorage.getItem('user');
    const user = ref(storedUser ? JSON.parse(storedUser) : {});
    const roles = ref<string[]>();
    const isAuth: Ref<boolean> = ref(false);

    async function dispatchAuthenticate(input: Login): Promise<APIResponse<LoginResponse>> {
        try {
            const { status, data } = await API.auth.authenticate(input);

            if (status === 200) {
                setToken(data.data!.accessToken);
                setUser(data.data!.userName);
                setRoles(data.data!.roles);
                setIsAuthenticated();
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

            setIsAuthenticated(); // Seta o estado de autenticação como verdadeiro

            return {
                succeeded: true,
                data: data.data,
                status: status
            };
        } catch (error) {
            const _error = error as AxiosError<string>;

            clear();
            router.push('/login');

            return {
                succeeded: false,
                status: _error.response?.status,
                data: undefined
            };
        }
    }

    async function dispatchGetAuthenticatedUser(): Promise<APIResponse<User>> {
        try {
            const { status, data } = await API.auth.retrieveAuthenticatedUser(token.value!);

            return {
                succeeded: true,
                data: data.data,
                status: status
            }
        } catch (error) {
            const _error = error as AxiosError<string>;

            clear();
            router.push('/login');

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

    const isAuthenticated = async (): Promise<boolean> => {
        return (await dispatchVerifyToken()).succeeded;
    };

    function setToken(tokenValue: string): void {
        localStorage.setItem('token', tokenValue);
        token.value = tokenValue;
    }

    function setUser(userValue: string): void {
        localStorage.setItem('user', JSON.stringify(userValue));
        user.value = userValue;
    }

    function setRoles(newRoles: string[]): void {
        roles.value = newRoles;
    }

    const userName = computed(() => user.value || '');

    const getRoles = computed(() => roles.value || []);

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
        dispatchGetAuthenticatedUser,
        isAuthenticated,
        userName,
        getRoles,
        setIsAuthenticated,
        clear,
        setToken,
        setUser
    };
});