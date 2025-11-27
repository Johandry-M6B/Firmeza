import apiClient from './api';

export const salesService = {
    async create(saleData) {
        const response = await apiClient.post('/sales', saleData);
        return response.data;
    },

    async getMyOrders() {
        const response = await apiClient.get('/sales');
        return response.data;
    },
};
