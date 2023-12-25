<script lang="ts">
	import { ConicGradient, type ConicStop } from '@skeletonlabs/skeleton';

	export let width = 'w-24';

    export let color = 'rgb(var(--color-primary-500))'

	const spinnerConicStops: ConicStop[] = [
		{ color: 'transparent', start: 0, end: 25 },
		{ color, start: 100, end: 100 },
	];
</script>

<ConicGradient {width} stops={spinnerConicStops} spin />
