    const API_URL = process.env.NEXT_PUBLIC_API_URL

    export interface LoginPayload {
    email: string
    password: string
    }

    export interface LoginResponseData {
    token: string
    idUsuario: number
    nome: string
    email: string
    ativo: boolean
    dataCriacao: string
    ultimoLogin: string | null
    roles: string[]
    }

    interface ApiResponseResult<T = unknown> {
    success: boolean
    message: string
    data?: T
    }

    export async function login(payload: LoginPayload): Promise<LoginResponseData> {
    if (!API_URL) {
        throw new Error(
        'API n\u00E3o configurada. Defina a vari\u00E1vel de ambiente NEXT_PUBLIC_API_URL.'
        )
    }

    const response = await fetch(`${API_URL}/api/Auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Email: payload.email, Senha: payload.password }),
    })

    const result: ApiResponseResult<LoginResponseData> = await response.json()

    if (!response.ok || !result.success || !result.data) {
        if (response.status === 401 || response.status === 400) {
        throw new Error(result.message || 'E-mail ou senha incorretos.')
        }
        throw new Error('Erro ao conectar com o servidor. Tente novamente.')
    }

    sessionStorage.setItem('token', result.data.token)

    return result.data
    }