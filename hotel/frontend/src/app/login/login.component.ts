import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ReservationsService } from '../services/reservations.service';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  logInForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private usersService: UsersService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.logInForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(4)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }
  onSubmit() {
    if (this.logInForm.invalid) {
      return;
    }
    const username = this.logInForm.get('username')?.value;
    const password = this.logInForm.get('password')?.value;

    this.usersService.login(username, password).subscribe({
      next: (response: any) => {
        localStorage.setItem('token', response.token);
        localStorage.setItem('userId', response.id);
        window.alert('Login successful');
        this.router.navigate(['/']);
      },
      error: (error: any) => {
        // Login failed
        window.alert('Log in details are invalid. Please try again.');
      },
    });
  }
}