import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, AbstractControl, ValidationErrors } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatStepperModule } from '@angular/material/stepper';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CommonModule } from '@angular/common';
import { Observable, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { differenceInYears, parseISO, isValid } from 'date-fns';
import { UniversityService } from '../../../core/services/university.service';
import { UniversityResponse } from '../../../shared/models/universityModel';
import { FileService } from '../../../core/services/file.service';
import { AuthenticationService } from '../../../core/services/authentication.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { RegisterRequest } from '../../../shared/models/authenticationModel';
import { Router, RouterModule } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { InfoDialogComponent } from '../../../shared/components/info-dialog/info-dialog.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatStepperModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatAutocompleteModule,
    RouterModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private universityService = inject(UniversityService);
  private fileService = inject(FileService);
  private authService = inject(AuthenticationService);
  private snackbarService = inject(SnackbarService);
  private router = inject(Router);
  private dialog = inject(MatDialog);

  universities$: Observable<UniversityResponse[]> = of([]);
  selectedUniversityId: string | null = null;
  selectedUniversityName: string | null = null;
  today: Date = new Date();

  step1Form: FormGroup = this.fb.group({
    firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50), this.nameValidator]],
    lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50), this.nameValidator]],
    birthday: ['', [Validators.required, this.ageValidator]],
    university: ['', Validators.required],
    year: ['', Validators.required],
  });

  constructor() {
    this.setupUniversitySearch();
  }

  private setupUniversitySearch() {
    this.universities$ = this.step1Form.get('university')!.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(value => value ? this.universityService.searchUniversity(value) : of([]))
    );
  }

  onUniversitySelected(selectedUniversity: UniversityResponse) {
    this.selectedUniversityId = selectedUniversity.id;
    this.selectedUniversityName = selectedUniversity.name;

    this.step1Form.patchValue({
      university: selectedUniversity.name
    });
  }

  onUniversityBlur() {
    const currentInput = this.step1Form.get('university')!.value;

    if (!this.selectedUniversityName || currentInput !== this.selectedUniversityName) {
      this.step1Form.patchValue({ university: '' });
      this.selectedUniversityId = null;
      this.selectedUniversityName = null;
    }
  }

  step2Form: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, this.passwordValidator]],
    confirmPassword: ['', [Validators.required]]
  }, { validators: this.passwordMatchValidator });

  step3Form: FormGroup = this.fb.group({
    profilePicture: [null]
  });

  hidePassword = true;
  hideConfirmPassword = true;
  selectedImage: string | ArrayBuffer | null = null;
  selectedFile: File | null = null;

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.hideConfirmPassword = !this.hideConfirmPassword;
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      if (!file.type.startsWith('image/')) {
        this.snackbarService.error('Only image files are allowed!');
        return;
      }

      const reader = new FileReader();
      reader.onload = (e) => this.selectedImage = e.target?.result!;
      reader.readAsDataURL(file);

      this.selectedFile = file;
    }
  }

  removeImage(): void {
    this.selectedImage = null;
    this.selectedFile = null;
  }

  isRegistering = false;
  onSubmit(): void {
    if (this.step1Form.valid && this.step2Form.valid) {
      this.isRegistering = true;

      let registerData: RegisterRequest = {
        firstName: this.step1Form.value.firstName,
        lastName: this.step1Form.value.lastName,
        birthday: new Date(this.step1Form.value.birthday).toISOString(),
        pictureURL: null,
        email: this.step2Form.value.email,
        password: this.step2Form.value.password,
        role: 1,
        year: this.mapUniversityYear(this.step1Form.value.year),
        universityId: this.selectedUniversityId
      }

      if (this.selectedFile) {
        this.fileService.uploadFile(this.selectedFile).subscribe({
          next: (response) => {
            registerData.pictureURL = response.url;
            this.registerUser(registerData);
          },
          error: () => {
            this.snackbarService.error("Picture upload failed. Registering without a picture.");
            this.registerUser(registerData);
          }
        });
      } else {
        this.registerUser(registerData);
      }
    }
  }

  private registerUser(data: RegisterRequest): void {
    this.authService.register(data).subscribe({
      next: () => {
        this.router.navigate(['']);
        this.dialog.open(InfoDialogComponent, {
          data: {
            title: 'Registration Submitted',
            message: 'Your account has been created and is now pending approval by an administrator. You will be able to log in once approved.',
            buttonText: 'OK'
          }
        });
      },
      error: (error) => {
        console.error("Registration failed:", error);

        if (error.status === 409) {
          this.snackbarService.error("An account with this email already exists. Please try logging in.");
        } else {
          this.snackbarService.error("Something went wrong. Please try again later.");
        }
      },
      complete: () => {
        this.isRegistering = false;
      }
    });
  }

  private mapUniversityYear(year: string): number {
    const mapping: { [key: string]: number } = {
      'I': 1, 'II': 2, 'III': 3, 'IV': 4, 'V': 5, 'VI': 6,
      'Master I': 7, 'Master II': 8
    };
    return mapping[year];
  }

  getPasswordErrors(): string[] {
    const control = this.step2Form.get('password');
    if (!control || !control.errors) return [];

    const errors = control.errors;
    const messages = [];

    if (errors['requiredLength']) messages.push('10 characters');
    if (errors['requireLowercase']) messages.push('one lowercase character');
    if (errors['requireUppercase']) messages.push('one uppercase character');
    if (errors['requireDigit']) messages.push('one digit');
    if (errors['requireNonAlphanumeric']) messages.push('one special character (!?@#)');

    return messages;
  }

  getPasswordValidationClass(errorType: string): string {
    const control = this.step2Form.get('password');

    if (!control || !control.value) return 'text-red-500';

    return control.errors?.[errorType] ? 'text-red-500' : 'text-green-500';
  }

  // Custom validator for names (only alphabet characters, spaces, and - , . ' allowed)
  nameValidator(control: AbstractControl): ValidationErrors | null {
    const nameRegex = /^[a-zA-Z ,.'-]+$/;
    if (!control.value || nameRegex.test(control.value)) {
      return null;
    }
    return { invalidName: 'Name must contain only letters, spaces, and - , . \'' };
  }

  // Custom validator to check if user is at least 18 years old
  ageValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (!value) return null;
  
    const birthDate = new Date(value);
    if (!isValid(birthDate)) return { invalidAge: 'Invalid date' };
  
    const age = differenceInYears(new Date(), birthDate);
  
    return age >= 18 ? null : { invalidAge: 'You must be at least 18 years old' };
  }

  // Custom validator to check for Identity password constraints
  passwordValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (!value) return null;

    const errors: any = {};
    
    if (value.length < 10) errors.requiredLength = true;
    if (!/[a-z]/.test(value)) errors.requireLowercase = true;
    if (!/[A-Z]/.test(value)) errors.requireUppercase = true;
    if (!/[0-9]/.test(value)) errors.requireDigit = true;
    if (!/[\W_]/.test(value)) errors.requireNonAlphanumeric = true;

    return Object.keys(errors).length ? errors : null;
  }

  // Custom validator to check if password and confirm password match
  passwordMatchValidator(group: AbstractControl): ValidationErrors | null {
    const passwordControl = group.get('password');
    const confirmPasswordControl = group.get('confirmPassword');
  
    if (!passwordControl || !confirmPasswordControl) return null;
  
    if (confirmPasswordControl.errors && !confirmPasswordControl.errors['passwordsNotMatch']) {
      return null;
    }
  
    if (passwordControl.value !== confirmPasswordControl.value) {
      confirmPasswordControl.setErrors({ passwordsNotMatch: true });
    } else {
      confirmPasswordControl.setErrors(null);
    }
  
    return null;
  }

  getErrorMessage(field: string, errors: any): string {
    if (!errors) return '';
  
    switch (field) {
      case 'firstName':
      case 'lastName':
        if (errors['required']) return `${field === 'firstName' ? 'First' : 'Last'} Name is required`;
        if (errors['minlength']) return `${field === 'firstName' ? 'First' : 'Last'} Name must have at least 3 characters`;
        if (errors['maxlength']) return `${field === 'firstName' ? 'First' : 'Last'} Name must have a maximum of 50 characters`;
        if (errors['invalidName']) return 'Only letters, spaces, and - , . \' are allowed';
        break;
  
      case 'birthday':
        if (errors['required']) return `Birthday is required`;
        if (errors['invalidAge']) return `You must be at least 18 years old`;
        break;
      
      case 'university':
        if (errors['required']) return `University is required`;
        break;
  
      case 'year':
        if (errors['required']) return `University Year is required`;
        break;
      
      case 'email':
        if (errors['required']) return `Email is required`;
        if (errors['email']) return 'Email must have a valid format';
        break;
  
      case 'password':
        if (errors['required']) return `Password is required`;
        if (errors['requiredLength']) return ``;
        if (errors['requireLowercase']) return ``;
        if (errors['requireUppercase']) return ``;
        if (errors['requireDigit']) return ``;
        if (errors['requireNonAlphanumeric']) return ``;
        break;
  
      case 'confirmPassword':
        if (errors['required']) return `Confirm Password is required`;
        if (errors['passwordsNotMatch']) return `Passwords must match`;
        break;
    }
  
    return 'Invalid input';
  }
}
