<script lang="ts">
	import { apiClient } from '$api';
	import { goto } from '$app/navigation';
	import Spinner from '$lib/components/Spinner.svelte';
	import { Maybe } from '$lib/functional';
	import { authTokens } from '$lib/stores';
	import { holdPromise } from '$lib/util';

	let login = '';
	let password = '';

	let inProgress = false;

	async function onSubmit() {
		inProgress = true;

		try {
			authTokens.set(
				Maybe.some(
					await holdPromise(
						500,
						apiClient.auth.login({
							requestBody: { login, password },
						}),
					),
				),
			);

			await goto('/user/diagrams');
		} finally {
			inProgress = false;
		}
	}
</script>

<div class="flex h-full w-full flex-col items-center justify-center gap-6">
	<h1 class="h1">diagram-editor</h1>
	<form
		class="flex w-96 flex-col gap-6 rounded-2xl bg-surface-800 p-6"
		on:submit|preventDefault={onSubmit}
	>
		<input class="input" type="text" placeholder="login" bind:value={login} />

		<input class="input" type="password" placeholder="password" bind:value={password} />

		<button class="variant-filled btn relative" type="submit" disabled={!login || !password}>
			{#if inProgress}
				<span>&nbsp;</span>
				<div class="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2">
					<Spinner width="w-5" color="rgb(var(--color-secondary-500))" />
				</div>
			{:else}
				Login
			{/if}
		</button>
	</form>
</div>
