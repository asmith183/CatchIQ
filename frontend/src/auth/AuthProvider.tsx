import { useCallback, useEffect, useMemo, useState } from 'react'
import type { ReactNode } from 'react'
import { useNavigate } from 'react-router-dom'
import { authClient } from '../api'
import { getToken, setToken } from '../api/authToken'
import { setUnauthorizedHandler } from '../api/http'
import { AuthContext } from './AuthContext'
import type { AuthContextValue } from './AuthContext'

export function AuthProvider({ children }: { children: ReactNode }) {
  const navigate = useNavigate()
  const [isAuthenticated, setIsAuthenticated] = useState(() => getToken() !== null)

  // The fetch wrapper already cleared the token by the time this runs; this
  // syncs React state and sends the user back to the login page.
  useEffect(() => {
    setUnauthorizedHandler(() => {
      setIsAuthenticated(false)
      navigate('/login')
    })
  }, [navigate])

  const login = useCallback(async (email: string, password: string) => {
    const result = await authClient.login({ email, password })

    setToken(result.token)
    setIsAuthenticated(true)
  }, [])

  const register = useCallback(
    async (username: string, email: string, password: string) => {
      const result = await authClient.register({ username, email, password })

      setToken(result.token)
      setIsAuthenticated(true)
    },
    [],
  )

  const logout = useCallback(() => {
    setToken(null)
    setIsAuthenticated(false)
    navigate('/login')
  }, [navigate])

  const value = useMemo<AuthContextValue>(
    () => ({ isAuthenticated, login, register, logout }),
    [isAuthenticated, login, register, logout],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
