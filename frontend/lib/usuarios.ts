// Importando a instância configurada do Axios em vez do apiFetch
import { api } from './api';

export interface UsuarioDto {
    IdUsuario: number;
    Nome: string;
    Email: string;
    Ativo: boolean;
    DataCriacao: string;
    UltimoLogin: string | null;
    Roles: string[];
    Permissoes: string[];
}

export interface CriarUsuarioPayload {
    Nome: string;
    Email: string;
    Senha: string;
    ConfirmarSenha: string;
    IdRole: number;
}

export async function listarUsuarios(): Promise<UsuarioDto[]> {
    // O Axios tipa o retorno na propriedade "data"
    const response = await api.get<UsuarioDto[]>('/api/Usuarios');
    return response.data;
}

export async function criarUsuario(payload: CriarUsuarioPayload): Promise<void> {
    // O Axios já transforma o objeto "payload" em JSON automaticamente
    await api.post('/api/Usuarios', payload);
}

export async function desativarUsuario(id: number): Promise<void> {
    await api.put(`/api/Usuarios/${id}/desativar`);
}