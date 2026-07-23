'use client'

import { useEffect, useState } from 'react'
import Link from 'next/link'
import { Loader2, Plus, UserX } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { listarUsuarios, desativarUsuario, UsuarioDto } from '@/lib/usuarios'
import axios from 'axios' // Importando o axios para checar o tipo do erro

export default function UsuariosPage() {
  const [usuarios, setUsuarios] = useState<UsuarioDto[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [desativandoId, setDesativandoId] = useState<number | null>(null)

  async function carregar() {
    setLoading(true)
    setError(null)
    try {
      setUsuarios(await listarUsuarios())
    } catch (err: unknown) {
      // Nova lógica de extração de erro adaptada para o Axios
      if (axios.isAxiosError(err) && err.response?.data?.Message) {
        setError(err.response.data.Message)
      } else {
        setError(err instanceof Error ? err.message : 'Erro ao carregar usuários.')
      }
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    carregar()
  }, [])

  async function handleDesativar(id: number) {
    if (!confirm('Deseja realmente desativar este usuário?')) return
    setDesativandoId(id)
    try {
      await desativarUsuario(id)
      await carregar()
    } catch (err: unknown) {
      // Nova lógica de extração de erro adaptada para o Axios
      let mensagem = 'Erro ao desativar usuário.'
      if (axios.isAxiosError(err) && err.response?.data?.Message) {
        mensagem = err.response.data.Message
      } else if (err instanceof Error) {
        mensagem = err.message
      }
      alert(mensagem)
    } finally {
      setDesativandoId(null)
    }
  }

  return (
    <div className="flex flex-col gap-6">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-semibold text-foreground">Usuários</h1>
        <Button asChild>
          <Link href="/dashboard/usuarios/novo" className="flex items-center gap-2">
            <Plus className="size-4" />
            Novo usuário
          </Link>
        </Button>
      </div>

      {error && (
        <div className="rounded-md border border-destructive/30 bg-destructive/5 px-4 py-3 text-sm text-destructive">
          {error}
        </div>
      )}

      {loading ? (
        <div className="flex items-center justify-center py-16">
          <Loader2 className="size-6 animate-spin text-muted-foreground" />
        </div>
      ) : (
        <div className="overflow-x-auto rounded-xl border border-border bg-card">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Nome</TableHead>
                <TableHead>E-mail</TableHead>
                <TableHead>Roles</TableHead>
                <TableHead>Status</TableHead>
                <TableHead className="text-right">Ações</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {usuarios.map((u) => (
                <TableRow key={u.IdUsuario}>
                  <TableCell className="font-medium">{u.Nome}</TableCell>
                  <TableCell className="text-muted-foreground">{u.Email}</TableCell>
                  <TableCell>
                    <div className="flex flex-wrap gap-1">
                      {u.Roles.length > 0 ? (
                        u.Roles.map((r) => <Badge key={r} variant="secondary">{r}</Badge>)
                      ) : (
                        <span className="text-sm text-muted-foreground">—</span>
                      )}
                    </div>
                  </TableCell>
                  <TableCell>
                    <Badge variant={u.Ativo ? 'default' : 'outline'}>
                      {u.Ativo ? 'Ativo' : 'Inativo'}
                    </Badge>
                  </TableCell>
                  <TableCell className="text-right">
                    {u.Ativo && (
                      <Button
                        variant="ghost"
                        size="sm"
                        disabled={desativandoId === u.IdUsuario}
                        onClick={() => handleDesativar(u.IdUsuario)}
                        className="text-destructive hover:text-destructive"
                      >
                        <UserX className="size-4" />
                      </Button>
                    )}
                  </TableCell>
                </TableRow>
              ))}
              {usuarios.length === 0 && (
                <TableRow>
                  <TableCell colSpan={5} className="py-8 text-center text-muted-foreground">
                    Nenhum usuário cadastrado.
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </div>
      )}
    </div>
  )
}