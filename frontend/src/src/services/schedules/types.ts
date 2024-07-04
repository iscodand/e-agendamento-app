import type { ApplicationUser } from "../user/types"

export type Schedule = {
    id: string,
    observation: string,
    requestedBy: ApplicationUser | undefined,
    confirmedBy: ApplicationUser | undefined,
    status: string,
    startAt: Date,
    endAt: Date,
    companyId: string
}

export type InputCreateSchedule = {
    itemId: string,
    observation: string
}