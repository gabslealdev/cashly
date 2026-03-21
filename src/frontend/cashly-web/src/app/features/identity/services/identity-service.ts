import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { RegisterUserRequest } from '../models/register-user-request.model';
import { Observable } from 'rxjs';
import { RegisterUserResponse } from '../models/register-user-response.model';

@Injectable({
  providedIn: 'root',
})
export class IdentityService {
  private readonly http = inject(HttpClient)
  private readonly apiUrl = environment.apiUrl

  registerUser(request: RegisterUserRequest): Observable<RegisterUserResponse> {
    return this.http.post<RegisterUserResponse>(`${this.apiUrl}/api/identity/user`, request);
  }
}
