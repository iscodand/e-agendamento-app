export type Company = {
    id: string,
    name: string,
    cnpj: string,
    description: string,
    isActive: boolean
}

export type InputCreateCompany = {
    name: string,
    cnpj: string,
    description: string
}