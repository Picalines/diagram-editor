import { pipe } from 'fp-ts/function';
import { record } from 'fp-ts';
import type { MonadThrow1 } from 'fp-ts/lib/MonadThrow';
import type { HTTPClient1 } from './codegen/client/client';

const URI = 'Promise' as const;
type URI = typeof URI;

declare module 'fp-ts/HKT' {
	interface URItoKind<A> {
		readonly [URI]: Promise<A>;
	}
}

const monadPromise: MonadThrow1<URI> = {
	URI,
	of: Promise.resolve,
	throwError: Promise.reject,
	map: (fa, f) => fa.then(a => Promise.resolve(f(a))),
	ap: (fab, fa) => fab.then(fn => fa.then(a => fn(a))),
	chain: (fa, f) => fa.then(f),
};

export const fetchHttpClient: HTTPClient1<URI> = {
	...monadPromise,
	request: ({ body, headers, method, query, url }) =>
		fetch(query ? `${url}?${query}` : url, {
			body: body !== undefined ? JSON.stringify(body) : undefined,
			headers:
				headers &&
				pipe(
					headers,
					record.map(val => (Array.isArray(val) ? val.join(',') : String(val))),
				),
			method,
		}),
};
