import { derived } from 'svelte/store';
import { authTokens } from './authTokens';
import { parseJwt } from '$lib/util';
import { Maybe } from '$lib/functional';
import type { UserDTO } from '$api/codegen';

type UserId = NonNullable<UserDTO['id']>;

const parseUserId = (accessToken: string): Maybe<UserId> =>
	Maybe.some(parseJwt(accessToken))
		.flatMap(payload => Maybe.nullable('id' in payload ? payload.id : null))
		.map(String);

const { subscribe } = derived(authTokens, tokens =>
	tokens.flatMap(({ accessToken }) => parseUserId(accessToken)),
);

export const currentUserId = { subscribe } as const;
