    'use client'

    import { useState } from 'react'
    import { useRouter } from 'next/navigation'
    import { Eye, EyeOff, Loader2 } from 'lucide-react'
    import { Button } from '@/components/ui/button'
    import { Input } from '@/components/ui/input'
    import { Label } from '@/components/ui/label'
    import { login } from '@/lib/auth'

    export function LoginForm() {
    const router = useRouter()
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [showPassword, setShowPassword] = useState(false)
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault()
        setError(null)
        setIsLoading(true)

        try {
        await login({ email, password })
        router.push('/dashboard')
        } catch (err) {
        setError(
            err instanceof Error
            ? err.message
            : 'N\u00E3o foi poss\u00EDvel entrar. Verifique seus dados e tente novamente.'
        )
        } finally {
        setIsLoading(false)
        }
    }

    return (
        <form onSubmit={handleSubmit} className="flex flex-col gap-5" noValidate>
        {error && (
            <div
            role="alert"
            className="rounded-md border border-destructive/30 bg-destructive/5 px-4 py-3 text-sm text-destructive"
            >
            {error}
            </div>
        )}

        <div className="flex flex-col gap-2">
            <Label htmlFor="email" className="text-base">
            E-mail
            </Label>
            <Input
            id="email"
            type="email"
            autoComplete="email"
            placeholder="seuemail@exemplo.com"
            required
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="h-12 text-base"
            />
        </div>

        <div className="flex flex-col gap-2">
            <Label htmlFor="password" className="text-base">
            Senha
            </Label>
            <div className="relative">
            <Input
                id="password"
                type={showPassword ? 'text' : 'password'}
                autoComplete="current-password"
                placeholder="Digite sua senha"
                required
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="h-12 pr-12 text-base"
            />
            <button
                type="button"
                onClick={() => setShowPassword((v) => !v)}
                className="absolute inset-y-0 right-0 flex w-12 items-center justify-center text-muted-foreground transition-colors hover:text-foreground"
                aria-label={showPassword ? 'Ocultar senha' : 'Mostrar senha'}
            >
                {showPassword ? <EyeOff className="size-5" /> : <Eye className="size-5" />}
            </button>
            </div>
        </div>

        <Button type="submit" disabled={isLoading} className="h-12 text-base font-semibold">
            {isLoading ? (
            <>
                <Loader2 className="size-5 animate-spin" aria-hidden="true" />
                Entrando...
            </>
            ) : (
            'Entrar'
            )}
        </Button>
        </form>
    )
    }