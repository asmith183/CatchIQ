import { useState } from 'react'
import type { SubmitEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { ApiException } from '../api/generated'
import TextField from '../components/TextField'

function Login() {
  const { login } = useAuth()
  const navigate = useNavigate()

  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  async function handleSubmit(e: SubmitEvent<HTMLFormElement>) {
    e.preventDefault()
    setError(null)
    setIsSubmitting(true)

    try {
      await login(email, password)
      navigate('/', { replace: true })
    } catch (error) {
      if (ApiException.isApiException(error) && error.status === 401) {
        setError('Incorrect email or password.')
      } else if (ApiException.isApiException(error)) {
        setError('Something went wrong. Please try again.')
      } else {
        setError('Could not reach the server. Please try again.')
      }
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <div className="flex min-h-screen items-center justify-center">
      <form
        onSubmit={handleSubmit}
        className="w-full max-w-sm space-y-4 rounded-lg border border-emerald-800 bg-emerald-900 p-8"
      >
        <h1 className="text-2xl font-bold text-emerald-50">Welcome back</h1>

        <TextField
          id="email"
          label="Email"
          type="email"
          required
          autoComplete="email"
          value={email}
          onChange={setEmail}
        />

        <TextField
          id="password"
          label="Password"
          type="password"
          required
          autoComplete="current-password"
          value={password}
          onChange={setPassword}
        />

        {error !== null && <p className="text-sm text-red-600">{error}</p>}

        <button
          type="submit"
          disabled={isSubmitting}
          className="w-full rounded bg-emerald-600 py-2 text-white disabled:opacity-50"
        >
          {isSubmitting ? 'Logging in…' : 'Log in'}
        </button>

        <p className="text-sm text-emerald-50">
          Need an account?{' '}
          <Link to="/register" className="text-teal-400 underline">
            Create one
          </Link>
        </p>
      </form>
    </div>
  )
}

export default Login
