import { API } from "@/services";
import type { Company, InputCreateCompany, InputUpdateCompany } from "@/services/companies/types";
import type { APIResponse, RequestParameters } from "@/services/types";
import type { User } from "@/services/user/types";
import type { AxiosError } from "axios";
import { defineStore } from "pinia";
import { ref } from "vue";

export const useCompanyStore = defineStore("companyStore", () => {
    const companies = ref<Company[]>([]);
    const employees = ref<User[]>([]);

    const token = ref<string>();

    function initCompanies(data: Company[]) {
        companies.value = data;
    }

    function initEmployees(data: User[]) {
        employees.value = data;
    }

    function addNewCompany(company: Company) {
        if (companies.value.length < 10)
            companies.value.push(company);
    }

    function getToken(): string {
        token.value = localStorage.getItem('token') || ''
        return token.value;
    } 

    async function dispatchGetCompanies(parameters: RequestParameters): Promise<APIResponse<Company[]>> {
        try {
            const { status, data } = await API.companies.getCompanies(getToken(), parameters.pageSize, parameters.pageNumber);
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
    
    async function dispatchGetCompanyById(companyId: string): Promise<APIResponse<Company>> {
        try {
            const { status, data } = await API.companies.getCompanyById(companyId, localStorage.getItem('token') || '');
            if (status === 200 && data.data) {
                return {
                    succeeded: true,
                    data: data.data,
                    status: status,
                    totalItems: data.totalItems
                };
            }

        // TODO => arrumar essa mensagem de erro (not found???)
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

    async function dispatchUpdateCompany(companyId: string, updatedCompany: InputUpdateCompany): Promise<APIResponse<Company>> {
        try {
            const { status, data} = await API.companies.updateCompany(companyId, updatedCompany, getToken())

            if (status === 200) {
                const index = companies.value.findIndex(company => company.id === companyId);
                companies.value[index] = data.data!;
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
                errors: _error.response?.data.Errors
            };
        }

        return {
            succeeded: false,
            status: 400,
            data: undefined
        };
    } 

    async function dispatchSearchByEmployeeOnCompany(companyId: string, searchTerm: string): Promise<APIResponse<User[]>> {
        try {
            const {status, data} = await API.companies.searchByEmployeeInCompany(companyId, searchTerm, getToken());

            if (status === 200) {
                initEmployees(data.data!);

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
                errors: _error.response?.data.Errors
            };
        }

        return {
            succeeded: false,
            status: 400,
            data: undefined
        };
    }

    async function dispatchAddUserToCompany(companyId: string, userId: string): Promise<APIResponse<string>> {
        try {
            const { status } = await API.companies.addEmployeeToCompany(companyId, userId, getToken());
        
            if (status === 200) {
    
                return {
                    succeeded: true,
                    status: 200,
                    data: ""
                }
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
            data: "Erro ao proceder solicitação."
        }
    }

    return {
        dispatchGetCompanies,
        dispatchSearchByEmployeeOnCompany,
        dispatchGetCompanyById,
        dispatchCreateCompany,
        dispatchUpdateCompany,
        dispatchAddUserToCompany,
    }
})