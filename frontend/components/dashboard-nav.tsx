'use client'

import Link from 'next/link'
import { usePathname, useRouter } from 'next/navigation'
import { LayoutDashboard, Users, LogOut, Menu } from 'lucide-react'
import { useState } from 'react'
import { cn } from '@/lib/utils'

const links = [
    { href: '/dashboard', label: 'In\u00EDcio', icon: LayoutDashboard },
    { href: '/dashboard/usuarios', label: 'Usu\u00E1rios', icon: Users },
]

export function DashboardNav() {
const pathname = usePathname()
const router = useRouter()
const [open, setOpen] = useState(false)

function handleLogout() {
sessionStorage.removeItem('token')
router.push('/')
}

return (
<header className="sticky top-0 z-10 border-b border-border bg-card">
    <div className="mx-auto flex h-16 max-w-6xl items-center justify-between px-4">
    <span className="text-lg font-semibold">Avo Eventos</span>

    <nav className="hidden items-center gap-1 sm:flex">
        {links.map(({ href, label, icon: Icon }) => (
        <Link
            key={href}
            href={href}
            className={cn(
            'flex items-center gap-2 rounded-md px-3 py-2 text-sm font-medium transition-colors',
            pathname === href
                ? 'bg-accent/10 text-accent'
                : 'text-muted-foreground hover:bg-muted hover:text-foreground'
            )}
        >
            <Icon className="size-4" />
            {label}
        </Link>
        ))}
        <button
        onClick={handleLogout}
        className="ml-2 flex items-center gap-2 rounded-md px-3 py-2 text-sm font-medium text-muted-foreground transition-colors hover:bg-muted hover:text-foreground"
        >
        <LogOut className="size-4" />
        Sair
        </button>
    </nav>

    <button className="sm:hidden" onClick={() => setOpen((v) => !v)} aria-label="Abrir menu">
        <Menu className="size-6" />
    </button>
    </div>

    {open && (
    <nav className="flex flex-col gap-1 border-t border-border px-4 py-3 sm:hidden">
        {links.map(({ href, label, icon: Icon }) => (
        <Link
            key={href}
            href={href}
            onClick={() => setOpen(false)}
            className={cn(
            'flex items-center gap-2 rounded-md px-3 py-2 text-sm font-medium',
            pathname === href ? 'bg-accent/10 text-accent' : 'text-muted-foreground hover:bg-muted'
            )}
        >
            <Icon className="size-4" />
            {label}
        </Link>
        ))}
        <button
        onClick={handleLogout}
        className="flex items-center gap-2 rounded-md px-3 py-2 text-left text-sm font-medium text-muted-foreground hover:bg-muted"
        >
        <LogOut className="size-4" />
        Sair
        </button>
    </nav>
    )}
</header>
)
}