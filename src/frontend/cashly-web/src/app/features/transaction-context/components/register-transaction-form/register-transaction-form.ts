import { Component, inject, input, output, signal } from '@angular/core';
import { IdentityButton } from '../../../identity-context/components/identity-button/identity-button';
import { TransactionService } from '../../services/transaction-service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RegisterTransactionRequest } from '../../models/register-transaction-request.model';

@Component({
  selector: 'app-register-transaction-form',
  imports: [IdentityButton, ReactiveFormsModule],
  templateUrl: './register-transaction-form.html',
  styleUrl: './register-transaction-form.scss',
})
export class RegisterTransactionForm {
  cashflowId = input.required<string>();
  transactionRegistered = output<void>();
  closeModal = output<void>();

  private readonly transactionService = inject(TransactionService);
  private readonly formBuilder = inject(FormBuilder);
  protected isSubmitting = signal(false);
  protected errorMessage = signal('');


  protected registerTransactionForm = this.formBuilder.group({
    type: ['', [Validators.required]],
    title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    amount: ['', [Validators.required]],
    date: ['', [Validators.required]],
    status: ['Scheduled', [Validators.required]]
  });



  protected formatAmountOnBlur(): void {
    const amountControl = this.registerTransactionForm.controls.amount;
    const amount = this.parseAmount(amountControl.value);

    if (amount === null) {
      amountControl.setValue('');
      return;
    }

    amountControl.setValue(this.formatAmount(amount));
  }

  protected getAmountAsNumber(): number | null {
    return this.parseAmount(this.registerTransactionForm.controls.amount.value);
  }

  protected onSubmit(): void {
    this.errorMessage.set('');

    if (this.registerTransactionForm.invalid) {
      this.registerTransactionForm.markAllAsTouched();
      return;
    }

    const amount = this.getAmountAsNumber();

    if (amount === null) {
      this.registerTransactionForm.controls.amount.setErrors({ invalidAmount: true });
      return;
    }

    const formValue = this.registerTransactionForm.getRawValue();

    const request: RegisterTransactionRequest = {
      title: formValue.title!,
      amount,
      type: formValue.type!,
      date: new Date(`${formValue.date}T00:00:00`),
      status: formValue.status!,
    };

    this.isSubmitting.set(true);

    this.transactionService.registerTransaction(request, this.cashflowId()).subscribe({
      next: () => {
        this.transactionRegistered.emit();
        this.closeModal.emit();
        this.registerTransactionForm.reset({
          type: '',
          title: '',
          amount: '',
          date: '',
          status: 'Scheduled',
        });
        this.isSubmitting.set(false);
      },
      error: () => {
        this.errorMessage.set('Não foi possível registrar a transação.');
        this.isSubmitting.set(false);
      }
    });
  }

  private parseAmount(value: string | null | undefined): number | null {
    if (!value) {
      return null;
    }

    const normalizedValue = value
      .trim()
      .replace(/\./g, '')
      .replace(',', '.');

    const amount = Number(normalizedValue);

    if (!Number.isFinite(amount) || amount <= 0) {
      return null;
    }

    return amount;
  }

  private formatAmount(amount: number): string {
    return amount.toLocaleString('pt-BR', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    });
  }

}
