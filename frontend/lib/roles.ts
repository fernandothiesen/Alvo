import { api } from './api';

// Interface que reflete o RoleDto retornado pelo C#
export interface RoleOption {
  id: number;
  nome: string;
}

/**
 * Busca a lista de roles na API .NET
 * O token JWT é injetado automaticamente pelo interceptador do axios (api.ts)
 */
export async function obterRoles(): Promise<RoleOption[]> {
  try {
    // O Axios já concatena a baseURL definida no NEXT_PUBLIC_API_URL
    const response = await api.get<RoleOption[]>('/api/Roles');
    
    // Retorna diretamente o array de roles
    return response.data;
  } catch (error) {
    console.error('Falha ao obter roles disponíveis:', error);
    // Propaga o erro para ser tratado no componente (ex: exibir um toast de erro)
    throw error;
  }
}