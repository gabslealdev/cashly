import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { CreateCashflowRequest } from '../models/create-cashflow-request.model';
import { Observable } from 'rxjs';
import { CreateCashflowResponse } from '../models/create-cashflow-response.model';
import { UserCashflowReadModel } from '../models/user-cashflow-readmodel';

@Injectable({
  providedIn: 'root',
})
export class CashflowService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl 

  createCashflow(request: CreateCashflowRequest): Observable<CreateCashflowResponse>{
    return this.http.post<CreateCashflowResponse>(`${this.apiUrl}/api/cashflows`, request)
  }

  getUserCashflow(): Observable<UserCashflowReadModel[]> {
    return this.http.get<UserCashflowReadModel[]>(`${this.apiUrl}/api/cashflows`)
  }
}


