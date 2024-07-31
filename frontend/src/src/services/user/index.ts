import type { APIResponse } from "../types";
import type { InputCreateUser, User } from "./types";
import http from '../api'

async function registerNewUser(request: InputCreateUser, token: string) {
    return await http.post<APIResponse<User>>('account/register', request, {
        headers: {
            Authorization: 'Bearer ' + token
        }
    })
}

export default {
    registerNewUser
}