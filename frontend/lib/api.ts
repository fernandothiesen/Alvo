    const API_URL = process.env.NEXT_PUBLIC_API_URL

    export interface ApiResponseResult<T = unknown> {
    Success: boolean
    Message: string
    Data?: T
    }

    export class ApiError extends Error {
    status: number
    constructor(message: string, status: number) {
        super(message)
        this.status = status
    }
    }

    export async function apiFetch<T>(path: string, options: RequestInit = {}): Promise<T> {
    if (!API_URL) {
        throw new Error('API n\u00E3o configurada. Defina NEXT_PUBLIC_API_URL.')
    }

    const token = typeof window !== 'undefined' ? sessionStorage.getItem('token') : null

    const response = await fetch(`${API_URL}${path}`, {
        ...options,
        headers: {
        'Content-Type': 'application/json',
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
        ...options.headers,
        },
    })

    if (response.status === 401) {
        if (typeof window !== 'undefined') {
        sessionStorage.removeItem('token')
        window.location.href = '/'
        }
        throw new ApiError('Sess\u00E3o expirada. Fa\u00E7a login novamente.', 401)
    }

    const result: ApiResponseResult<T> = await response.json()

    if (!response.ok || !result.Success) {
        throw new ApiError(result.Message || 'Erro ao processar solicita\u00E7\u00E3o.', response.status)
    }

    return result.Data as T
    }