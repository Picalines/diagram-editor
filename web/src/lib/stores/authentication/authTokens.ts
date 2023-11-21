import { assertIsBrowser } from '$lib/assert';
import { Maybe } from 'purify-ts';
import { writable } from 'svelte/store';

type AuthTokens = {
	accessToken: string;
	refreshToken: string;
};

const ACCESS_TOKEN_KEY = 'auth.access';
const REFRESH_TOKEN_KEY = 'auth.refresh';

const { set: _set, subscribe: subscribe } = writable<Maybe<AuthTokens>>(
	Maybe.empty(),
	setInitial => {
		assertIsBrowser();

		const accessToken = localStorage.getItem(ACCESS_TOKEN_KEY);
		const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY);

		if (!(accessToken && refreshToken)) {
			setInitial(Maybe.empty());
			return;
		}

		try {
			setInitial(Maybe.of({ accessToken: atob(accessToken), refreshToken: atob(refreshToken) }));
		} catch {
			localStorage.removeItem(ACCESS_TOKEN_KEY);
			localStorage.removeItem(REFRESH_TOKEN_KEY);
			setInitial(Maybe.empty());
		}
	},
);

const set = (tokens: Maybe<AuthTokens>) => {
	assertIsBrowser();

	localStorage.removeItem(ACCESS_TOKEN_KEY);
	localStorage.removeItem(REFRESH_TOKEN_KEY);

	tokens.ifJust(({ accessToken, refreshToken }) => {
		localStorage.setItem(ACCESS_TOKEN_KEY, btoa(accessToken));
		localStorage.setItem(REFRESH_TOKEN_KEY, btoa(refreshToken));
	});

	_set(tokens);
};

export const authTokens = { subscribe, set } as const;
