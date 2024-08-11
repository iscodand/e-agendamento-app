import http from '../api'
import { type APIResponse } from '../types'
import type { InputCreateSchedule, Schedule } from './types'


async function getMySchedules(token: string) {
    return await http.get<APIResponse<Schedule[]>>('schedules/me', {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

async function newSchedule(request: InputCreateSchedule, token: string) {
    return await http.post<APIResponse<Schedule>>('schedules/', request, {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

export default {
    getMySchedules,
    newSchedule
}
