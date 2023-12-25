<script>
	import { currentUser } from '$lib/stores';
	import UserAvatar from './UserAvatar.svelte';
</script>

<div class="flex w-max flex-col gap-4 p-4">
	{#await $currentUser then userMaybe}
		{@const user = userMaybe.unwrap()}

		<div class="flex w-full justify-center">
			<UserAvatar {user} interactive />
		</div>

		<input class="input" type="text" value={user.login} />

		<input class="input" type="text" value={user.displayName} />

		<button class="variant-filled btn">Save</button>
	{/await}
</div>
