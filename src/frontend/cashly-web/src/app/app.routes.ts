import { Routes } from '@angular/router';
import { RegisterUserPage } from './features/identity/pages/register-user-page/register-user-page';

export const routes: Routes = [
    { path: '', redirectTo: 'register/select-profile', pathMatch: 'full' },
    { path: 'register', component: RegisterUserPage }
];
