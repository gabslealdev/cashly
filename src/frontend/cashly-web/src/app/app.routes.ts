import { Routes } from '@angular/router';
import { RegisterUserPage } from './features/identity-context/pages/register-user-page/register-user-page';
import { LoginUserPage } from './features/identity-context/pages/login-user-page/login-user-page';
import { DashboardPage } from './features/cashflow-context/pages/dashboard-page/dashboard-page';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'register', component: RegisterUserPage },
    { path: 'login', component: LoginUserPage },
    { path: 'dashboard', canActivate: [authGuard],
        loadComponent: () => 
            import('./features/cashflow-context/pages/dashboard-page/dashboard-page')
            .then(c => c.DashboardPage)
    },
];
