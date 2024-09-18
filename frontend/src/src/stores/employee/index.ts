import { defineStore } from "pinia";
import { ref } from "vue";
import type { Employee } from "@/services/employees/types";
import type { APIResponse } from "@/services/types";
import { API } from "@/services";
import type { AxiosError } from "axios";

export const useEmployeeStore = defineStore("employeeStore", () => {
    const employees = ref<Employee[]>([]);

    function initEmployees(data: Employee[]) {
        employees.value = data;
    }

    function addNewItem(employee: Employee) {
        employees.value.push(employee);
    }

    function getToken(): string {
        return localStorage.getItem('token') || '';
    }

    function removeEmployee(employeeId: string) {
        const idx = employees.value.findIndex(i => i.id === employeeId);
        if (idx === -1) return;
        employees.value.splice(idx, 1);
    }

    async function dispatchGetEmployeeById(id: string): Promise<APIResponse<Employee>> {
        try {
            const { status, data } = await API.employees.detailById(id, getToken());
            if (status === 200 && data.data) {
                return {
                    succeeded: true,
                    data: data.data,
                    status: status,
                };
            }
        } catch (error) {
            const _error = error as AxiosError<string>;

            return {
                succeeded: false,
                status: _error.response?.status,
                data: undefined
            };
        }

        return {
            succeeded: false,
            status: 400,
            data: undefined
        };
    }

    return {
        employees,
        dispatchGetEmployeeById
    };
});
