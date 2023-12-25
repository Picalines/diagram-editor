<script lang="ts">
	import type { UserDTO } from '$api/codegen';
	import { Avatar } from '@skeletonlabs/skeleton';

	export let width = 'w-24';

	export let user: UserDTO;

	export let interactive = false;

	const displayNameParts = user.displayName.split(' ');

	const initials =
		displayNameParts.length > 2
			? displayNameParts[0][0] + displayNameParts[1][0]
			: displayNameParts[0].substring(0, 2);
</script>

<Avatar
	{width}
	{initials}
	src={user.avatarUrl ?? undefined}
	border={interactive
		? 'border-4 border-surface-300-600-token hover:!border-primary-500'
		: undefined}
	cursor={interactive ? 'cursor-pointer' : undefined}
/>
