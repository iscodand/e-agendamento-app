export type User = {
    userName: string,
    fullName: string,
    email: string,
    isActive: boolean
    roles: string[],
    company: string
}

export type ApplicationUser = {
    id: string,
    userName: string,
    fullName: string
}

export type InputCreateUser = {
    userName: string,
    fullName: string,
    phoneNumber: string,
    email: string,
    roles: string[],
    companyId: string,
    password: string,
    confirmPassword: string
}