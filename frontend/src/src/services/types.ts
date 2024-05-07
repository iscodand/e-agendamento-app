export type APIResponse<T> = {
    succeeded: boolean,
    data: T,
    status?: number
};