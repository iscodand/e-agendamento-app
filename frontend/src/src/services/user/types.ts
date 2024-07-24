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