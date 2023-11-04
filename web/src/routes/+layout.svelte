<script lang="ts">
	import './styles.css';
	import { OpenAPI } from '$api/codegen';

	// TODO: environment variable?
	OpenAPI.BASE = 'http://localhost:8080';
</script>

<div id="app">
	<main>
		<slot />
	</main>
</div>

