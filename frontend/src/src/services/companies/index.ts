import http from '../api'
import type { APIResponse } from '../types'
import type { User } from '../user/types'
import type { Company, InputCreateCompany, InputUpdateCompany } from './types'

async function getCompanies(token: string, pageSize: number, pageNumber: number) {
    return await http.get<APIResponse<Company[]>>(`companies?pageSize=${pageSize}&pageNumber=${pageNumber}`, {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

async function getCompanyById(companyId: string, token: string) {
    return await http.get<APIResponse<Company>>(`companies/${companyId}`, {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

async function createCompany(input: InputCreateCompany, token: string) {
    return await http.post<APIResponse<Company>>('companies', input, {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

async function updateCompany(companyId: string, input: InputUpdateCompany, token: string) {
    return await http.put<APIResponse<Company>>(`companies/${companyId}`, input, {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

async function getEmployeesByCompany(companyId: string, token: string) {
    return await http.get<APIResponse<User[]>>(`companies/${companyId}/employees`, {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

async function searchByEmployeeInCompany(companyId: string, searchTerm: string, token: string) {
    return await http.get<APIResponse<User[]>>(`companies/${companyId}/employees/search?search=${searchTerm}`, {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

async function addEmployeeToCompany(companyId: string, userId: string, token: string) {
    return await http.post<APIResponse<string>>(`companies/${companyId}/add-user`, {"userId": userId}, {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

export default {
    getCompanies,
    getCompanyById,
    getEmployeesByCompany,
    searchByEmployeeInCompany,
    addEmployeeToCompany,
    createCompany,
    updateCompany
}