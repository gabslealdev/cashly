import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { RegisterTransactionRequest } from '../models/register-transaction-request.model';
import { Observable } from 'rxjs';
import { RegisterTransactionResponse } from '../models/register-transaction-response.model';

@Injectable({
  providedIn: 'root',
})
export class TransactionService {
    private readonly http = inject(HttpClient)
    private readonly apiUrl = environment.apiUrl

    registerTransaction(request: RegisterTransactionRequest, cashflowId: string): Observable<RegisterTransactionResponse>{
      return this.http.post<RegisterTransactionResponse>(`${this.apiUrl}/api/transaction/${cashflowId}/add`, request)
    }

}
