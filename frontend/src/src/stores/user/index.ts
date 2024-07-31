import { API } from "@/services";
import type { APIResponse } from "@/services/types";
import type { InputCreateUser, User } from "@/services/user/types";
import type { AxiosError } from "axios";
import { defineStore } from "pinia";
import { ref } from "vue";

export const useUserStore = defineStore('userStore', () => {
    const users = ref<User[]>([]);

    function initUsers(data: User[]) {
        users.value = data;
    }

    function addUser(data: User){
        users.value.push(data);
    }

    const token = ref<string>("");
    function getToken(): string {
        token.value = localStorage.getItem('token') || ''
        return token.value;
    } 

    async function dispatchGetEmployeesByCompany(companyId: string): Promise<APIResponse<User[]>> {
        try {
            const {status, data} = await API.companies.getEmployeesByCompany(companyId, localStorage.getItem('token') || '');

            if (status === 200) {
                initUsers(data.data!);
                return {
                    succeeded: true,
                    data: data.data,
                    status: status
                }
            }
        } catch (error) {
            const _error = error as AxiosError<{ Message: string, Errors?: string[] }>;

            return {
                succeeded: false,
                status: _error.response?.status,
                // data: null,
                errors: _error.response?.data.Errors
            };
        }
        
        return {
            succeeded: false,
            status: 400,
            data: undefined
        };
    } 

    async function dispatchCreateUser(request: InputCreateUser): Promise<APIResponse<User>> {
        try {
            const {data, status} = await API.users.registerNewUser(request, getToken());

            if (status === 201) {
                addUser(data.data!);

                return {
                    succeeded: true,
                    data: data.data,
                    status: status,
                    // totalItems: data.totalItems
                };
            }
        } catch (error) {
            const _error = error as AxiosError<{ Message: string, Errors?: string[] }>;

            return {
                succeeded: false,
                status: _error.response?.status,
                errors: _error.response?.data.Errors
            };
        }
    
        return {
            succeeded: false,
            status: 400,
            data: undefined
        };
    }
    
    return {
        dispatchGetEmployeesByCompany,
        dispatchCreateUser
    }
})