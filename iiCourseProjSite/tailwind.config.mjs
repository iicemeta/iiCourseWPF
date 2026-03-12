/** @type {import('tailwindcss').Config} */
import daisyui from 'daisyui';

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
  plugins: [daisyui],
  daisyui: {
    themes: [
      {
        iicourse: {
          primary: '#FF6B35',
          'primary-focus': '#E55A2B',
          'primary-content': '#FFFFFF',
          secondary: '#FFB347',
          'secondary-focus': '#F5A030',
          'secondary-content': '#1F2937',
          accent: '#2A9D8F',
          'accent-focus': '#248A7E',
          'accent-content': '#FFFFFF',
          neutral: '#5A392C',
          'neutral-focus': '#4A2F24',
          'neutral-content': '#FFFFFF',
          'base-100': '#FDF8F3',
          'base-200': '#F9F0E8',
          'base-300': '#F3E0D0',
          'base-content': '#1F2937',
          info: '#3ABFF8',
          success: '#36D399',
          warning: '#FBBD23',
          error: '#F87272',
        }
      }
    ],
    darkTheme: false,
    base: true,
    styled: true,
    utils: true,
  }
}
