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
import dayjs from 'dayjs';
import { UniversityService } from '../../../core/services/university.service';
import { UniversityResponse } from '../../../shared/models/universityModel';
import { FileService } from '../../../core/services/file.service';
import { AuthenticationService } from '../../../core/services/authentication.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { RegisterRequest } from '../../../shared/models/authenticationModel';
import { Router, RouterModule } from '@angular/router';

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
    password: ['', [Validators.required, Validators.minLength(10)]],
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

  onSubmit(): void {
    if (this.step1Form.valid && this.step2Form.valid) {
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
        this.router.navigate(['/home']);
      },
      error: () => {
        console.error("Registration failed:");
        this.snackbarService.error("Registration failed. Please try again.");
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
    if (!control.value) return null;
    
    const birthDate = dayjs(control.value);
    const currentAge = dayjs().diff(birthDate, 'year');

    return currentAge >= 18 ? null : { invalidAge: 'You must be at least 18 years old' };
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
        if (errors['required']) return `First Name is required`;
        if (errors['minlength']) return `First Name must have at least 3 characters`;
        if (errors['maxlength']) return `First Name must have a maximum of 50 characters`;
        if (errors['invalidName']) return 'Only letters, spaces, and - , . \' are allowed';
        break;

      case 'lastName':
        if (errors['required']) return `Last Name is required`;
        if (errors['minlength']) return `Last Name must have at least 3 characters`;
        if (errors['maxlength']) return `Last Name must have a maximum of 50 characters`;
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
        if (errors['minlength']) return `Password must have at least 10 characters`;
        break;
  
      case 'confirmPassword':
        if (errors['required']) return `Confirm Password is required`;
        if (errors['passwordsNotMatch']) return `Passwords must match`;
        break;
    }
  
    return 'Invalid input';
  }
}
