import { Component, signal } from '@angular/core';
import { MenuButton } from '../../components/menu-button/menu-button';
import { Modal } from '../../../../shared/components/modal/modal';
import { CreateCashflowForm } from '../../components/create-cashflow-form/create-cashflow-form';

@Component({
  selector: 'app-dashboard-page',
  imports: [MenuButton, Modal, CreateCashflowForm],
  templateUrl: './dashboard-page.html',
  styleUrl: './dashboard-page.scss',
})
export class DashboardPage {

  isCreateCashflowModalOpen = signal(false);

  openCreateCashflowModal() {
    this.isCreateCashflowModalOpen.set(true);
  }

  closeCreateCashflowModal() {
    this.isCreateCashflowModalOpen.set(false);
  }
}
