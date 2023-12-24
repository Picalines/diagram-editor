import type { User } from '$api/codegen';
import { derived } from 'svelte/store';
import { authTokens } from './authTokens';
import { parseJwt } from '$lib/util';
import { Maybe } from '$lib/functional';

type UserId = NonNullable<User['id']>;

const parseUserId = (accessToken: string): Maybe<UserId> =>
	Maybe.some(parseJwt(accessToken))
		.flatMap(payload => Maybe.nullable('id' in payload ? payload.id : null))
		.map(String)
		.map(parseInt)
		.filter(isFinite);

const { subscribe } = derived(authTokens, tokens =>
	tokens.flatMap(({ accessToken }) => parseUserId(accessToken)),
);

export const currentUserId = { subscribe } as const;
