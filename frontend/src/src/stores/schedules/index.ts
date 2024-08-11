import { API } from '@/services'
import type { InputCreateSchedule, Schedule } from '@/services/schedules/types'
import type { APIResponse } from '@/services/types'
import type { AxiosError } from 'axios'
import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useScheduleStore = defineStore('scheduleStore', () => {
  const schedules = ref<Schedule[]>([])

  function initSchedules(data: Schedule[]) {
    schedules.value = data
  }

  function getToken() {
    return localStorage.getItem('token') ?? '';
  }

  async function dispatchGetSchedules(): Promise<APIResponse<Schedule[]>> {
    try {
        const { status, data } = await API.schedules.getMySchedules(getToken());
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

  async function dispatchCreateSchedule(request: InputCreateSchedule) : Promise<APIResponse<Schedule>> {
      try {
        const {status, data} = await API.schedules.newSchedule(request, getToken());

        if (status === 201) {
          return {
            data: data.data,
            status: status,
            succeeded: true
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

  return {
    dispatchGetSchedules,
    dispatchCreateSchedule
  }

})
