import { CommonModule, Location } from '@angular/common';
import { AfterViewInit, Component, ElementRef, inject, ViewChild } from '@angular/core';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { NavbarComponent } from "../../shared/components/navbar/navbar.component";
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { GoogleMap, MapMarker } from '@angular/google-maps';
import { MatIconModule } from '@angular/material/icon';
import { FileService } from '../../core/services/file.service';
import { SnackbarService } from '../../core/services/snackbar.service';
import { EventFullResponse, UpdateEventRequest } from '../../shared/models/eventModel';
import { EventService } from '../../core/services/event.service';
import { TokenService } from '../../core/services/token.service';
import { CustomButtonComponent } from '../../shared/components/custom-button/custom-button.component';

@Component({
  selector: 'app-event-update',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    NavbarComponent,
    GoogleMap,
    MapMarker,
    CustomButtonComponent
  ],
  templateUrl: './event-update.component.html',
  styleUrl: './event-update.component.scss'
})
export class EventUpdateComponent implements AfterViewInit {
  private fb = inject(FormBuilder);
  private tokenService = inject(TokenService);
  private fileService = inject(FileService);
  private eventService = inject(EventService);
  private snackbarService = inject(SnackbarService);
  private router = inject(Router);
  private location = inject(Location);
  private route = inject(ActivatedRoute);

  @ViewChild('searchBox') searchBox!: ElementRef;
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  @ViewChild(GoogleMap) googleMap!: GoogleMap;

  eventId!: string;
  originalImageUrl: string | null = null;
  eventTypeName: string = '';
  isLoading = true;

  // Custom validator for image
  createFileValidator = () => {
    return (control: AbstractControl): ValidationErrors | null => {
      const hasNewFile = !!control.value;
      const hasOriginalImage = !!this.originalImageUrl;
  
      return hasNewFile || hasOriginalImage ? null : { requiredFile: 'Please provide an image for event thumbnail.' };
    };
  }  

