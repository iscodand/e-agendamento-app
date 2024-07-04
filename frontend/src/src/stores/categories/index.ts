import { API } from "@/services";
import type { Category, InputCreateCategory, InputUpdateCategory } from "@/services/categories/types";
import type { APIResponse } from "@/services/types";
import type { AxiosError } from "axios";
import { defineStore } from "pinia";
import { ref } from "vue";

export const useCategoryStore = defineStore("categoryStore", () => {
    const categories = ref<Category[]>([]);

    function initCategories(data: Category[] | undefined) {
        categories.value = data!;
    }

    function addCategory(category: Category) {
        console.log('entrou')
        categories.value.push(category)
    }

    function removeCategory(categoryId: string) {
        const idx = categories.value.findIndex(c => c.id === categoryId);
        if (idx === -1) return;
        categories.value.splice(idx, 1);
    }

    async function dispatchGetCategories(): Promise<APIResponse<Category[]>> {
        const { status, data } = await API.categories.getCategories();
        initCategories(data.data);
        return {
            succeeded: true,
            data: data.data,
            status: status
        }
    }

    async function dispatchCreateCategory(input: InputCreateCategory): Promise<APIResponse<Category | null>> {
        try {
            const { data, status } = await API.categories.createCategory(input);
            if (status === 201) {
                addCategory(data.data!);
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

    async function dispatchUpdateCategory(updatedCategory: InputUpdateCategory): Promise<APIResponse<Category | null>> {
        try {
            const { data, status } = await API.categories.updateCategory(updatedCategory.id!, updatedCategory);
            if (status === 200) {
                const index = categories.value.findIndex(category => category.id === updatedCategory.id);

                if (index !== -1) {
                    categories.value[index] = data.data!;

                    return {
                        succeeded: true,
                        data: null
                    }

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

    async function dispatchDeleteCategory(categoryId: string): Promise<APIResponse<null>> {
        try {
            const { status } = await API.categories.deleteCategory(categoryId);

            if (status == 200) {
                removeCategory(categoryId);
                return {
                    succeeded: true,
                    status: 200,
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
            status: 400
        }
    }

    return {
        dispatchGetCategories,
        dispatchCreateCategory,
        dispatchUpdateCategory,
        dispatchDeleteCategory
    }
})