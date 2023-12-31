import { assertIsBrowser } from '$lib/assert';
import { Maybe } from '$lib/functional';
import { writable } from 'svelte/store';

type AuthTokens = Readonly<{
	accessToken: string;
	refreshToken: string;
}>;

const ACCESS_TOKEN_KEY = 'auth.access';
const REFRESH_TOKEN_KEY = 'auth.refresh';

const toBase64 = atob;
const fromBase64 = btoa;

const { set: _set, subscribe } = writable<Maybe<AuthTokens>>(Maybe.none(), setInitial => {
	assertIsBrowser();

	const accessToken = localStorage.getItem(ACCESS_TOKEN_KEY);
	const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY);

	if (!(accessToken && refreshToken)) {
		setInitial(Maybe.none());
		return;
	}

	try {
		setInitial(
			Maybe.some({ accessToken: toBase64(accessToken), refreshToken: toBase64(refreshToken) }),
		);
	} catch {
		localStorage.removeItem(ACCESS_TOKEN_KEY);
		localStorage.removeItem(REFRESH_TOKEN_KEY);
		setInitial(Maybe.none());
	}
});

const set = (tokens: Maybe<AuthTokens>) => {
	assertIsBrowser();

	localStorage.removeItem(ACCESS_TOKEN_KEY);
	localStorage.removeItem(REFRESH_TOKEN_KEY);

	tokens.ifSome(({ accessToken, refreshToken }) => {
		localStorage.setItem(ACCESS_TOKEN_KEY, fromBase64(accessToken));
		localStorage.setItem(REFRESH_TOKEN_KEY, fromBase64(refreshToken));
	});

	_set(tokens);
};

export const authTokens = { subscribe, set } as const;
