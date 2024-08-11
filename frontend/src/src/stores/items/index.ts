import { defineStore } from "pinia";
import { ref } from "vue";
import { type InputCreateItem, type InputUpdateItem, type Item } from "@/services/items/types";
import type { APIResponse } from "@/services/types";
import { API } from "@/services";
import type { Axios, AxiosError } from "axios";

export const useItemStore = defineStore("itemStore", () => {
    const items = ref<Item[]>([]);

    function initItems(data: Item[]) {
        items.value = data;
    }

    function addNewItem(item: Item) {
        items.value.push(item);
    }

    function getToken(): string {
        return localStorage.getItem('token') || '';
    }

    function removeItem(itemId: string) {
        const idx = items.value.findIndex(i => i.id === itemId);
        if (idx === -1) return;
        items.value.splice(idx, 1);
    }

    async function dispatchGetItems(): Promise<APIResponse<Item[]>> {
        try {
            const { status, data } = await API.items.getItems();
            if (status === 200 && data.data) {
                initItems(data.data);

                return {
                    succeeded: true,
                    data: data.data,
                    status: status,
                };
            }
        } catch (error) {
            const _error = error as AxiosError<string>;
            return {
                succeeded: false,
                status: _error.response?.status,
                data: undefined
            };
        }

        return {
            succeeded: false,
            status: 400,
            data: undefined
        };
    }

    async function dispatchSearchItems(searchTerm: string): Promise<APIResponse<Item[]>> {
        try {
            const { status, data } = await API.items.searchByItem(searchTerm, getToken());
            if (status === 200 && data.data) {
                return {
                    succeeded: true,
                    data: data.data,
                    status: status,
                };
            }
        } catch (error) {
            const _error = error as AxiosError<string>;
            return {
                succeeded: false,
                status: _error.response?.status,
                data: undefined
            };
        }

        return {
            succeeded: false,
            status: 400,
            data: undefined
        };
    }

    async function dispatchCreateItem(
        input: InputCreateItem
    ): Promise<APIResponse<null>> {
        try {
            const { data, status } = await API.items.createItem(input);
            if (status === 200) {
                addNewItem(data.data!);
                return {
                    succeeded: true,
                    data: null
                }
            }
        } catch (error) {
            const _error = error as AxiosError<{ Message: string, Errors?: string[] }>;

            return {
                succeeded: false,
                status: _error.response?.status,
                data: null,
                errors: _error.response?.data.Errors
            };
        }

        return {
            succeeded: false,
            data: null,
            status: 400
        }
    }

    async function dispatchUpdateItem(
        updatedItem: InputUpdateItem
    ): Promise<APIResponse<null>> {
        try {
            const { data, status } = await API.items.updateItem(updatedItem.id!, updatedItem);
            if (status === 200) {
                const index = items.value.findIndex(item => item.id === updatedItem.id);

                if (index !== -1) {
                    items.value[index] = data.data!;

                    return {
                        succeeded: true,
                        data: null
                    }
                }
            }
        } catch (error) {
            const _error = error as AxiosError<{ Message: string, Errors?: string[] }>;

            return {
                succeeded: false,
                status: _error.response?.status,
                data: null,
                errors: _error.response?.data.Errors
            };
        }

        return {
            succeeded: false,
            data: null,
        };
    }


    async function dispatchDeleteItem(
        itemId: string
    ): Promise<APIResponse<null>> {
        try {
            const { data, status } = await API.items.deleteItem(itemId);

            if (status === 200) {
                removeItem(itemId);
                return {
                    succeeded: true,
                    data: null
                }
            }
        } catch (error) {
            const _error = error as AxiosError<string>;
            return {
                succeeded: false,
                status: _error.response?.status,
                data: null,
                errors: [_error.message]
            }
        }

        return {
            succeeded: false,
            data: null,
            status: 400
        }
    }

    return {
        items,
        initItems,
        dispatchGetItems,
        dispatchCreateItem,
        dispatchUpdateItem,
        dispatchDeleteItem,
        dispatchSearchItems
    };
});
