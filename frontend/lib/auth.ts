    const API_URL = process.env.NEXT_PUBLIC_API_URL

    export interface LoginPayload {
    email: string
    password: string
    }

    export interface UsuarioLogado {
    IdUsuario: number
    Nome: string
    Email: string
    Ativo: boolean
    DataCriacao: string
    UltimoLogin: string | null
    Roles: string[]
    Permissoes: string[]
    }

    export interface LoginResponseData {
    Token: string
    Expiracao: string
    Usuario: UsuarioLogado
    }

    interface ApiResponseResult<T = unknown> {
    Success: boolean
    Message: string
    Data?: T
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

    if (!response.ok || !result.Success || !result.Data) {
        if (response.status === 401 || response.status === 400) {
        throw new Error(result.Message || 'E-mail ou senha incorretos.')
        }
        throw new Error('Erro ao conectar com o servidor. Tente novamente.')
    }

    sessionStorage.setItem('token', result.Data.Token)

    return result.Data
    }