export type APIResponse<T> = {
    succeeded: boolean,
    data?: T,
    status?: number,
    pageNumber?: number,
    pageSize?: number,
    message?: string,
    errors?: string[]
};