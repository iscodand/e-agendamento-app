export type Employee = {
    id: string,
    userName: string,
    fullName: string,
    email: string,
    phoneNumber: string,
    // isActive: boolean,
    // alreadyInCompany?: boolean,
    roles: string[],
    companies: string[]
}

export type UpdateEmployeeRequest = {
    id: string,
    userName: string,
    fullName: string,
    email: string,
    phoneNumber: string,
}
