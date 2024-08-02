import type { Category } from "../categories/types";

export type Item = {
    id: string,
    name: string,
    description: string,
    categoryId: string,
    category?: Category, 
    totalQuantity: number,
    quantityAvailable: number,
    isAvailable: boolean
}

export type InputCreateItem = {
    id?: string,
    name: string,
    description: string,
    categoryId: string,
    totalQuantity: number,
    quantityAvailable: number,
    companyId?: string
};

export type InputUpdateItem = {
    id?: string,
    name: string,
    description: string,
    categoryId: string,
    totalQuantity: number,
    quantityAvailable: number,
    isAvailable: boolean
}