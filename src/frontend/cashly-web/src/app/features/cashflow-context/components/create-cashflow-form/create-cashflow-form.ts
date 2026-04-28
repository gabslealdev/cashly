import { Component, inject } from '@angular/core';
import { IdentityButton } from "../../../identity/components/identity-button/identity-button";
import { FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import { CreateCashflowService } from '../../services/create-cashflow-service';
import { CreateCashflowRequest } from '../../models/create-cashflow-request.model';

@Component({
  selector: 'app-create-cashflow-form',
  imports: [IdentityButton, ReactiveFormsModule],
  templateUrl: './create-cashflow-form.html',
  styleUrl: './create-cashflow-form.scss',
})
export class CreateCashflowForm {
  private readonly formBuilder = inject(FormBuilder);
  private readonly cashflowCreateService = inject(CreateCashflowService)

  protected form = this.formBuilder.group({
    title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]]
  });

  protected onSubmit(): void {
    if (this.form.invalid){
      this.form.markAllAsTouched();
      return;
    }

    const request: CreateCashflowRequest = {
      title: this.title?.value ?? '',
    }

    this .cashflowCreateService.createCashflow(request).subscribe({
      next: (response) => {
        console.log(response)
        this.form.reset();
      },
      error: (error) => {
        console.log('Erro ao registrar usuário', error)
      }
    });
  }

  protected get title(){
    return this.form.get('title');
  }

}
