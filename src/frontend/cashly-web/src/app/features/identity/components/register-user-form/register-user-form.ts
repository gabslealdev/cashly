import { Component, inject } from '@angular/core';
import { IdentityButton } from '../identity-button/identity-button';
import { FormBuilder, ReactiveFormsModule, Validators, AbstractControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RegisterUserRequest } from '../../models/register-user-request.model';
import { IdentityService } from '../../services/identity-service';



@Component({
  selector: 'app-register-user-form',
  imports: [CommonModule, IdentityButton, ReactiveFormsModule, IdentityButton],
  templateUrl: './register-user-form.html',
  styleUrl: './register-user-form.scss',
})
export class RegisterUserForm {
  private readonly formBuilder = inject(FormBuilder);
  private identityService = inject(IdentityService);

  protected registerForm = this.formBuilder.group({
    firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(80)]],
    lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(80)]],
    email: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(255)]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required, Validators.minLength(8)]]
  }); 

  protected onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const registerRequest: RegisterUserRequest = {
      firstName: this.firstName?.value ?? '',
      lastName: this.lastName?.value ?? '',
      email: this.email?.value ?? '',
      password: this.password?.value ?? ''
    };

    this.identityService.registerUser(registerRequest).subscribe({
      next: (response) => {
        console.log("Usuário registrado com sucesso", response)
        this.registerForm.reset();
      },
      error: (error) => {
        console.error('Erro ao registrar usuário:', error)
      }
    });


  }

  protected get firstName(){
    return this.registerForm.get('firstName');
  }

    protected get lastName(){
    return this.registerForm.get('lastName');
  }

    protected get email(){
    return this.registerForm.get('email');
  }

    protected get password(){
    return this.registerForm.get('password');
  }

    protected get confirmPassword(){
    return this.registerForm.get('confirmPassword');
  }

  protected passwordsMatch(): boolean {
    const password = this.password?.value; 
    const confirmPassoword = this.confirmPassword?.value;

    if (!password || !confirmPassoword){
      return true;
    }

    return password === confirmPassoword;
  }
}
