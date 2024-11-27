import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Room } from '../models/room.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerForm!: FormGroup;
  showRooms = false;
  reserve = false;
  ratePerNight : number = 0;
  taxes = 0;
  totalRoomCost = 0;
  capacity!:number;
  totalNights :any;
  totalBill =0;
  type!:string
  extraCharges = 0; 
  roomNo = 0;
  token:any;
  userid:any;
  // checkin:any = null;
  // checkout:any = null;
  availableRooms:Array<Room>= [];

  constructor(
  ) {
    this.token = localStorage.getItem('token');
    this.userid = Number(localStorage.getItem('userId'));
    console.log(localStorage.getItem('token'), this.userid);

    // this.token = (localStorage.getItem('token'))

  }
  isSignIn = false;
  isLogIn = false;

  ngOnInit(): void {
// this.reservationsService.rooms("Deluxe Super",2, 900).subscribe((res)=>{
//   console.log(Response)
// })


  }


}
