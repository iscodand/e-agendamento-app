import { defineStore } from "pinia";
import { ref } from "vue";
import { type InputCreateItem, type Item } from "@/services/items/types";
import type { APIResponse } from "@/services/types";
import { API } from "@/services";
import type { Axios, AxiosError } from "axios";

export const useItemStore = defineStore("itemStore", () => {
    const items = ref<Item[]>([]);

    function initItems(data: Item[]) {
        items.value = data;
    }

    function addNewItem(item: any) {
        items.value.push(item);
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

    async function dispatchCreateItem(
        input: InputCreateItem
    ): Promise<APIResponse<null>> {
        try {
            const { data, status } = await API.items.createItem(input);
            if (status === 200) {
                addNewItem(data.data);
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
                data: null
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
        dispatchCreateItem,
        dispatchGetItems
    };
});
