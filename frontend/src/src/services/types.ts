export type APIResponse<T> = {
    succeeded: boolean,
    data?: T,
    status?: number,
    message?: string,
    errors?: string[],

    // PagedResponse Properties
    pageNumber?: number,
    pageSize?: number,
    totalItems?: number,
};

export type RequestParameters = {
    pageSize: number,
    pageNumber: number
}