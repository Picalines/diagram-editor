export const nonResolving: Promise<unknown> = new Promise(() => {});

export function delay(durationMs: number) {
	return new Promise(resolve => setTimeout(resolve, durationMs));
}

export async function holdPromise<T>(minDurationMs: number, promise: Promise<T>): Promise<T> {
	const minDurationSentinel = {};

	const minDurationPromise: Promise<typeof minDurationSentinel> = new Promise(resolve => {
		window.setTimeout(() => resolve(minDurationSentinel), minDurationMs);
	});

	const raceResult = await Promise.race([minDurationPromise, promise]);

	if (raceResult === minDurationSentinel) {
		return await promise;
	}

	await minDurationPromise;
	return raceResult as Awaited<T>;
}

export class RemotePromise<T> implements Promise<T> {
	readonly then: Promise<T>['then'];
	readonly catch: Promise<T>['catch'];
	readonly finally: Promise<T>['finally'];

	[Symbol.toStringTag] = RemotePromise.name;

	//@ts-expect-error #resolve is assigned inside Promise constructor
	#resolve: (value: T) => void;
	//@ts-expect-error #reject is assigned inside Promise constructor
	#reject: (reason?: unknown) => void;

	#state: 'pending' | 'resolved' | 'rejected' = 'pending';

	constructor() {
		const promise = new Promise<T>((resolve, reject) => {
			this.#resolve = resolve;
			this.#reject = reject;
		});

		this.then = (...args) => promise.then(...args);
		this.catch = (...args) => promise.catch(...args);
		this.finally = (...args) => promise.finally(...args);
	}

	resolve(value: T) {
		if (this.#state != 'pending') {
			throw new Error(`${RemotePromise.name}.${this.resolve.name} called in ${this.#state} state`);
		}

		this.#resolve(value);
		this.#state = 'resolved';
	}

	reject(reason?: unknown) {
		if (this.#state != 'pending') {
			throw new Error(`${RemotePromise.name}.${this.reject.name} called in ${this.#state} state`);
		}

		this.#reject(reason);
		this.#state = 'rejected';
	}

	get state() {
		return this.#state;
	}
}

export class LazyPromise<T> implements Promise<T> {
	readonly then: Promise<T>['then'];
	readonly catch: Promise<T>['catch'];
	readonly finally: Promise<T>['finally'];

	[Symbol.toStringTag] = 'LazyPromise';

	readonly #remote: RemotePromise<T>;
	#promise: Promise<T> | undefined;

	constructor(executor: (resolve: (value: T) => void, reject: (reason?: unknown) => void) => void) {
		this.#remote = new RemotePromise<T>();

		this.then = (...args) => {
			this.#startEvaluation(executor);
			return this.#remote.then(...args);
		};

		this.catch = (...args) => {
			this.#startEvaluation(executor);
			return this.#remote.catch(...args);
		};

		this.finally = (...args) => {
			this.#startEvaluation(executor);
			return this.#remote.finally(...args);
		};
	}

	static create<T>(createPromise: () => Promise<T>): LazyPromise<T> {
		return new LazyPromise(async resolve => {
			resolve(await createPromise());
		});
	}

	get state(): 'waiting' | RemotePromise<T>['state'] {
		return this.#promise === undefined ? 'waiting' : this.#remote.state;
	}

	#startEvaluation(
		executor: (resolve: (value: T) => void, reject: (reason?: unknown) => void) => void,
	) {
		if (this.#promise !== undefined) {
			return;
		}

		this.#promise = new Promise(executor);

		this.#promise.then(value => this.#remote.resolve(value));
		this.#promise.catch(reason => this.#remote.reject(reason));
		this.#promise.finally(() => this.#remote.finally());
	}
}

export class PromiseLock {
	[Symbol.toStringTag] = 'PromiseLock';

	#locked = false;
	#queue: RemotePromise<void>[] = [];

	async acquire(): Promise<void> {
		if (this.#locked) {
			const remote = new RemotePromise<void>();
			this.#queue.push(remote);
			await remote;
		} else {
			this.#locked = true;
		}
	}

	release() {
		const remote = this.#queue.shift();
		if (remote) {
			remote.resolve();
		} else {
			this.#locked = false;
		}
	}
}
