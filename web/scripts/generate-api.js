import path from 'path';
import fs from 'fs-extra';
import { generate } from '@devexperts/swagger-codegen-ts';
import { serialize as serializeOpenAPI3 } from '@devexperts/swagger-codegen-ts/dist/language/typescript/3.0/index.js';
import { OpenapiObjectCodec } from '@devexperts/swagger-codegen-ts/dist/schema/3.0/openapi-object.js';
import { either } from 'fp-ts';

const cwd = path.resolve('src/api');
const specName = 'swagger.json';
const spec = path.resolve(cwd, specName);
const out = path.resolve(cwd, 'codegen');

const codegenTask = generate({
	cwd,
	spec,
	out,
	language: serializeOpenAPI3,
	decoder: OpenapiObjectCodec,
});

codegenTask().then(
	either.match(
		error => {
			console.error('Code generation failed', error);
			process.exit(1);
		},
		async () => {
			const tempPath = path.resolve(cwd, '_');
			await fs.move(path.resolve(out, specName), tempPath);
			await fs.rm(out, { recursive: true });
			await fs.move(tempPath, out);

			console.log('Generated successfully');
		},
	),
);
