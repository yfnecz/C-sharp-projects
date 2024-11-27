import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  constructor(private router: Router, private http: HttpClient) {}

  register(username: string, password: string): Observable<any> {
    return this.http
      .post(
        '/register',
        { username, password },
        {
          responseType: 'text', // Set the response type to text
        }
      )
      .pipe(catchError((error: any) => throwError(() => error)));
  }

  login(username: string, password: string): Observable<any> {
    return this.http
      .post('/login', { username, password })
      .pipe(catchError((error: any) => throwError(() => error)));
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    this.router.navigate(['/']);
  }

  getUser(id: string): Observable<any> {
    return this.http
      .get(`/getUserById/${id}`, {
        responseType: 'text', // Set the response type to text
      })
      .pipe(catchError((error: any) => throwError(() => error)));
  }
}