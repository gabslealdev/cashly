import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { LoginUserRequest } from '../models/login-user-request.model';
import { Observable } from 'rxjs';
import { LoginUserResponse } from '../models/login-user-response.model';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  login(request: LoginUserRequest): Observable<LoginUserResponse>{
    return this.http.post<LoginUserResponse>(`${this.apiUrl}/api/login`, request);
  }
}
