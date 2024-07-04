export type User = {
    username: string,
    email: string,
    roles: string[],
    company: string
}

export type ApplicationUser = {
    id: string,
    username: string,
    fullName: string
}