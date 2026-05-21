import { Component, effect, inject, input, signal } from '@angular/core';
import { GetCashflowBoardResponse } from '../../models/get-cashflow-board-response.model';
import { CashflowService } from '../../services/cashflow-service';

@Component({
  selector: 'app-cashflow-board',
  imports: [],
  templateUrl: './cashflow-board.html',
  styleUrl: './cashflow-board.scss',
})
export class CashflowBoard {
  private readonly cashflowService = inject(CashflowService)
  cashflowId = input.required<string>()

  protected board = signal<GetCashflowBoardResponse | null>(null);
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

}
