import http from '../api'
import type { APIResponse } from '../types'
import type { User } from '../user/types'
import type { Company, InputCreateCompany } from './types'

async function getCompanies(token: string, pageSize: number, pageNumber: number) {
    return await http.get<APIResponse<Company[]>>(`companies?pageSize=${pageSize}&pageNumber=${pageNumber}`, {
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

async function getEmployeesByCompany(companyId: string, token: string) {
    return await http.get<APIResponse<User[]>>(`companies/${companyId}/employees`, {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

export default {
    getCompanies,
    getEmployeesByCompany,
    createCompany
}