import {
  AuthClient,
  BaitClient,
  CatchClient,
  InsightsClient,
  SpeciesClient,
  SpotClient,
} from './generated'
import { http } from './http'
import { reviveJson } from './reviveJson'

const baseUrl = import.meta.env.VITE_API_URL

// Every generated client runs its responses through `jsonParseReviver`, but
// leaves it unset. It is `protected`, so setting it needs the cast.
function withReviver<T>(client: T): T {
  ;(client as { jsonParseReviver?: typeof reviveJson }).jsonParseReviver = reviveJson

  return client
}

// Shared client instances. `http` attaches the JWT and handles 401s centrally.
export const authClient = withReviver(new AuthClient(baseUrl, http))
export const baitClient = withReviver(new BaitClient(baseUrl, http))
export const catchClient = withReviver(new CatchClient(baseUrl, http))
export const insightsClient = withReviver(new InsightsClient(baseUrl, http))
export const speciesClient = withReviver(new SpeciesClient(baseUrl, http))
export const spotClient = withReviver(new SpotClient(baseUrl, http))

export * from './generated'
