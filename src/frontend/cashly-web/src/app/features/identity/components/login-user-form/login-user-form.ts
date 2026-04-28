import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { IdentityButton } from '../identity-button/identity-button';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginService } from '../../services/login-service';
import { LoginUserRequest } from '../../models/login-user-request.model';
import { AuthService } from '../../services/auth-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-user-form',
  imports: [IdentityButton, ReactiveFormsModule],
  templateUrl: './login-user-form.html',
  styleUrl: './login-user-form.scss',
})
export class LoginUserForm {
  private readonly formBuilder = inject(FormBuilder);
  private readonly loginService = inject(LoginService);
  private readonly authService = inject(AuthService);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly router = inject(Router)

  protected apiErrorMessage: string | null = null;


  protected loginForm = this.formBuilder.group({
    email: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });

  protected get email() {
    return this.loginForm.get('email');
  }

  protected get password() {
    return this.loginForm.get('password');
  } 
  
  protected onSubmit(): void {
      if(this.loginForm.invalid){
        this.loginForm.markAllAsTouched();
        return;
      }

        const loginRequest: LoginUserRequest = {
          email: this.email?.value ?? '',
          password: this.password?.value ?? ''
        };

        this.loginService.login(loginRequest).subscribe({
          next: (response) => {
            this.authService.saveSession(response);

            console.log("Token salvo:", this.authService.getToken());
            console.log("Expira em:", this.authService.getExpiresAt());

            this.loginForm.reset();

            this.router.navigate(['dashboard'])
          },
          error: (error) => {
            console.error("Erro ao realizar login", error.status);

            if (error.status === 400) {
              this.apiErrorMessage = "Email ou Senha inválidos.";
            } else {
             this.apiErrorMessage = "Ocorreu um erro ao tentar realizar login. Por favor, tente novamente mais tarde.";
            }

            this.cdr.detectChanges();
          }

        });
  };
}
