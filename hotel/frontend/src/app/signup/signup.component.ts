import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  registerForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private usersService: UsersService,
    private router : Router

  ) { }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(4)]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {
    if (this.registerForm.invalid) {
      return;
    }
    const username = this.registerForm.get('username')?.value;
    const password = this.registerForm.get('password')?.value;

    this.usersService.register(username, password).subscribe({
      next: (response: any) => {
        // Registration success
        window.alert('Registration successful');            
        this.router.navigate(['/login']);
      },
      error: (error: any) => {
        // Registration fail
        window.alert('Please enter valid details to register yourself.');
      },
    });
  }
}