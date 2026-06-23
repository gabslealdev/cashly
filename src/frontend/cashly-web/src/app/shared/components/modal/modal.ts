import { Component, input, output } from '@angular/core';
import { CreateCashflowForm } from "../../../features/cashflow-context/components/create-cashflow-form/create-cashflow-form";
import { LucideX } from '@lucide/angular';

@Component({
  selector: 'app-modal',
  imports: [LucideX],
  templateUrl: './modal.html',
  styleUrl: './modal.scss',
})
export class Modal {
isOpen = input(false);
title = input('');

closed = output<void>();

close(){
  this.closed.emit();
}
}
