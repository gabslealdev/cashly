import { Component, inject, signal } from '@angular/core';
import { MenuButton } from '../../components/ui/menu-button/menu-button';
import { Modal } from '../../../../shared/components/modal/modal';
import { CreateCashflowForm } from '../../components/create-cashflow-form/create-cashflow-form';
import { CashflowHeaderCard } from "../../components/ui/cashflow-header-card/cashflow-header-card";
import { UserCashflowReadModel } from '../../models/user-cashflow-readmodel';
import { CashflowService } from '../../services/cashflow-service';

@Component({
  selector: 'app-dashboard-page',
  imports: [MenuButton, Modal, CreateCashflowForm, CashflowHeaderCard],
  templateUrl: './dashboard-page.html',
  styleUrl: './dashboard-page.scss',
})
export class DashboardPage {
  private readonly cashflowService = inject(CashflowService)

  ngOnInit(): void {
    this.loadCashflows();
  }

  cashflows = signal<UserCashflowReadModel[]>([]);
  errorMessage = signal('');

  private loadCashflows(): void{
    this.errorMessage.set('');

    this.cashflowService.getUserCashflow().subscribe({
      next: (response) => {
        this.cashflows.set(response)
      },
      error: (error) => {
        this.errorMessage.set('Não foi possível carregar seus cashflows.');
      }
    });
  }

  isCreateCashflowModalOpen = signal(false);

  openCreateCashflowModal() {
    this.isCreateCashflowModalOpen.set(true);
  }

  closeCreateCashflowModal() {
    this.isCreateCashflowModalOpen.set(false);
  }

  protected onCashflowCreated(): void {
    this.loadCashflows();
  }
}
