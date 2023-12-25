import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vite';

export default defineConfig({
	plugins: [sveltekit()],

	server: {
		strictPort: true,

		host: '0.0.0.0',

		port: 5173,
		hmr: {
			clientPort: 8080,
		},

		watch: {
			usePolling: true,
		},
	},
});
