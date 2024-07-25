import { API } from "@/services";
import type { Company, InputCreateCompany } from "@/services/companies/types";
import type { APIResponse, RequestParameters } from "@/services/types";
import type { User } from "@/services/user/types";
import type { AxiosError } from "axios";
import { defineStore } from "pinia";
import { ref } from "vue";

export const useCompanyStore = defineStore("companyStore", () => {
    const companies = ref<Company[]>([]);
    
    // TODO => separar employees de companies ???
    const employees = ref<User[]>([]);

    function initCompanies(data: Company[]) {
        companies.value = data;
    }

    function addNewCompany(company: Company) {
        console.log(companies.value.length)
        if (companies.value.length < 10)
            companies.value.push(company);
    }

    async function dispatchGetCompanies(parameters: RequestParameters): Promise<APIResponse<Company[]>> {
        try {
            const { status, data } = await API.companies.getCompanies(localStorage.getItem('token') || '', parameters.pageSize, parameters.pageNumber);
            if (status === 200 && data.data) {
                initCompanies(data.data);

                return {
                    succeeded: true,
                    data: data.data,
                    status: status,
                    totalItems: data.totalItems
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

    // TODO => separar employees de companies ???
    async function dispatchGetEmployeesByCompany(companyId: string): Promise<APIResponse<User[]>> {
        try {
            const {status, data} = await API.companies.getEmployeesByCompany(companyId, localStorage.getItem('token') || '');

            if (status === 200) {
                employees.value = data.data!;
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
        dispatchCreateCompany,
        dispatchGetEmployeesByCompany
    }
})