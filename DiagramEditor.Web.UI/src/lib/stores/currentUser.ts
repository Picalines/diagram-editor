import { derived } from 'svelte/store';
import { isAuthenticated } from './authentication';
import { apiClient } from '$api';
import { Maybe } from '$lib/functional';
import type { UserDTO } from '$api/codegen';

const { subscribe } = derived(isAuthenticated, $isAuthenticated =>
	$isAuthenticated
		? (async () => {
				const { user } = await apiClient.currentUser.getCurrentUser();
				return Maybe.some(user);
			})()
		: Promise.resolve(Maybe.none<UserDTO>()),
);

export const currentUser = { subscribe } as const;
