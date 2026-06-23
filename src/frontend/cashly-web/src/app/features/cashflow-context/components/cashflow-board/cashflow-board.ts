import { Component, effect, inject, input, signal } from '@angular/core';
import { CashflowService } from '../../services/cashflow-service';
import { TransactionCard } from '../../../transaction-context/ui/transaction-card/transaction-card';
import { CurrencyPipe } from '@angular/common';
import { LucideLockKeyhole, LucideLockKeyholeOpen } from '@lucide/angular';
import { CashflowHeaderBoardButton } from '../ui/cashflow-header-board-button/cashflow-header-board-button';
import { Modal } from '../../../../shared/components/modal/modal';
import { RegisterTransactionForm } from '../../../transaction-context/components/register-transaction-form/register-transaction-form';
import { GetCashflowBoardResponse } from '../../models/get-cashflow-board-response.model';

@Component({
  selector: 'app-cashflow-board',
  imports: [TransactionCard, CurrencyPipe, LucideLockKeyhole, 
    LucideLockKeyholeOpen, CashflowHeaderBoardButton, Modal,
    RegisterTransactionForm],
  templateUrl: './cashflow-board.html',
  styleUrl: './cashflow-board.scss',
})
export class CashflowBoard {
  private readonly cashflowService = inject(CashflowService)
  cashflowId = input.required<string>()

  protected board = signal<GetCashflowBoardResponse | null>(null)
  protected isLoading = signal(false);
  protected errorMessage = signal('');

  constructor(){
    effect(() => {
      const cashflowId = this.cashflowId();
      this.loadBoard(cashflowId);
    });
  }

  private loadBoard(cashflowId: string): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.cashflowService.getCashflowBoard(cashflowId).subscribe({
      next: (response) => {
        this.board.set(response);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('Não foi possível carregar o cashflow');
        this.isLoading.set(false);
      }
    });
  }

  isRegisterTransactionModal = signal(false);

  openRegisterTransactionModal(){
    this.isRegisterTransactionModal.set(true);
  }

  closeRegisterTransactionModal(){
    this.isRegisterTransactionModal.set(false);
  }

  protected onTransactionRegistered(): void {
    this.loadBoard(this.cashflowId());
  }

}
