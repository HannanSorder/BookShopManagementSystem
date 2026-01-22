import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(private http: HttpClient, private router: Router) {}

  onLogin() {
    if (!this.username || !this.password) {
      this.errorMessage = 'Please enter username and password';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const loginData = {
      username: this.username,
      password: this.password
    };

    this.http.post('https://localhost:7084/Auth/login', loginData)
      .subscribe({
        next: (response: any) => {
          console.log('Login successful', response);
          // Store user data in localStorage
          localStorage.setItem('user', JSON.stringify(response));
          // Navigate to home page
          this.router.navigate(['/home']);
        },
        error: (error) => {
          console.error('Login failed', error);
          this.isLoading = false;
          if (error.status === 401) {
            this.errorMessage = 'Invalid username or password';
          } else {
            this.errorMessage = 'An error occurred. Please try again.';
          }
        },
        complete: () => {
          this.isLoading = false;
        }
      });
  }
}
