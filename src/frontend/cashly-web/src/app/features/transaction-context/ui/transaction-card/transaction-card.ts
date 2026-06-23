import { Component, input } from '@angular/core';
import { CashflowBoardTransactionResponse } from '../../../cashflow-context/models/get-cashflow-board-response.model';
import { LucideEllipsis } from '@lucide/angular';
import { CurrencyPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'app-transaction-card',
  imports: [LucideEllipsis, CurrencyPipe, DatePipe],
  templateUrl: './transaction-card.html',
  styleUrl: './transaction-card.scss',
})
export class TransactionCard {
  transaction = input.required<CashflowBoardTransactionResponse>();
}
