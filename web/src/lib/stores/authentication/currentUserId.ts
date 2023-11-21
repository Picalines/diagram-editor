import type { User } from '$api/codegen';
import { Maybe } from 'purify-ts';
import { derived } from 'svelte/store';
import { authTokens } from './authTokens';
import { parseJwt } from '$lib/util';

type UserId = NonNullable<User['id']>;

function parseUserId(accessToken: string): Maybe<UserId> {
	const payload = parseJwt(accessToken);
	const userId = 'id' in payload ? payload.id : null;
	return Maybe.fromNullable(userId).map(String).map(parseInt).filter(isFinite);
}

const { subscribe } = derived(authTokens, tokens =>
	tokens.chain(({ accessToken }) => parseUserId(accessToken)),
);

export const currentUserId = { subscribe } as const;
