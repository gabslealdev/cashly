import { Injectable } from '@angular/core';
import { LoginUserResponse } from '../models/login-user-response.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly tokenKey = 'accessToken';
  private readonly expiresAtKey = 'expiresAt';

  public saveSession(response: LoginUserResponse): void{
    localStorage.setItem(this.tokenKey, response.accessToken);
    localStorage.setItem(this.expiresAtKey, response.expiresAt);
  }

  public getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  public getExpiresAt(): string | null {
    return localStorage.getItem(this.expiresAtKey);
  }

  public logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.expiresAtKey);
  }

  public isAuthenticated(): boolean {
    const token = this.getToken();
    const expiresAt = this.getExpiresAt();
    
    if (!token || !expiresAt) {
      return false;
    }

    const isTokenValid = new Date(expiresAt) > new Date();

    if(!isTokenValid){
      this.logout();
      return false;
    }

    return true;
  }

}
