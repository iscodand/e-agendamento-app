import http from '../api'
import type { APIResponse } from '../types'
import type { Company, InputCreateCompany } from './types'

async function getCompanies(token: string) {
    return await http.get<APIResponse<Company[]>>('companies/', {
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

export default {
    getCompanies,
    createCompany
}