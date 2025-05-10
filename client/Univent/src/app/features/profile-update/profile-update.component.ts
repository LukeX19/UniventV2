import { Component, inject } from '@angular/core';
import { UserProfileResponse } from '../../shared/models/userModel';
import { UserService } from '../../core/services/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { UniversityService } from '../../core/services/university.service';
import { FileService } from '../../core/services/file.service';
import { SnackbarService } from '../../core/services/snackbar.service';
import { Observable, of } from 'rxjs';
import { UniversityResponse } from '../../shared/models/universityModel';
import { differenceInYears, isValid, parseISO } from 'date-fns';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { AuthenticationService } from '../../core/services/authentication.service';

@Component({
  selector: 'app-profile-update',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatAutocompleteModule,
    NavbarComponent
  ],
  templateUrl: './profile-update.component.html',
  styleUrl: './profile-update.component.scss'
})
export class ProfileUpdateComponent {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private universityService = inject(UniversityService);
  private fileService = inject(FileService);
  private snackbarService = inject(SnackbarService);
  private authService = inject(AuthenticationService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  
  user: UserProfileResponse | null = null;
  isUserLoading = true;

  universities$: Observable<UniversityResponse[]> = of([]);
  profileForm!: FormGroup;
  selectedUniversityId: string | null = null;
  selectedUniversityName: string | null = null;
  selectedFile: File | null = null;
  selectedImage: string | ArrayBuffer | null = null;
  today = new Date();
  isSubmitting = false;

  years = [
    { label: 'I', value: 1 }, { label: 'II', value: 2 }, { label: 'III', value: 3 },
    { label: 'IV', value: 4 }, { label: 'V', value: 5 }, { label: 'VI', value: 6 },
    { label: 'Master I', value: 7 }, { label: 'Master II', value: 8 },
  ];
  
  ngOnInit() {
    this.initializeForm();
    this.fetchUser();
    this.setupUniversitySearch();
  }

  initializeForm() {
    this.profileForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50), this.nameValidator]],
      lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50), this.nameValidator]],
      birthday: ['', [Validators.required, this.ageValidator]],
      university: ['', Validators.required],
      year: ['', Validators.required]
    });
  }

  fetchUser() {
    const userId = this.route.snapshot.paramMap.get('id');
    if (!userId) return;

    this.isUserLoading = true;
    this.userService.fetchUserProfileById(userId).subscribe({
      next: (data) => {
        this.user = data;
        this.isUserLoading = false;
        this.patchFormWithUserData(data);
      },
      error: (error) => {
        console.error("Error fetching user:", error);
        this.isUserLoading = false;
      }
    });
  }

  patchFormWithUserData(user: UserProfileResponse) {
    this.profileForm.patchValue({
      firstName: user.firstName,
      lastName: user.lastName,
      birthday: parseISO(user.birthday),
      university: user.universityName,
      year: user.year
    });

    this.selectedUniversityName = user.universityName;
    this.selectedUniversityId = user.universityId;
  }

  setupUniversitySearch() {
    this.universities$ = this.profileForm.get('university')!.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(value => value ? this.universityService.searchUniversity(value) : of([]))
    );
  }

  onUniversitySelected(university: UniversityResponse) {
    this.selectedUniversityId = university.id;
    this.selectedUniversityName = university.name;
    this.profileForm.patchValue({ university: university.name });
  }

  onUniversityBlur() {
    const input = this.profileForm.get('university')?.value;
    if (input !== this.selectedUniversityName) {
      this.profileForm.patchValue({ university: '' });
      this.selectedUniversityId = null;
    }
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file && file.type.startsWith('image/')) {
      const reader = new FileReader();
      reader.onload = e => this.selectedImage = e.target?.result!;
      reader.readAsDataURL(file);
      this.selectedFile = file;
    } else {
      this.snackbarService.error('Only image files are allowed.');
    }
  }

  removeImage() {
    this.selectedImage = null;
    this.selectedFile = null;
    this.user!.pictureUrl = null;
  }

  onSubmit() {
    if (this.profileForm.invalid || !this.selectedUniversityId) return;

    this.isSubmitting = true;

    const updatedProfile = {
      firstName: this.profileForm.value.firstName,
      lastName: this.profileForm.value.lastName,
      birthday: new Date(this.profileForm.value.birthday).toISOString(),
      year: this.profileForm.value.year,
      universityId: this.selectedUniversityId,
      pictureUrl: null as string | null
    };

    const submitUpdate = () => {
      this.userService.updateUserProfile(updatedProfile).subscribe({
        next: () => {
          this.snackbarService.success('Profile updated successfully.');
          this.authService.fetchUser();
          this.router.navigate([`/profile/${this.user!.id}`]);
        },
        error: (error) => {
          console.error("Update profile failed:", error);
          this.snackbarService.error("Something went wrong. Please try again later.");
        },
        complete: () => {
          this.isSubmitting = false;
        }
      });
    };

    if (this.selectedFile) {
      this.fileService.uploadFile(this.selectedFile).subscribe({
        next: (response) => {
          updatedProfile.pictureUrl = response.url;
          submitUpdate();
        },
        error: () => {
          this.snackbarService.error('Image upload failed.');
          submitUpdate();
        }
      });
    } else {
      updatedProfile.pictureUrl = this.user?.pictureUrl ?? null;
      submitUpdate();
    }
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

  getErrorMessage(field: string, errors: any): string {
    switch (field) {
      case 'firstName':
      case 'lastName':
        if (errors['required']) return `${field === 'firstName' ? 'First' : 'Last'} Name is required`;
        if (errors['minlength']) return `${field === 'firstName' ? 'First' : 'Last'} Name must have at least 3 characters`;
        if (errors['maxlength']) return `${field === 'firstName' ? 'First' : 'Last'} Name must have a maximum of 50 characters`;
        if (errors['invalidName']) return 'Only letters, spaces, and - , . \' are allowed';
        break;
      
      case 'birthday':
        if (errors['required']) return 'Birthday is required';
        if (errors['invalidAge']) return 'You must be at least 18 years old';
        break;

      case 'university':
        if (errors['required']) return 'University is required';
        break;

      case 'year':
        if (errors['required']) return 'Year is required';
        break;
    }
    return 'Invalid input';
  }
}
