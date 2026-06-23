import { Component, input, output } from '@angular/core';
import { GetCashflowBoardResponse } from '../../../models/get-cashflow-board-response.model';

@Component({
  selector: 'app-cashflow-header-board-button',
  imports: [],
  templateUrl: './cashflow-header-board-button.html',
  styleUrl: './cashflow-header-board-button.scss',
})
export class CashflowHeaderBoardButton {
  label = input.required<string>();
  board = input.required<GetCashflowBoardResponse | null>();

  clicked = output<void>();

  onClick() {
    this.clicked.emit();
  }
}
