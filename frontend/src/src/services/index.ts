import itemController from './items';
import authController from './auth'
import categoryController from './categories';

export const API = {
    items: itemController,
    auth: authController,
    categories: categoryController
};
