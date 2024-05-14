import http from '../api'
import { type APIResponse } from '../types'
import { type Item, type InputCreateItem } from './types'

async function getItems() {
    return await http.get<APIResponse<Item[]>>("items/")
}

export default {
    getItems
}