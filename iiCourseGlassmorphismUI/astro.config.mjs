// @ts-check
import { defineConfig } from 'astro/config';
import vue from '@astrojs/vue';
import tailwindcss from '@tailwindcss/vite';
import unocss from 'unocss/astro';
import { fileURLToPath } from 'url';
import path from 'path';

const __dirname = fileURLToPath(new URL('.', import.meta.url));

// https://astro.build/config
export default defineConfig({
  integrations: [
    vue(),
    unocss(),
  ],
  vite: {
    plugins: [tailwindcss()],
    resolve: {
      alias: {
        '@': path.resolve(__dirname, './src'),
      },
    },
    build: {
      outDir: './dist',
      emptyOutDir: true,
      rollupOptions: {
        output: {
          manualChunks: undefined,
          entryFileNames: '_astro/[name].js',
          chunkFileNames: '_astro/[name].js',
          assetFileNames: (assetInfo) => {
            const info = assetInfo.name.split('.');
            const ext = info[info.length - 1];
            if (/\.(css)$/i.test(assetInfo.name)) {
              return '_astro/[name][extname]';
            }
            return '_astro/[name][extname]';
          },
        },
      },
    },
  },
});
