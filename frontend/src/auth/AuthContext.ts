import { createContext, useContext } from 'react'

export interface AuthContextValue {
  isAuthenticated: boolean
  login: (email: string, password: string) => Promise<void>
  register: (username: string, email: string, password: string) => Promise<void>
  logout: () => void
}

// Kept separate from AuthProvider so that file exports only a component,
// which is what react-refresh needs to hot-reload it reliably.
export const AuthContext = createContext<AuthContextValue | undefined>(undefined)

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext)

  if (context === undefined) {
    throw new Error('useAuth must be used inside an AuthProvider')
  }

  return context
}
