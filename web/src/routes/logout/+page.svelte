<script lang="ts">
	import { apiClient } from '$api';
	import { Maybe } from '$lib/functional';
	import { authTokens } from '$lib/stores';

	async function logout() {
		await apiClient.user.postLogoutUser();

		authTokens.set(Maybe.none());
	}
</script>

<button on:click={logout}>logout</button>
