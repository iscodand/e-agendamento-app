export type User = {
    username: string,
    fullName: string,
    email: string,
    isActive: boolean
    roles: string[],
    company: string
}

export type ApplicationUser = {
    id: string,
    username: string,
    fullName: string
}