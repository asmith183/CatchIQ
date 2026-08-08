const STORAGE_KEY = 'catchiq.token'

// Cached in a module variable so we don't touch localStorage on every request.
// localStorage remains the source of truth across page loads.
let token: string | null = localStorage.getItem(STORAGE_KEY)

export function getToken(): string | null {
  return token
}

export function setToken(next: string | null): void {
  token = next

  if (next === null) {
    localStorage.removeItem(STORAGE_KEY)
  } else {
    localStorage.setItem(STORAGE_KEY, next)
  }
}
