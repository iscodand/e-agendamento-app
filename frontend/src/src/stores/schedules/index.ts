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

  function addSchedule(schedule: Schedule) {
    schedules.value.push(schedule);
  }

  function removeSchedule(scheduleId: string) {
    const idx = schedules.value.findIndex(i => i.id === scheduleId);
    if (idx === -1) return;
    schedules.value.splice(idx, 1);
  }

  function getToken() {
    return localStorage.getItem('token') ?? '';
  }

  async function dispatchGetSchedules(scheduleStatus: string): Promise<APIResponse<Schedule[]>> {
    try {
      const { status, data } = await API.schedules.getMySchedules(getToken(), scheduleStatus);
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

  async function dispatchGetSchedulesByCompany(companyId: string): Promise<APIResponse<Schedule[]>> {
    try {
      const { status, data } = await API.schedules.getSchedulesByCompany(companyId, getToken());
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

  async function dispatchCreateSchedule(request: InputCreateSchedule): Promise<APIResponse<Schedule>> {
    try {
      const { status, data } = await API.schedules.newSchedule(request, getToken());

      if (status === 201) {
        addSchedule(data.data!);

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

  async function dispatchCancelSchedule(scheduleId: string): Promise<APIResponse<Schedule>> {
    console.log(getToken())

    try {
      const { status, data } = await API.schedules.cancelSchedule(scheduleId, getToken());

      if (status === 200) {
        const index = schedules.value.findIndex(schedule => schedule.id === scheduleId);

        if (index !== -1) {
          schedules.value[index] = data.data!;

          return {
            succeeded: true,
            data: data.data
          }
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

  async function dispatchDeleteSchedule(scheduleId: string): Promise<APIResponse<string>> {
    try {
      const { status, data } = await API.schedules.deleteSchedule(scheduleId, getToken());

      if (status === 204) {
        removeSchedule(scheduleId);

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
    schedules,
    dispatchGetSchedules,
    dispatchGetSchedulesByCompany,
    dispatchCancelSchedule,
    dispatchCreateSchedule,
    dispatchDeleteSchedule
  }
})
