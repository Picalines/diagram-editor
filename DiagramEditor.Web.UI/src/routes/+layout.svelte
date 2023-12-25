<script lang="ts">
	import '../app.pcss';
	import { goto, invalidateAll } from '$app/navigation';
	import { navigating, page } from '$app/stores';
	import { isAuthenticated } from '$lib/stores';
	import { holdPromise } from '$lib/util';
	import './styles.css';
	import Spinner from '$lib/components/Spinner.svelte';
	import { browser } from '$app/environment';
	import { backOut } from 'svelte/easing';
	import { fly, fade } from 'svelte/transition';

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

	function pageInTransition(node: Element) {
		return browser && $page.url.pathname == '/login'
			? fly(node, { delay: 60, duration: 200, y: 50, easing: backOut })
			: fade(node, { delay: 60, duration: 100 });
	}
</script>

<svelte:head>
	<title>diagram-editor</title>
</svelte:head>

{#key $isAuthenticated}
	{#await holdPromise(500, gotoIfNotLoggedIn('/login'))}
		<div class="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2">
			<Spinner />
		</div>
	{:then}
		{#if $navigating}
			<div class="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2">
				<Spinner />
			</div>
		{:else}
			<div class="h-full w-full" in:pageInTransition|global>
				<slot />
			</div>
		{/if}
	{/await}
{/key}
