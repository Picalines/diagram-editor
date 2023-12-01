import { assertIsBrowser } from '$lib/assert';
import { authTokens } from '$lib/stores';
import { get } from 'svelte/store';
import { AxiosHttpRequest } from './AxiosHttpRequest';
import { GeneratedApiClient } from './codegen';
import axios, { AxiosError } from 'axios';
import { Maybe } from '$lib/functional';

assertIsBrowser();

const axiosInstance = axios.create();

axiosInstance.interceptors.request.use(request => {
	get(authTokens).ifSome(({ accessToken }) => {
		request.headers['Authorization'] = `Bearer ${accessToken}`;
	});

	return request;
});

export function useRefreshInterceptor() {
	const interceptor = axiosInstance.interceptors.response.use(
		response => response,
		error => {
			if (!(error instanceof AxiosError)) {
				return Promise.reject(error);
			}

			const { response } = error;

			if (response?.status !== 401) {
				return Promise.reject(error);
			}

			axiosInstance.interceptors.response.eject(interceptor);

			return get(authTokens)
				.map(({ accessToken, refreshToken }) =>
					apiClient.user
						.postRefreshUser({ requestBody: { accessToken, refreshToken } })
						.then(newTokens => {
							authTokens.set(Maybe.some(newTokens));
							return axiosInstance(response.config);
						})
						.catch(refreshError => {
							authTokens.set(Maybe.none());
							return Promise.reject(refreshError);
						})
						.finally(useRefreshInterceptor),
				)
				.orDefault(Promise.reject(error));
		},
	);
}

useRefreshInterceptor();

const apiBaseUrl = new URL(location.toString());
apiBaseUrl.port = '8099';

export const apiClient = new GeneratedApiClient(
	{ BASE: apiBaseUrl.origin },
	AxiosHttpRequest.factory(axiosInstance),
);
