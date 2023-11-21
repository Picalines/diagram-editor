import { browser } from '$app/environment';

export function assertIsBrowser() {
	if (!browser) {
		throw new Error('browser-only code called on server');
	}
}
