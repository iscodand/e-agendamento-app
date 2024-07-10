import http from '../api'
import type { APIResponse } from '../types'
import type { Company } from './types'

async function getCompanies() {
    return await http.get<APIResponse<Company[]>>('companies/', {
        headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token')
        }
    })
}

export default {
    getCompanies
}