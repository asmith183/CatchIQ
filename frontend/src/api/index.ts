import {
  AuthClient,
  BaitClient,
  CatchClient,
  SpeciesClient,
  SpotClient,
} from './generated'
import { http } from './http'

const baseUrl = import.meta.env.VITE_API_URL

// Shared client instances. `http` attaches the JWT and handles 401s centrally.
export const authClient = new AuthClient(baseUrl, http)
export const baitClient = new BaitClient(baseUrl, http)
export const catchClient = new CatchClient(baseUrl, http)
export const speciesClient = new SpeciesClient(baseUrl, http)
export const spotClient = new SpotClient(baseUrl, http)

export * from './generated'
