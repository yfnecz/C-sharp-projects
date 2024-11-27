import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ReservationsService {
  constructor(private http: HttpClient) { }

  getReservationsOfLoggedInUser(token: string): Observable<any> {
    return this.http
      .get('/getAllReservations/' + token)
      .pipe(catchError((error: any) => throwError(() => error)));
  }

  getAvailableRooms(checkInDate: any, checkOutDate: any): Observable<any> {
    return this.http
      .get(`/getAvailableRooms/${checkInDate}to${checkOutDate}`)
      .pipe(catchError((error: any) => throwError(() => error)));
  }

  reserve(userId: number, roomId: number, checkInDate: any, checkOutDate: any, bill: number): Observable<any> {
    return this.http.post('/reserve', {userId, roomId, checkInDate, checkOutDate, bill})
      .pipe(
        catchError((error: any) => throwError(() => error))
      );
  }
}