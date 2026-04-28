import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { CreateCashflowRequest } from '../models/create-cashflow-request.model';
import { Observable } from 'rxjs';
import { CreateCashflowResponse } from '../models/create-cashflow-response.model';

@Injectable({
  providedIn: 'root',
})
export class CreateCashflowService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl 

  createCashflow(request: CreateCashflowRequest): Observable<CreateCashflowResponse>{
    return this.http.post<CreateCashflowResponse>(`${this.apiUrl}/api/cashflows`, request)
  }
}
