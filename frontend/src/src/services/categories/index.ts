import http from '../api'
import type { APIResponse } from '../types'
import type { Category, InputCreateCategory, InputUpdateCategory } from './types'

async function getCategories() {
    return await http.get<APIResponse<Category[]>>("categories/", {
        headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token')
        }
    });
}

async function createCategory(input: InputCreateCategory) {
    return await http.post<APIResponse<Category>>("categories/", input, {
        headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token')
        }
    })
}

async function updateCategory(id: string, input: InputUpdateCategory) {
    return await http.put<APIResponse<Category>>(`categories/${id}`, input, {
        headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token')
        }
    })
}

async function deleteCategory(id: string) {
    return await http.delete<APIResponse<string>>(`categories/${id}`, {
        headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token')
        }
    })
}

export default {
    getCategories,
    createCategory,
    updateCategory,
    deleteCategory
}
