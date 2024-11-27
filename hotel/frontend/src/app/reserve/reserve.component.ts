import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Room } from '../models/room.model';
import { ReservationsService } from '../services/reservations.service';
import { UsersService } from '../services/users.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reserve',
  templateUrl: './reserve.component.html',
  styleUrls: ['./reserve.component.css'],
})
export class ReserveComponent implements OnInit {
  reserveForm!: FormGroup;
  //Parameter to show the complete reserve page to only logged-in users
  showPage = true;
  // Parameter to show the invoice if book now is clicked
  showInvoice = false;

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
  checkin:any;
  checkout:any;
  availableRooms:Array<Room>= [];

  constructor(
    private usersService: UsersService,
    private reservationsService: ReservationsService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {

  }

  ngOnInit(): void {

    this.token = localStorage.getItem('token');
    if(!this.token) {
      this.showPage = false;
      window.alert("Please login to reserve a room")
      this.router.navigate(['/login']);
    }
    this.userid = Number(localStorage.getItem('userId'));
    console.log(localStorage.getItem('token'), this.userid);
    this.reserveForm = this.formBuilder.group({
      checkInDate: ['', Validators.required],
      checkOutDate: ['', Validators.required],
    });

  }
  getAvailableRooms(){
    this.availableRooms = [];
    const checkInDate = this.reserveForm.get('checkInDate')?.value;
    const checkOutDate = this.reserveForm.get('checkOutDate')?.value;

    this.reservationsService.getAvailableRooms(checkInDate, checkOutDate).subscribe({
    next: (response) => {   
        for(let res of response){
          this.availableRooms.push({
            "Id": res.id,
            "Type":res.type,
            "Capacity":res.capacity,
            "RatePerDay": res.ratePerNight
          })
        }
        console.log(this.availableRooms);
  },
    error: (error) => window.alert("An unexpected error occurred while fetching available rooms!"),

  })

  }

  roomAvailable(id:number, capacity:number,ratePerDay:number, type:string){
    window.scrollTo({
      top: 0,
      behavior: 'smooth' // You can also use 'auto' for instant scrolling
    });

    this.checkin = this.reserveForm.get('checkInDate')?.value;
    this.checkout = this.reserveForm.get('checkOutDate')?.value;
    const diffInMilliseconds = new Date(this.checkout).valueOf() - new Date(this.checkin).valueOf();
    this.totalNights = Math.floor(diffInMilliseconds / (1000 * 60 * 60 * 24));
    this.showInvoice = true
    this.capacity = capacity;
    this.ratePerNight = ratePerDay;
    this.type = type;
    this.roomNo = id;
    this.totalRoomCost = this.ratePerNight * this.totalNights;
    this.taxes = 0.16 * this.totalRoomCost;
    this.extraCharges = 0.10 * this.totalRoomCost;
    this.totalBill = this.totalRoomCost + this.taxes + this.extraCharges;        
  }

  close(){
    this.showInvoice = false;
  }

  confirmReserve(){
    const checkInDate = this.reserveForm.get('checkInDate')?.value ;
    const checkOutDate = this.reserveForm.get('checkOutDate')?.value ;
    this.reservationsService.reserve(this.userid,this.roomNo,checkInDate, checkOutDate, this.totalBill).subscribe(
    (response)=>{
      console.log(response)
      this.availableRooms=[]
        window.alert("Your reservation has been confirmed")
        this.router.navigate(["/dashboard"])
        this.showInvoice = false;
      },
      error => {
        window.alert("An unexpected error occurred while reserving a room! Please re-check the entered details!")
      }
    )
  }
}