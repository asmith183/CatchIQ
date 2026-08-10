import { useState } from 'react'
import type { SubmitEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { ApiException } from '../api/generated'
import TextField from '../components/TextField'

// A failed registration comes back as `{ errors: [...] }` carrying Identity's
// own descriptions, which are already written for end users.
function readValidationErrors(response: string): string | null {
  try {
    const body = JSON.parse(response) as { errors?: string[] }

    return body.errors?.length ? body.errors.join(' ') : null
  } catch {
    return null
  }
}

function Register() {
  const { register } = useAuth()
  const navigate = useNavigate()

  const [username, setUsername] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  async function handleSubmit(e: SubmitEvent<HTMLFormElement>) {
    e.preventDefault()
    setError(null)
    setIsSubmitting(true)

    try {
      await register(username, email, password)
      navigate('/', { replace: true })
    } catch (error) {
      if (ApiException.isApiException(error) && error.status === 400) {
        setError(
          readValidationErrors(error.response) ??
            'Registration failed. Please check your details.',
        )
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
        className="w-full max-w-sm space-y-4 rounded-lg border border-line bg-panel p-8"
      >
        <h1 className="text-2xl font-bold text-heading">Create an account</h1>

        <TextField
          id="username"
          label="Username"
          required
          autoComplete="username"
          value={username}
          onChange={setUsername}
        />

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
          autoComplete="new-password"
          value={password}
          onChange={setPassword}
        />

        {error !== null && <p className="text-sm text-danger">{error}</p>}

        <button
          type="submit"
          disabled={isSubmitting}
          className="w-full rounded bg-primary py-2 text-heading hover:brightness-110 disabled:opacity-50"
        >
          {isSubmitting ? 'Creating account…' : 'Create account'}
        </button>

        <p className="text-sm text-heading">
          Already have an account?{' '}
          <Link to="/login" className="text-icon underline">
            Log in
          </Link>
        </p>
      </form>
    </div>
  )
}

export default Register
