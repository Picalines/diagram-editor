<script lang="ts">
	import { apiClient } from '$api';
	import Spinner from '$lib/components/Spinner.svelte';
	import { currentUser } from '$lib/stores';
	import { holdPromise } from '$lib/util';
	import DiagramCard from './DiagramCard.svelte';
</script>

{#await holdPromise(500, apiClient.userDiagrams.getOwnedDiagrams())}
	<div class="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2">
		<Spinner />
	</div>
{:then { diagrams }}
	<div class="flex flex-wrap gap-4 p-4">
		{#await $currentUser then user}
			{#each diagrams as diagram}
				<DiagramCard {diagram} user={user.unwrap()} />
			{/each}
		{/await}
	</div>
{/await}
