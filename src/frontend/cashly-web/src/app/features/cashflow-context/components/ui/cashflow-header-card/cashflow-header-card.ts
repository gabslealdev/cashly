import { Component, input } from '@angular/core';
import { UserCashflowReadModel } from '../../../models/user-cashflow-readmodel';

@Component({
  selector: 'app-cashflow-header-card',
  imports: [],
  templateUrl: './cashflow-header-card.html',
  styleUrl: './cashflow-header-card.scss',
})
export class CashflowHeaderCard {
  cashflow = input.required<UserCashflowReadModel>();
}
