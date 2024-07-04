import http from '../api'
import { type APIResponse } from '../types'
import { type Login, type LoginResponse, type VerifyToken } from './types'


async function authenticate(input: Login) {
    return await http.post<APIResponse<LoginResponse>>("account/auth", input);
}

async function verifyToken(token: string) {
    return await http.get("account/verify-token", {
        headers: {
            Authorization: 'Bearer ' + token
        }
    });
}

export default {
    authenticate,
    verifyToken
}