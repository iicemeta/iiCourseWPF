/** @type {import('tailwindcss').Config} */
import daisyui from 'daisyui';

export default {
  content: ['./src/**/*.{astro,html,js,jsx,md,mdx,svelte,ts,tsx,vue}'],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#FF6B35',
          light: '#FF8A5C',
          dark: '#E55A2B',
        },
        secondary: {
          DEFAULT: '#FFB347',
          light: '#FFD4A3',
        },
        accent: {
          DEFAULT: '#2A9D8F',
          light: '#3DBFB0',
        },
        coral: {
          DEFAULT: '#FF6B6B',
          light: '#FF8E8E',
        },
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
        },
        glass: {
          white: 'rgba(255, 255, 255, 0.72)',
          'white-light': 'rgba(255, 255, 255, 0.45)',
          'white-strong': 'rgba(255, 255, 255, 0.85)',
          border: 'rgba(255, 255, 255, 0.5)',
          dark: 'rgba(31, 41, 55, 0.65)',
        }
      },
      fontFamily: {
        sans: ['Nunito', 'Noto Sans SC', 'system-ui', '-apple-system', 'sans-serif'],
        mono: ['Fira Code', 'monospace'],
      },
      boxShadow: {
        'glass': '0 8px 32px rgba(0, 0, 0, 0.08)',
        'glass-lg': '0 12px 48px rgba(0, 0, 0, 0.12)',
        'glass-xl': '0 20px 64px rgba(0, 0, 0, 0.15)',
        'inner-glass': 'inset 0 1px 0 rgba(255, 255, 255, 0.6)',
        'glow-primary': '0 0 40px rgba(255, 107, 53, 0.3)',
        'glow-secondary': '0 0 40px rgba(255, 179, 71, 0.3)',
      },
      backdropBlur: {
        'glass': '20px',
      },
      animation: {
        'float': 'float 6s ease-in-out infinite',
        'float-delayed': 'float 6s ease-in-out infinite 3s',
        'pulse-glow': 'pulse-glow 3s ease-in-out infinite',
        'gradient': 'gradient-shift 8s ease infinite',
        'glass-shine': 'glass-shine 3s ease-in-out infinite',
      },
      keyframes: {
        float: {
          '0%, 100%': { transform: 'translateY(0) rotate(0deg)' },
          '50%': { transform: 'translateY(-20px) rotate(2deg)' },
        },
        'pulse-glow': {
          '0%, 100%': { boxShadow: '0 0 20px rgba(255, 107, 53, 0.3)' },
          '50%': { boxShadow: '0 0 40px rgba(255, 107, 53, 0.5)' },
        },
        'gradient-shift': {
          '0%': { backgroundPosition: '0% 50%' },
          '50%': { backgroundPosition: '100% 50%' },
          '100%': { backgroundPosition: '0% 50%' },
        },
        'glass-shine': {
          '0%': { backgroundPosition: '-200% 0' },
          '100%': { backgroundPosition: '200% 0' },
        },
      },
      backgroundImage: {
        'gradient-radial': 'radial-gradient(var(--tw-gradient-stops))',
        'gradient-conic': 'conic-gradient(from 180deg at 50% 50%, var(--tw-gradient-stops))',
      },
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
