<script lang="ts">
	import type { DiagramDTO, UserDTO } from '$api/codegen';
	import UserAvatar from '../UserAvatar.svelte';

	export let diagram: DiagramDTO;

	export let user: UserDTO;
</script>

<div class="text-token grid flex-shrink-0">
	<a class="bg-initial card card-hover overflow-hidden" href="/user/diagrams/{diagram.id}">
		<div class="space-y-4 p-4">
			<h3 class="h3" data-toc-ignore>{diagram.name}</h3>
			<article>
				<p>
					{diagram.description}
				</p>
			</article>
		</div>
		<hr class="opacity-50" />
		<footer class="flex items-center justify-start space-x-4 p-4">
			<UserAvatar width="w-8" {user} />

			<div class="flex flex-auto items-center justify-between">
				<h6 class="font-bold" data-toc-ignore>{user.displayName}</h6>
				<small>Updated {new Date(diagram.updatedDate).toLocaleDateString()}</small>
			</div>
		</footer>
	</a>
</div>
