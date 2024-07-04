import http from '../api'
import { type APIResponse } from '../types'
import { type Item, type InputCreateItem, type InputUpdateItem } from './types'

async function getItems() {
    return await http.get<APIResponse<Item[]>>("items/", {
        headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token')
        }
    });
}

async function createItem(input: InputCreateItem) {
    return await http.post<APIResponse<Item>>("items/", input, {
        headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token')
        }
    });
}

async function updateItem(itemId: string, input: InputUpdateItem) {
    return await http.put<APIResponse<Item>>(`items/${itemId}`, input, {
        headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token')
        }
    });
}

async function deleteItem(itemId: string) {
    return await http.delete<APIResponse<Item>>(`items/${itemId}`, {
        headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token')
        }
    });
}

export default {
    getItems,
    createItem,
    updateItem,
    deleteItem
}