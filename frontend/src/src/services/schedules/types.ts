import type { Item } from "../items/types"
import type { ApplicationUser } from "../user/types"

export type Schedule = {
    id: string,
    item?: Item,
    itemId: string,
    observation: string,
    requestedBy: ApplicationUser | undefined,
    confirmedBy: ApplicationUser | undefined,
    status: ScheduleStatus,
    startedAt: Date,
    endAt: Date,
    companyId: string
}

export type InputCreateSchedule = {
    item?: Item,
    itemId: string,
    observation: string,
    startAt: Date,
    endAt: Date
}

export enum ScheduleStatus {
    Pending,
    Open,
    Closed,
    Canceled
}