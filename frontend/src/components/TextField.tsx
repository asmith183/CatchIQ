interface TextFieldProps {
  id: string
  label: string
  value: string
  onChange: (value: string) => void
  type?: string
  autoComplete?: string
  required?: boolean
}

// `onChange` takes the string rather than the event: no call site needs the
// event object, and it lets them pass a `useState` setter directly.
function TextField({
  id,
  label,
  value,
  onChange,
  type = 'text',
  autoComplete,
  required = false,
}: TextFieldProps) {
  return (
    <div>
      <label htmlFor={id} className="block text-sm font-medium text-emerald-50">
        {label}
      </label>
      <input
        id={id}
        type={type}
        required={required}
        autoComplete={autoComplete}
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className="mt-1 w-full rounded border border-emerald-700 bg-emerald-950 px-3 py-2 text-emerald-50 focus:border-emerald-400 focus:outline-none"
      />
    </div>
  )
}

export default TextField
