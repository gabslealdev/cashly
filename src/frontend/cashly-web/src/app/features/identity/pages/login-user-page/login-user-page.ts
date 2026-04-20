import { Component } from '@angular/core';
import { IdentityShell } from '../../components/identity-shell/identity-shell';
import { LoginUserForm } from '../../components/login-user-form/login-user-form';

@Component({
  selector: 'app-login-user-page',
  imports: [IdentityShell, LoginUserForm],
  templateUrl: './login-user-page.html',
  styleUrl: './login-user-page.scss',
})
export class LoginUserPage {

}
