import { API } from '@/services'
import type { Schedule } from '@/services/schedules/types'
import type { APIResponse } from '@/services/types'
import type { AxiosError } from 'axios'
import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useScheduleStore = defineStore('scheduleStore', () => {
  const schedules = ref<Schedule[]>([])

  function initSchedules(data: Schedule[]) {
    schedules.value = data
  }

  async function dispatchGetSchedules(): Promise<APIResponse<Schedule[]>> {
    try {
        const { status, data } = await API.schedules.getMySchedules();
        if (status === 200 && data.data) {
            initSchedules(data.data);

            return {
                succeeded: true,
                data: data.data,
                status: status,
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

  return {
    dispatchGetSchedules
  }

})
