import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    tailwindcss()
  ],
  server: {
    // The backend's CORS policy only allows localhost:5173, so hopping to
    // another port when 5173 is busy just breaks every API call. Fail loudly.
    port: 5173,
    strictPort: true,
  },
})
  