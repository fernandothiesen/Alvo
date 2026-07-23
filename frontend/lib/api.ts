import axios from 'axios';

// Captura a URL da sua API a partir das variáveis de ambiente do projeto
const API_URL = process.env.NEXT_PUBLIC_API_URL;

if (!API_URL) {
    console.warn('API não configurada. Defina a variável de ambiente NEXT_PUBLIC_API_URL.');
}

// Cria a instância principal do Axios
export const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

// Interceptor de Requisição: Injeta o token antes de enviar para o servidor
api.interceptors.request.use(
    (config) => {
        // Como o Next.js pode rodar no servidor (SSR), precisamos garantir 
        // que o sessionStorage só seja acessado no navegador (cliente)
        if (typeof window !== 'undefined') {
            const token = sessionStorage.getItem('token');
            
            if (token && config.headers) {
                config.headers.Authorization = `Bearer ${token}`;
            }
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);


api.interceptors.response.use(
    (response) => {
        
        return response;
    },
    (error) => {
    
        if (error.response && error.response.status === 401) {
            if (typeof window !== 'undefined') {
                console.warn('Sessão expirada ou não autorizada. Limpando token...');
                sessionStorage.removeItem('token');
                
            
                // window.location.href = '/login'; 
            }
        }
        return Promise.reject(error);
    }
);