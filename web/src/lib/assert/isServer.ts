import { browser } from "$app/environment";

export function assertIsServer() {
	if (browser) {
		throw new Error('server-only code called in browser');
	}
}
