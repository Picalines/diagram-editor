import { derived } from 'svelte/store';
import { authTokens } from './authTokens';

const { subscribe } = derived(authTokens, tokens => Boolean(tokens.isJust()));

export const isAuthenticated = { subscribe } as const;
