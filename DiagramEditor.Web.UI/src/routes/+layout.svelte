<script lang="ts">
	import { goto, invalidateAll } from '$app/navigation';
	import { navigating, page } from '$app/stores';
	import { isAuthenticated } from '$lib/stores';
	import { holdPromise } from '$lib/util';
	import './styles.css';

	const publicRoutes = ['/login', '/register'];

	function isAuthorized() {
		return $isAuthenticated || publicRoutes.includes($page.url.pathname);
	}

	async function gotoIfNotLoggedIn(defaultRoute: string) {
		if (!isAuthorized()) {
			await invalidateAll();
			await goto(defaultRoute);
		}
	}
</script>

<svelte:head>
	<title>diagram-editor</title>
</svelte:head>

{#key $isAuthenticated}
	{#await holdPromise(500, gotoIfNotLoggedIn('/login'))}
		Loading...
	{:then}
		<div id="app">
			{#if $navigating}
				Navigating...
			{:else}
				<main>
					<slot />
				</main>
			{/if}
		</div>
	{/await}
{/key}
