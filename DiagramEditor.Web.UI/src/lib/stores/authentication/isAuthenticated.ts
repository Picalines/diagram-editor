import { derived } from 'svelte/store';
import { authTokens } from './authTokens';

const { subscribe } = derived(authTokens, tokens => tokens.isSome);

export const isAuthenticated = { subscribe } as const;
