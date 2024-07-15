export type Company = {
    id: String,
    name: String,
    cnpj: String,
    description: String,
    isActive: boolean
}

export type InputCreateCompany = {
    name: String,
    cnpj: String,
    description: String
}