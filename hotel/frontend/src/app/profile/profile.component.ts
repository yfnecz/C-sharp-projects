import { Component, OnInit } from '@angular/core';
import { Reservation } from '../models/reservtion.model';
import { ReservationsService } from '../services/reservations.service';
import { UsersService } from '../services/users.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  id: any;
  username: any;
  token: any;
  serialNo = 0;
  dataSource: any;
  reservations: Array<Reservation> = [];
  noReservation = false;
  displayedColumns: string[] = ['Id', 'Room Id', 'CheckInDate', 'CheckOutDate'];

  constructor(
    private reservationsService: ReservationsService,
    private usersService: UsersService,
    private router: Router
  ) {
    this.id = localStorage.getItem('userId');
    this.token = localStorage.getItem('token');
  }

  ngOnInit(): void {
    if (!this.token) {
      window.alert('Please login first to access this page!');
      this.router.navigate(['/login']);
    } else {
      this.reservationsService
        .getReservationsOfLoggedInUser(this.token)
        .subscribe(
          (response) => {
            for (let res of response) {
              this.serialNo + 1
              this.reservations.push({
                Id: this.serialNo,
                CheckInDate: res.checkInDate,
                CheckOutDate: res.checkOutDate,
                RoomId: res.roomId,
                Bill: res.bill,
                UserId: res.userId,
              });
              this.serialNo = this.serialNo + 1;
            }
            this.dataSource = this.reservations;
          },
          (error) => {
            if (error.error == null) {
              window.alert('Please login to view your dashboard');
            }
            window.alert(error.error);
          }
        );

      this.usersService.getUser(this.id).subscribe(
        (response) => {
          this.username = response;
        },
        (error) => {
          window.alert(error.error);
        }
      );
    }
  }
}