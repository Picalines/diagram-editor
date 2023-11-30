<script lang="ts">
	import { apiClient } from '$api';
	import { Maybe } from '$lib/functional';
	import { authTokens, currentUserId, isAuthenticated } from '$lib/stores';

	let loginInput = '';
	let passwordInput = '';

	async function onSubmit() {
		authTokens.set(
			Maybe.some(
				await apiClient.user.postLoginUser({
					requestBody: { login: loginInput, password: passwordInput },
				}),
			),
		);
	}
</script>

<form on:submit|preventDefault={onSubmit}>
	<div>
		<label for="login">Login</label>
		<input name="login" bind:value={loginInput} />
	</div>
	<div>
		<label for="password">Password</label>
		<input name="password" type="password" bind:value={passwordInput} />
	</div>
	<button type="submit">Login</button>
</form>

<div>
	{#if $isAuthenticated}
		Authenticated as #{$currentUserId}
	{:else}
		Not authenticated
	{/if}
</div>
