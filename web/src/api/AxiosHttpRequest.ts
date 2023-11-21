import type { AxiosInstance } from 'axios';
import { BaseHttpRequest, CancelablePromise, type OpenAPIConfig } from './codegen';
import type { ApiRequestOptions } from './codegen/core/ApiRequestOptions';
import { request as __request } from './codegen/core/request';

export class AxiosHttpRequest extends BaseHttpRequest {
	constructor(
		public readonly axios: AxiosInstance,
		apiConfig: OpenAPIConfig,
	) {
		super(apiConfig);
	}

	public request<T>(options: ApiRequestOptions): CancelablePromise<T> {
		return __request(this.config, options, this.axios);
	}

	public static factory(axios: AxiosInstance): new (apiConfig: OpenAPIConfig) => AxiosHttpRequest {
		return class extends AxiosHttpRequest {
			constructor(apiConfig: OpenAPIConfig) {
				super(axios, apiConfig);
			}
		};
	}
}
