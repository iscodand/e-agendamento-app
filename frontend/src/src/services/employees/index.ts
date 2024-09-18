import type { APIResponse } from "../types";
import http from '../api'
import type { Employee } from "./types";

async function detailById(id: string, token: string) {
    return await http.get<APIResponse<Employee>>(`employees/${id}`, {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

export default {
    detailById
}