  eventForm: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    description: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(3000)]],
    maximumParticipants: ['', [Validators.required, Validators.min(1)]],
    startDate: ['', Validators.required],
    startTime: ['', Validators.required],
    locationAddress: ['', Validators.required],
    locationLat: [null, Validators.required],
    locationLong: [null, Validators.required],
    selectedFile: [null, this.createFileValidator()]
  });

  selectedImage: string | ArrayBuffer | null = null;

  // 86400000 = 24 * 60 * 60 * 1000
  tomorrow: Date = new Date(Date.now() + 86400000);

  mapZoom: number = 13;
  mapCenter: google.maps.LatLngLiteral = { lat: 45.7559, lng: 21.2298 };
  selectedLocation: google.maps.LatLngLiteral | null = null;

  isSubmitting: boolean = false;

  ngOnInit() {
    this.eventId = this.route.snapshot.paramMap.get('id')!;
    const resolvedEvent = this.route.snapshot.data['event'] as EventFullResponse | null;

    if (!resolvedEvent) {
      this.router.navigate(['/not-found']);
      return;
    }

    const token = localStorage.getItem('uniapi-token');
    const currentUserId = this.tokenService.getUserId(token ?? '');

    const isAuthor = resolvedEvent.author.id === currentUserId;
    const isTooClose = (new Date(resolvedEvent.startTime).getTime() - Date.now()) < 2 * 60 * 60 * 1000;
    const isCancelled = resolvedEvent.isCancelled;

    if (!isAuthor || isTooClose || isCancelled) {
      this.router.navigate(['/forbidden']);
      return;
    }

    this.populateForm(resolvedEvent);
  }

  populateForm(event: EventFullResponse) {
    const rawStartTime = event.startTime;
    const isoStartTime = rawStartTime.endsWith('Z') ? rawStartTime : rawStartTime + 'Z';
    const userTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;

    const utcStartTime = new Date(isoStartTime);

    const localDate = new Date(
      utcStartTime.toLocaleString("en-US", { timeZone: userTimeZone })
    );
    const localHours = String(localDate.getHours()).padStart(2, '0');
    const localMinutes = String(localDate.getMinutes()).padStart(2, '0');
    const localTimeString = `${localHours}:${localMinutes}`;

    this.eventForm.patchValue({
      name: event.name,
      description: event.description,
      maximumParticipants: event.maximumParticipants,
      startDate: localDate,
      startTime: localTimeString,
      locationAddress: event.locationAddress,
      locationLat: event.locationLat,
      locationLong: event.locationLong
    });

    this.originalImageUrl = event.pictureUrl;
    this.selectedImage = event.pictureUrl;
    this.eventTypeName = event.typeName;

    this.eventForm.get('selectedFile')?.setValidators(this.createFileValidator());
    this.eventForm.get('selectedFile')?.updateValueAndValidity();

    this.selectedLocation = {
      lat: event.locationLat,
      lng: event.locationLong
    };
    this.mapCenter = this.selectedLocation;
    this.mapZoom = 15;

    this.isLoading = false;
  }

  extractTimeFromISOString(isoString: string): string {
    const date = new Date(isoString);
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    return `${hours}:${minutes}`;
  }

  ngAfterViewInit(): void {
    const defaultBounds = new google.maps.LatLngBounds(
      new google.maps.LatLng(45.5000, 20.9000), // Southwest corner
      new google.maps.LatLng(45.9000, 21.4000)  // Northeast corner
    );
  
    const autocomplete = new google.maps.places.Autocomplete(this.searchBox.nativeElement, {
      bounds: defaultBounds,
      strictBounds: true,
      componentRestrictions: { country: 'ro' },
      fields: ['place_id', 'geometry', 'name', 'formatted_address']
    });
  
    autocomplete.addListener('place_changed', () => {
      const place = autocomplete.getPlace();
      if (place.geometry) {
        this.selectedLocation = {
          lat: place.geometry.location!.lat(),
          lng: place.geometry.location!.lng()
        };
        this.mapCenter = this.selectedLocation;
        this.mapZoom = 16;
        this.eventForm.patchValue({
          locationAddress: place.formatted_address,
          locationLat: this.selectedLocation.lat,
          locationLong: this.selectedLocation.lng
        });
      }
    });
  }

  onZoomChanged(): void {
    if (this.googleMap) {
      this.mapZoom = this.googleMap.getZoom()!;
    }
  }

  onSearchInput(event: Event) {
    event.preventDefault();
  }

  onChooseImage(event: Event): void {
    event.preventDefault();
    event.stopPropagation();
    this.fileInput.nativeElement.click();
  }

  onEnterPress(event: Event): void {
    event.preventDefault();
    event.stopPropagation();
  }

  onMapClick(event: google.maps.MapMouseEvent) {
    if (event.latLng) {
      this.selectedLocation = {
        lat: event.latLng.lat(),
        lng: event.latLng.lng()
      };
      this.mapCenter = this.selectedLocation;
  
      // Reverse Geocoding: Get address from coordinates
      const geocoder = new google.maps.Geocoder();
      geocoder.geocode({ location: this.selectedLocation }, (results, status) => {
        if (status === "OK" && results?.length) {
          this.eventForm.patchValue({
            locationAddress: results[0].formatted_address,
            locationLat: this.selectedLocation!.lat,
            locationLong: this.selectedLocation!.lng
          });
        } else {
          console.error("Geocoder failed due to: ", status);
        }
      });
    }
  }

  clearLocation() {
    this.searchBox.nativeElement.value = '';
    this.selectedLocation = null;

    this.eventForm.patchValue({
      locationAddress: '',
      locationLat: '',
      locationLong: ''
    });
  }

  triggerImageValidation() {
    const selectedFileControl = this.eventForm.get('selectedFile');
    if (selectedFileControl) {
      selectedFileControl.markAsTouched(); 
      selectedFileControl.updateValueAndValidity();
    }
  }

  onFileSelected(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      if (!file.type.startsWith('image/')) {
        this.snackbarService.error('Only image files are allowed!');
        return;
      }

      const reader = new FileReader();
      reader.onload = (e) => this.selectedImage = e.target?.result!;
      reader.readAsDataURL(file);

      this.eventForm.patchValue({ selectedFile: file });
      this.triggerImageValidation();
    }
  }
  
  onDragOver(event: DragEvent): void {
    event.preventDefault();
  }
  
  onDrop(event: DragEvent): void {
    event.preventDefault();
    const file = event.dataTransfer?.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => this.selectedImage = e.target?.result!;
      reader.readAsDataURL(file);

      this.eventForm.patchValue({ selectedFile: file });
    }
  }

  removeImage() {
    this.selectedImage = null;
    this.originalImageUrl = null;

    this.eventForm.patchValue({ selectedFile: null });
    this.triggerImageValidation();
  }

  cancel() {
    this.location.back();
  }

  onSubmit(): void {
    this.eventForm.markAllAsTouched();

    if (this.eventForm.valid) {
      this.isSubmitting = true;

      const selectedFile = this.eventForm.value.selectedFile;

      if (selectedFile) {
        this.fileService.uploadFile(this.eventForm.value.selectedFile).subscribe({
          next: (response) => {
            this.updateEvent(response.url);
          },
          error: () => {
            this.snackbarService.error("Picture upload failed. Event creation failed.");
            this.isSubmitting = false;
          }
        });
      } else {
        this.updateEvent(this.originalImageUrl!);
      }
      
    }
  }

  private updateEvent(responseUrl: string): void {
    const formValues = this.eventForm.value;
    const fullStartDateTime = new Date(formValues.startDate);
    const [startHours, startMinutes] = formValues.startTime.split(':').map(Number);
    fullStartDateTime.setHours(startHours, startMinutes);

    let eventData: UpdateEventRequest = {
      name: formValues.name,
      description: formValues.description,
      maximumParticipants: formValues.maximumParticipants,
      startTime: new Date(fullStartDateTime).toISOString(),
      locationAddress: formValues.locationAddress,
      locationLat: formValues.locationLat,
      locationLong: formValues.locationLong,
      pictureUrl: responseUrl
    }

    this.eventService.updateEvent(this.eventId, eventData).subscribe({
      next: () => {
        this.snackbarService.success("Event updated successfully.");
        console.log(eventData);
        this.router.navigate(['/browse']);
      },
      error: (error) => {
        console.error("Event update failed:", error);
        this.snackbarService.error("Something went wrong. Please try again later.");
      },
      complete: () => {
        this.isSubmitting = true;
      }
    })
  }

  getErrorMessage(field: string, errors: any): string {
    if (!errors) return '';
  
    switch (field) {
      case 'name':
        if (errors['required']) return `Event Name is required`;
        if (errors['minlength']) return `Event Name must have at least 3 characters`;
        if (errors['maxlength']) return `Event Name must have a maximum of 50 characters`;
        break;

      case 'description':
        if (errors['required']) return `Description is required`;
        if (errors['minlength']) return `Description must have at least 3 characters`;
        if (errors['maxlength']) return `Description must have a maximum of 3000 characters`;
        break;
  
      case 'maximumParticipants':
        if (errors['required']) return `Maximum participants number is required`;
        if (errors['min']) return `Maximum participants number must be at least 1`;
        break;
      
      case 'startDate':
        if (errors['required']) return `Start date is required`;
        break;
  
      case 'startTime':
        if (errors['required']) return `Start time is required`;
        break;
      
      case 'locationAddress':
        if (errors['required']) return `Location is required`;
        break;
    }
  
    return 'Invalid input';
  }
}
