'use client'

import { useEffect, useState } from 'react'
import { useRouter } from 'next/navigation'
import { Loader2 } from 'lucide-react'

export function AuthGuard({ children }: { children: React.ReactNode }) {
const router = useRouter()
const [checked, setChecked] = useState(false)

useEffect(() => {
    const token = sessionStorage.getItem('token')
    if (!token) {
    router.replace('/')
    return
    }
    setChecked(true)
}, [router])

if (!checked) {
    return (
    <div className="flex min-h-svh items-center justify-center bg-background">
        <Loader2 className="size-6 animate-spin text-muted-foreground" />
    </div>
    )
}

return <>{children}</>
}