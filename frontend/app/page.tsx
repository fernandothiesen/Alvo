import Image from 'next/image'
import { LoginForm } from '@/components/login-form'

export default function LoginPage() {
  return (
    <main className="flex min-h-svh items-center justify-center bg-background px-4 py-10">
      <div className="w-full max-w-md">
        <div className="rounded-xl border border-border bg-card p-8 shadow-sm sm:p-10">
          <header className="mb-8 flex flex-col items-center gap-5 text-center">
            <Image
              src="/images/logo.png"
              alt="Avo Eventos"
              width={140}
              height={140}
              priority
              className="h-auto w-32"
            />
            <div className="flex flex-col gap-1.5">
              <h1 className="text-2xl font-semibold text-balance text-foreground">
                Bem-vindo de volta
              </h1>
              <p className="text-base leading-relaxed text-muted-foreground">
                Entre com seus dados para acessar sua conta
              </p>
            </div>
          </header>

          <LoginForm />
        </div>

        <p className="mt-6 text-center text-sm text-muted-foreground">
          {'\u00A9 '}
          {new Date().getFullYear()} Avo Eventos. Todos os direitos reservados.
        </p>
      </div>
    </main>
  )
}
