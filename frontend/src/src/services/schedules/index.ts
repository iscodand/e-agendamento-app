import http from '../api'
import { type APIResponse } from '../types'
import type { InputCreateSchedule, Schedule } from './types'


async function getMySchedules(token: string, status: string = "all") {
    return await http.get<APIResponse<Schedule[]>>('schedules/me', {
        headers: {
            Authorization: 'Bearer ' + token
        },
        params: {
            status: status
        }
    })
}

async function getSchedulesByCompany(companyId: string, token: string) {
    return await http.get<APIResponse<Schedule[]>>(`schedules/${companyId}`, {
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

async function cancelSchedule(scheduleId: string, token: string) {
    return await http.put<APIResponse<Schedule>>(`schedules/${scheduleId}/cancel`, null, {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

async function deleteSchedule(scheduleId: string, token: string) {
    return await http.delete<APIResponse<string>>(`schedules/${scheduleId}`, {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

export default {
    getMySchedules,
    getSchedulesByCompany,
    cancelSchedule,
    newSchedule,
    deleteSchedule
}
