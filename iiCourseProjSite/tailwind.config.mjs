/** @type {import('tailwindcss').Config} */
export default {
  content: ['./src/**/*.{astro,html,js,jsx,md,mdx,svelte,ts,tsx,vue}'],
  theme: {
    extend: {
      colors: {
        primary: '#FF6B35',
        secondary: '#FFB347',
        accent: '#2A9D8F',
        coral: '#FF6B6B',
        warm: {
          50: '#FDF8F3',
          100: '#F9F0E8',
          200: '#F3E0D0',
          300: '#E8C9B0',
          400: '#D9A885',
          500: '#C98A5E',
          600: '#A66D45',
          700: '#855238',
          800: '#6D4432',
          900: '#5A392C',
        }
      },
      fontFamily: {
        sans: ['system-ui', '-apple-system', 'BlinkMacSystemFont', 'Segoe UI', 'Roboto', 'sans-serif'],
        mono: ['Fira Code', 'monospace'],
      },
      animation: {
        'float': 'float 6s ease-in-out infinite',
        'pulse-slow': 'pulse 4s cubic-bezier(0.4, 0, 0.6, 1) infinite',
      },
      keyframes: {
        float: {
          '0%, 100%': { transform: 'translateY(0)' },
          '50%': { transform: 'translateY(-20px)' },
        }
      }
    },
  },
  plugins: [],
}
