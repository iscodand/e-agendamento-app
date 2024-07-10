import itemController from './items';
import authController from './auth'
import categoryController from './categories';
import companyController from './companies';

export const API = {
    companies: companyController,
    items: itemController,
    auth: authController,
    categories: categoryController
};
