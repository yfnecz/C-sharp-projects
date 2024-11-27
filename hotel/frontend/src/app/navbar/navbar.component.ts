import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../models/user.model';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(private usersService: UsersService, private route:Router) { 
  }

  ngOnInit(): void {

  }

  get isLoggedIn(): boolean {
    return localStorage.getItem('token') !== null;
  }

  logout(){
    this.usersService.logout();
    this.route.navigate(["/"]);
  }
}