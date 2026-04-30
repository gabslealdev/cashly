import { Component } from '@angular/core';
import { IdentityShell } from '../../components/identity-shell/identity-shell';
import { RegisterUserForm } from '../../components/register-user-form/register-user-form';

@Component({
  selector: 'app-register-user-page',
  imports: [IdentityShell,RegisterUserForm],
  templateUrl: './register-user-page.html',
  styleUrl: './register-user-page.scss',
})
export class RegisterUserPage {

}
