import axios from 'axios';
import store from '../store';

// Configure the base URL for your local API
const apiClient = axios.create({
    baseURL: 'https://localhost:7026/api',
    headers: {
        'Content-Type': 'application/json',
    },
});

// Attach JWT from Vuex store or localStorage to every non-auth request
apiClient.interceptors.request.use((config) => {
    // don't attach token to auth routes (login/register)
    const url = config.url?.toLowerCase() ?? '';
    if (url.includes('/auth/login') || url.includes('/auth/register')) {
        return config;
    }

    const tokenFromStore = store.state?.token;
    const tokenFromStorage = localStorage.getItem('token');
    const token = tokenFromStore || tokenFromStorage;

    if (token) {
        config.headers = config.headers || {};
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
}, (error) => Promise.reject(error));

export default {
    // GET all products: https://localhost:7026/api/products/
    getProducts() {
        return apiClient.get('/products/');
    },

    // Example GET single product
    getProduct(id) {
        return apiClient.get(`/products/${id}`);
    },

    // Login against MyApp.Api
    login(credentials) {
        return apiClient.post('/auth/login', credentials);
    },

    // Register new user
    register(credentials) {
        return apiClient.post('/auth/register', credentials);
    },

    // expose apiClient if you need it elsewhere
    apiClient
};