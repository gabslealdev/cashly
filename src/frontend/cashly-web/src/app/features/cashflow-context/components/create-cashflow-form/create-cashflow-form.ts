import { Component, inject, output } from '@angular/core';
import { IdentityButton } from "../../../identity-context/components/identity-button/identity-button";
import { FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import { CashflowService } from '../../services/cashflow-service';
import { CreateCashflowRequest } from '../../models/create-cashflow-request.model';

@Component({
  selector: 'app-create-cashflow-form',
  imports: [IdentityButton, ReactiveFormsModule],
  templateUrl: './create-cashflow-form.html',
  styleUrl: './create-cashflow-form.scss',
})
export class CreateCashflowForm {
  cashflowCreated = output<void>();
  closeModal = output<void>();

  private readonly formBuilder = inject(FormBuilder);
  private readonly cashflowService = inject(CashflowService)


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

    this.cashflowService.createCashflow(request).subscribe({
      next: (response) => {
        console.log(response)
        this.form.reset();
        this.cashflowCreated.emit()
        this.closeModal.emit()
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
