<script>
	import { apiClient } from '$api';
</script>

{#await apiClient.user.getCurrentUser()}
	Fetching...
{:then user}
	<pre>{JSON.stringify(user, null, 2)}</pre>
{:catch error}
	Error!
	<pre>{JSON.stringify(error, null, 2)}</pre>
{/await}
