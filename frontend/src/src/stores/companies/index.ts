import { API } from "@/services";
import type { Company } from "@/services/companies/types";
import type { APIResponse } from "@/services/types";
import type { AxiosError } from "axios";
import { defineStore } from "pinia";
import { ref } from "vue";

export const useCompanyStore = defineStore("companyStore", () => {
    const companies = ref<Company[]>([]);

    function initCompanies(data: Company[]) {
        companies.value = data;
    }

    async function dispatchGetCompanies(): Promise<APIResponse<Company[]>> {
        
        
        try {
            const { status, data } = await API.companies.getCompanies();
            if (status === 200 && data.data) {
                initCompanies(data.data);

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
        dispatchGetCompanies
    }
})