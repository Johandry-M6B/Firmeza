import apiClient from './api';

export const productsService = {
    async getAll(onlyActive = true) {
        const response = await apiClient.get('/products', {
            params: { onlyActive },
        });
        return response.data;
    },

    async getById(id) {
        const response = await apiClient.get(`/products/${id}`);
        return response.data;
    },
};
