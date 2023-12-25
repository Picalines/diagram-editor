<script>
	import { page } from '$app/stores';
	import Spinner from '$lib/components/Spinner.svelte';
	import { Maybe } from '$lib/functional';
	import { authTokens, currentUser } from '$lib/stores';
	import { AppRail, AppRailAnchor } from '@skeletonlabs/skeleton';

	async function logout() {
		authTokens.set(Maybe.none());
	}
</script>

<div class="flex h-full flex-row">
	<section class="h-full">
		<AppRail>
			<AppRailAnchor href="/user" selected={$page.url.pathname === '/user'}>
				<i class="fa-solid fa-user text-2xl"></i>
			</AppRailAnchor>
			<AppRailAnchor href="/user/diagrams" selected={$page.url.pathname === '/user/diagrams'}>
				<i class="fa-solid fa-diagram-project text-2xl"></i>
			</AppRailAnchor>

			<svelte:fragment slot="trail">
				<AppRailAnchor href="/logout" on:click={logout}>
					<i class="fa-solid fa-circle-arrow-right text-2xl"></i>
				</AppRailAnchor>
			</svelte:fragment>
		</AppRail>
	</section>

	<section class="relative h-full flex-grow">
		{#await $currentUser}
			<Spinner />
		{:then user}
			{#if user.isSome}
				<slot />
			{:else}
				User not found
			{/if}
		{/await}
	</section>
</div>
