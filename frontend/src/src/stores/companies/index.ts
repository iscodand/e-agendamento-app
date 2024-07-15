import { API } from "@/services";
import type { Company, InputCreateCompany } from "@/services/companies/types";
import type { APIResponse } from "@/services/types";
import type { AxiosError } from "axios";
import { defineStore } from "pinia";
import { ref } from "vue";

export const useCompanyStore = defineStore("companyStore", () => {
    const companies = ref<Company[]>([]);

    function initCompanies(data: Company[]) {
        companies.value = data;
    }

    function addNewCompany(company: Company) {
        companies.value.push(company);
    }

    async function dispatchGetCompanies(): Promise<APIResponse<Company[]>> {
        try {
            const { status, data } = await API.companies.getCompanies(localStorage.getItem('token') || '');
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
    
    async function dispatchCreateCompany(input: InputCreateCompany): Promise<APIResponse<Company>> {
        try {
            const {status, data} = await API.companies.createCompany(input, localStorage.getItem('token') || '');
            
            if (status === 201) {
                addNewCompany(data.data!)
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

    return {
        dispatchGetCompanies,
        dispatchCreateCompany
    }
})