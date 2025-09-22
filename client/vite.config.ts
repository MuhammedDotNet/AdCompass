import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000,
    host: 'localhost',
  },
  define: {
    'process.env.VITE_API_URL': JSON.stringify('https://localhost:7130/api')
  }
})