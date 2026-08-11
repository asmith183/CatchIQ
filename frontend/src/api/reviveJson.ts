/**
 * Passed as the `jsonParseReviver` of every generated client, which otherwise
 * hands back raw JSON that does not match the types it declares:
 *
 *  - `DateTimeOffset` fields are typed `Date` but arrive as ISO strings.
 *  - Optional fields are typed `| undefined` but arrive as `null`.
 *
 * Returning `undefined` from a reviver drops the key entirely, which is what
 * the `| undefined` types already assume. Nulls inside an array would become
 * holes rather than being dropped, but no response returns one.
 */

const isoDateTime = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d+)?(Z|[+-]\d{2}:\d{2})$/

export function reviveJson(_key: string, value: unknown): unknown {
  if (typeof value === 'string' && isoDateTime.test(value)) {
    return new Date(value)
  }

  if (value === null) {
    return undefined
  }

  return value
}
