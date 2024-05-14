import { defineStore } from "pinia";
import { ref } from "vue";
import { type Item } from "@/services/items/types";
import type { APIResponse } from "@/services/types";
import { API } from "@/services";
import type { AxiosError } from "axios";

export const useItemStore = defineStore("itemStore", () => {
    const items = ref<Item[]>([]);

    function initItems(data: Item[]) {
        items.value = data;
    }

    async function dispatchGetItems(): Promise<APIResponse<Item[]>> {
        const { status, data } = await API.items.getItems();

        return {
            succeeded: true,
            data: data.data,
            status: status,
        };
    }

    return {
        items,
        initItems,
        dispatchGetItems
    };
});
