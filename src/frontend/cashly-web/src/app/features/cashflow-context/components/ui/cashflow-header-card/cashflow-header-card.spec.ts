import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CashflowHeaderCard } from './cashflow-header-card';

describe('CashflowHeaderCard', () => {
  let component: CashflowHeaderCard;
  let fixture: ComponentFixture<CashflowHeaderCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CashflowHeaderCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CashflowHeaderCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
