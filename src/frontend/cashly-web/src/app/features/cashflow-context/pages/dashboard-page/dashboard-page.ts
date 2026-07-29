import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { MenuButton } from '../../components/ui/menu-button/menu-button';
import { Modal } from '../../../../shared/components/modal/modal';
import { CreateCashflowForm } from '../../components/create-cashflow-form/create-cashflow-form';
import { CashflowHeaderCard } from "../../components/ui/cashflow-header-card/cashflow-header-card";
import { UserCashflowReadModel } from '../../models/user-cashflow-read-model.model';
import { CashflowService } from '../../services/cashflow-service';
import { CashflowBoard } from '../../components/cashflow-board/cashflow-board';
import { AuthService } from '../../../identity-context/services/auth-service';

@Component({
  selector: 'app-dashboard-page',
  imports: [MenuButton, Modal, CreateCashflowForm, CashflowHeaderCard, CashflowBoard],
  templateUrl: './dashboard-page.html',
  styleUrl: './dashboard-page.scss',
})
export class DashboardPage {
  private readonly cashflowService = inject(CashflowService)
  private readonly authService = inject(AuthService)
  private readonly router = inject(Router)

  selectedCashflow = signal<UserCashflowReadModel | null>(null);
  isAsideExpanded = signal(false);

  toggleAsideMenu(): void {
    if (window.innerWidth < 768) {
      return;
    }

    this.isAsideExpanded.update((isExpanded) => !isExpanded);
  }

  onCashflowSelected(cashflow: UserCashflowReadModel): void {
    this.selectedCashflow.set(cashflow)
  }

  onCashflowSelectedById(event: Event): void {
    const cashflowId = (event.target as HTMLSelectElement).value;
    const cashflow = this.cashflows().find((item) => item.cashflowId === cashflowId);

    if (!cashflow) {
      return;
    }

    this.onCashflowSelected(cashflow);
  }

  ngOnInit(): void {
    this.loadCashflows();
  }

  cashflows = signal<UserCashflowReadModel[]>([]);
  isLoadingCashflows = signal(false);
  errorMessage = signal('');

  private loadCashflows(): void{
    this.isLoadingCashflows.set(true);
    this.errorMessage.set('');

    this.cashflowService.getUserCashflow().subscribe({
      next: (response) => {
        this.cashflows.set(response);
        this.isLoadingCashflows.set(false);
      },
      error: () => {
        this.errorMessage.set('Não foi possível carregar seus cashflows.');
        this.isLoadingCashflows.set(false);
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

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
