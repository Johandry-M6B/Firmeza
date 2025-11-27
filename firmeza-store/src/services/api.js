import axios from 'axios';

const API_URL = 'http://localhost:5235/api';

const apiClient = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

// Add token to requests
apiClient.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

// Handle 401 errors
apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response?.status === 401) {
            // Clear the invalid token
            localStorage.removeItem('token');
            // Don't redirect automatically - let the calling code handle it
            // This allows public pages to load even if API calls fail
        }
        return Promise.reject(error);
    }
);

export default apiClient;
