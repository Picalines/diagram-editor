<script lang="ts">
	import { userController } from '$api';

	let loginInput = '';
	let passwordInput = '';

	async function onSubmit(event: SubmitEvent) {
		event.preventDefault();

		const keys = await userController.postLoginUser({
			body: { login: loginInput, password: passwordInput },
		});

		console.log(keys);
	}
</script>

<form on:submit={onSubmit}>
	<input bind:value={loginInput} />
	<input bind:value={passwordInput} />
</form>
