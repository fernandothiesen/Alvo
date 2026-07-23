'use client'

import { useState, useEffect } from 'react'
import { useRouter } from 'next/navigation'
import Link from 'next/link'
import { Loader2, ArrowLeft } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { criarUsuario } from '@/lib/usuarios'
import { obterRoles, RoleOption } from '@/lib/roles' // Importação atualizada
import axios from 'axios' // Adicionado o Axios para o tratamento de erros

export default function NovoUsuarioPage() {
  const router = useRouter()
  const [form, setForm] = useState({ nome: '', email: '', senha: '', confirmarSenha: '', idRole: '' })
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)
  const [roles, setRoles] = useState<RoleOption[]>([]) // Novo estado para armazenar as roles da API

  // Efeito para buscar as roles quando a página carregar
  useEffect(() => {
    async function carregarRoles() {
      try {
        const dados = await obterRoles()
        setRoles(dados)
      } catch (err) {
        console.error('Falha ao carregar as roles:', err)
        setError('Não foi possível carregar as opções de papéis (roles).')
      }
    }
    
    carregarRoles()
  }, [])

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    setError(null)

    if (form.senha !== form.confirmarSenha) {
      setError('As senhas não coincidem.')
      return
    }
    if (!form.idRole) {
      setError('Selecione um papel para o usuário.')
      return
    }

    setLoading(true)
    try {
      await criarUsuario({
        Nome: form.nome,
        Email: form.email,
        Senha: form.senha,
        ConfirmarSenha: form.confirmarSenha,
        IdRole: Number(form.idRole),
      })
      router.push('/dashboard/usuarios')
    } catch (err: unknown) {
      // Nova lógica de tratamento de erro adaptada para o Axios
      if (axios.isAxiosError(err) && err.response?.data?.Message) {
        setError(err.response.data.Message)
      } else {
        setError(err instanceof Error ? err.message : 'Erro ao criar usuário.')
      }
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="mx-auto flex max-w-lg flex-col gap-6">
      <Link href="/dashboard/usuarios" className="flex items-center gap-1 text-sm text-muted-foreground hover:text-foreground">
        <ArrowLeft className="size-4" />
        Voltar
      </Link>

      <div className="rounded-xl border border-border bg-card p-8 shadow-sm">
        <h1 className="mb-6 text-2xl font-semibold text-foreground">Novo usuário</h1>

        <form onSubmit={handleSubmit} className="flex flex-col gap-5" noValidate>
          {error && (
            <div role="alert" className="rounded-md border border-destructive/30 bg-destructive/5 px-4 py-3 text-sm text-destructive">
              {error}
            </div>
          )}

          <div className="flex flex-col gap-2">
            <Label htmlFor="nome">Nome</Label>
            <Input id="nome" required value={form.nome} onChange={(e) => setForm({ ...form, nome: e.target.value })} />
          </div>

          <div className="flex flex-col gap-2">
            <Label htmlFor="email">E-mail</Label>
            <Input id="email" type="email" required value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} />
          </div>

          <div className="flex flex-col gap-2">
            <Label htmlFor="senha">Senha</Label>
            <Input id="senha" type="password" required minLength={6} value={form.senha} onChange={(e) => setForm({ ...form, senha: e.target.value })} />
          </div>

          <div className="flex flex-col gap-2">
            <Label htmlFor="confirmarSenha">Confirmar senha</Label>
            <Input id="confirmarSenha" type="password" required value={form.confirmarSenha} onChange={(e) => setForm({ ...form, confirmarSenha: e.target.value })} />
          </div>

          <div className="flex flex-col gap-2">
            <Label htmlFor="role">Papel</Label>
            <Select value={form.idRole} onValueChange={(value) => setForm({ ...form, idRole: value })}>
              <SelectTrigger id="role">
                <SelectValue placeholder="Selecione um papel" />
              </SelectTrigger>
              <SelectContent>
                {/* Agora mapeamos o estado `roles` preenchido pela API */}
                {roles.map((role) => (
                  <SelectItem key={role.id} value={String(role.id)}>{role.nome}</SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          <Button type="submit" disabled={loading} className="h-11 font-semibold">
            {loading ? (<><Loader2 className="size-4 animate-spin" />Criando...</>) : 'Criar usuário'}
          </Button>
        </form>
      </div>
    </div>
  )
}