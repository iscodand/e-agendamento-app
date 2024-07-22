import http from '../api'
import { type APIResponse } from '../types'
import type { Schedule } from './types'


async function getMySchedules() {
    return await http.get<APIResponse<Schedule[]>>('schedules/me', {
        headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token')
        }
    })
}

export default {
    getMySchedules
}
