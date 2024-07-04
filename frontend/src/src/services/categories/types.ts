export type Category = {
    id: string,
    description: string,
    companyId: string
}

export type InputCreateCategory = {
    id?: string,
    description: string,
    companyId: string
}

export type InputUpdateCategory = {
    id?: string,
    description: string
}