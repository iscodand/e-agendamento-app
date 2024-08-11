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
    name: string,
    description: string,
    categoryId: string, 
    category?: Category | undefined,
    totalQuantity: number,
    quantityAvailable: number
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