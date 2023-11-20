import { userController as createUserController } from './codegen/paths/UserController';
import { fetchHttpClient } from './fetchHttpClient';

export const userController = createUserController({ httpClient: fetchHttpClient });

