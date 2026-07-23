import { api } from './api'; // Importando a instância configurada no passo anterior
import axios from 'axios';
export interface LoginPayload {
  email: string;
  password: string;
}

export interface UsuarioLogado {
  IdUsuario: number;
  Nome: string;
  Email: string;
  Ativo: boolean;
  DataCriacao: string;
  UltimoLogin: string | null;
  Roles: string[];
  Permissoes: string[];
}

export interface LoginResponseData {
  Token: string;
  Expiracao: string;
  Usuario: UsuarioLogado;
}

interface ApiResponseResult<T = unknown> {
    Success: boolean;
    Message: string;
    Data?: T;
}

export async function login(payload: LoginPayload): Promise<LoginResponseData> {
  try {
    const response = await api.post<ApiResponseResult<LoginResponseData>>(
      '/api/Auth/login',
      { Email: payload.email, Senha: payload.password }
    );

    const result = response.data;

    if (!result.Success || !result.Data) {
      throw new Error(result.Message || 'Erro ao realizar login.');
    }

    sessionStorage.setItem('token', result.Data.Token);
    return result.Data;

  } catch (error: unknown) { // Melhor usar unknown do que any
    
    // 1. Verifica primeiro se é um erro gerado por uma requisição do Axios (ex: 400, 401)
    if (axios.isAxiosError(error)) {
      const status = error.response?.status;
      const dataErro = error.response?.data as ApiResponseResult;

      if (status === 401 || status === 400) {
        throw new Error(dataErro?.Message || 'E-mail ou senha incorretos.');
      }
    }
    
    // 2. Se for um Erro padrão do JavaScript (como o que lançamos manualmente no `if (!result.Success)`)
    if (error instanceof Error) {
      throw error;
    }

    // 3. Fallback genérico para erros de rede, servidor fora do ar, etc.
    throw new Error('Erro ao conectar com o servidor. Tente novamente.');
  }
}