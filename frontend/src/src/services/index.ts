import itemController from './items';
import authController from './auth'
import categoryController from './categories';
import companyController from './companies';
import scheduleController from './schedules'
import userController from './user';
import employeeController from './employees'

export const API = {
    companies: companyController,
    items: itemController,
    auth: authController,
    employees: employeeController,
    categories: categoryController,
    schedules: scheduleController,
    users: userController
};
