import { Routes } from '@angular/router';
import { RegisterUserPage } from './features/identity/pages/register-user-page/register-user-page';
import { LoginUserPage } from './features/identity/pages/login-user-page/login-user-page';

export const routes: Routes = [
    { path: '', redirectTo: 'register', pathMatch: 'full' },
    { path: 'register', component: RegisterUserPage },
    { path: 'login', component: LoginUserPage },
];
