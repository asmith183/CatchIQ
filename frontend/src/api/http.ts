import { getToken, setToken } from './authToken'

/**
 * Passed as the `http` argument to every generated client. It attaches the JWT
 * and centralizes 401 handling, so no page has to think about either.
 */

type UnauthorizedHandler = () => void

let onUnauthorized: UnauthorizedHandler = () => {}

/** Registered by AuthProvider so a rejected token can clear React state and redirect. */
export function setUnauthorizedHandler(handler: UnauthorizedHandler): void {
  onUnauthorized = handler
}

// A 401 from the login/register endpoints means "bad credentials", not "session
// expired" — treating it as the latter would fire a spurious logout redirect.
function isAuthEndpoint(url: RequestInfo): boolean {
  const path = typeof url === 'string' ? url : url.url

  return path.includes('/api/Auth/')
}

export const http = {
  async fetch(url: RequestInfo, init?: RequestInit): Promise<Response> {
    const token = getToken()
    const headers = new Headers(init?.headers)

    if (token !== null) {
      headers.set('Authorization', `Bearer ${token}`)
    }

    const response = await window.fetch(url, { ...init, headers })

    if (response.status === 401 && !isAuthEndpoint(url)) {
      setToken(null)
      onUnauthorized()
    }

    return response
  },
}
