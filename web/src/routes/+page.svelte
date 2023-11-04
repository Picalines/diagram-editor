<script>
	import { WeatherForecastService } from '$api/codegen';
</script>

<svelte:head>
	<title>diagram-editor</title>
</svelte:head>

<div>
	{#await WeatherForecastService.getWeatherForecast() then forecast}
		<pre>{JSON.stringify(forecast, null, 4)}</pre>
	{/await}
</div>
