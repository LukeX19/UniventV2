import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { AuthenticationService } from '../../../core/services/authentication.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { LoginResponse } from '../../../shared/models/authenticationModel';
import { TokenService } from '../../../core/services/token.service';
import { MatDialog } from '@angular/material/dialog';
import { InfoDialogComponent } from '../../../shared/components/info-dialog/info-dialog.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    RouterModule,
    MatCardModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthenticationService);
  private snackbarService = inject(SnackbarService);
  private router = inject(Router);
  private tokenService = inject(TokenService);
  private dialog = inject(MatDialog);

  loginForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  hidePassword = true;

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }

  getErrorMessage(field: string, errors: any): string {
    if (!errors)
      return '';
  
    if (errors['required'])
      return `${field.charAt(0).toUpperCase() + field.slice(1)} is required`;
    if (errors['email'])
      return `Please enter a valid email address`;
  
    return 'Invalid input';
  }  

  onSubmit(): void {  
    if (this.loginForm.valid) {
      const loginData = this.loginForm.value; 

      this.authService.login(loginData).subscribe({
        next: (response: LoginResponse) => {
          const role = this.tokenService.getUserRole(response.token);

          if (role === 0) {
            this.router.navigate(['/admin/dashboard']);
          }
          else if (role === 1) {
            this.router.navigate(['/home']);
          }
        },
        error: (error) => {
          const message = error.error?.message as string;

          if (error.status === 403 && message === "This account is awaiting approval.") {
            this.dialog.open(InfoDialogComponent, {
              data: {
                title: 'Account Pending',
                message: 'Your account is currently awaiting approval by an administrator. You will be able to log in once approved.',
                buttonText: 'OK'
              }
            });
          } else if (error.status === 403 && message === "This account has been banned.") {
            this.snackbarService.error("This account has been banned from the platform.");
          } else if (error.status === 401) {
            this.snackbarService.error("The provided credentials are not valid.");
          } else {
            console.error("Login failed:", error);
            this.snackbarService.error("An error has occured! Please try again later.");
          }
        }
      });
    }
  }
}
