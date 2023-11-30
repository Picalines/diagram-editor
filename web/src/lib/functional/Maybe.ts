const noneIterator: Iterator<never> = {
	next() {
		return { done: true, value: undefined };
	},
};

export class Maybe<T> implements Iterable<T> {
	static readonly #none = new Maybe(false);

	readonly #hasValue: boolean;
	readonly #value: T;

	private constructor(hasValue: boolean, value?: T) {
		this.#hasValue = hasValue;
		this.#value = value as T;
	}

	public static some<T>(value: T): Maybe<T> {
		return new Maybe(true, value);
	}

	public static none<T = never>(): Maybe<T> {
		return Maybe.#none as Maybe<T>;
	}

	public static nullable<T>(value: T | null | undefined): Maybe<T> {
		return value === null || value === undefined ? Maybe.none() : Maybe.some(value);
	}

	public get isSome(): boolean {
		return this.#hasValue;
	}

	public get isNone(): boolean {
		return !this.#hasValue;
	}

	public unwrap(errorMessage?: string): T {
		if (this.isNone) {
			throw new Error(errorMessage ?? 'Maybe contains no value');
		}

		return this.#value;
	}

	public orDefault(defaultValue: T): T {
		return this.isSome ? this.#value : defaultValue;
	}

	public map<U>(f: (value: T) => U): Maybe<U> {
		return this.isSome ? Maybe.some(f(this.#value)) : Maybe.none();
	}

	public flatMap<U>(f: (value: T) => Maybe<U>): Maybe<U> {
		return this.isSome ? f(this.#value) : Maybe.none();
	}

	public filter(predicate: (value: T) => boolean): Maybe<T> {
		return this.isSome && predicate(this.#value) ? this : Maybe.none();
	}

	public ifSome(action: (value: T) => unknown): void {
		if (this.isSome) {
			action(this.#value);
		}
	}

	public ifNone(action: () => unknown): void {
		if (this.isNone) {
			action();
		}
	}

	public match<U, V>(ifSome: (value: T) => U, ifNone: () => V): U | V {
		return this.isSome ? ifSome(this.#value) : ifNone();
	}

	public toString(): string {
		return this.isSome ? `Some(${this.#value})` : 'None';
	}

	public toArray(): T[] {
		return this.isSome ? [this.#value] : [];
	}

	[Symbol.iterator](): Iterator<T> {
		return this.isNone
			? noneIterator
			: {
					next: () => ({ done: true, value: this.#value }),
			  };
	}
}
