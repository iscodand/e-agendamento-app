export type Item = {
    name: string,
    description: string,
    categoryId: string,
    totalQuantity: number,
    quantityAvailable: number
}

export type InputCreateItem = {
    name: string,
    description: string,
    categoryId: string,
    totalQuantity: number,
    quantityAvailable: number
};