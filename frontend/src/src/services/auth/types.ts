export type Login = {
    username: string,
    password: string
}

export type LoginResponse = {
    id: string,
    userName: string,
    email: string,
    roles: string[],
    companies: string[],
    accessToken: string
}

export type VerifyToken = {
    token: string
}