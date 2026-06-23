import { Component, input, output } from '@angular/core';
import { UserCashflowReadModel } from '../../../models/user-cashflow-read-model.model';

@Component({
  selector: 'app-cashflow-header-card',
  imports: [],
  templateUrl: './cashflow-header-card.html',
  styleUrl: './cashflow-header-card.scss',
})
export class CashflowHeaderCard {
  cashflow = input.required<UserCashflowReadModel>();
  isSelected = input(false);
  selected = output<UserCashflowReadModel>();

  selectCashflow(): void {
    this.selected.emit(this.cashflow());
  }
}
