import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, ElementRef, inject, ViewChild } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
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
import { EventTypeService } from '../../core/services/event-type.service';
import { EventTypeResponse } from '../../shared/models/eventTypeModel';
import { FileService } from '../../core/services/file.service';
import { SnackbarService } from '../../core/services/snackbar.service';

@Component({
  selector: 'app-event-create',
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
    MapMarker
],
  providers: [],
  templateUrl: './event-create.component.html',
  styleUrl: './event-create.component.scss'
})
export class EventCreateComponent implements AfterViewInit {
  private fb = inject(FormBuilder);
  private eventTypeService = inject(EventTypeService);
  private fileService = inject(FileService);
  private snackbarService = inject(SnackbarService);
  private router = inject(Router);

  @ViewChild('searchBox') searchBox!: ElementRef;
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  eventForm: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    description: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(3000)]],
    maximumParticipants: ['', [Validators.required, Validators.min(1)]],
    startDate: ['', Validators.required],
    startTime: ['', Validators.required],
    endDate: ['', Validators.required],
    endTime: ['', Validators.required],
    locationAddress: ['', Validators.required],
    locationLat: [null, Validators.required],
    locationLong: [null, Validators.required],
    typeId: ['', [Validators.required]],
    selectedFile: [null, this.fileRequiredValidator]
  });

  selectedImage: string | ArrayBuffer | null = null;
  //selectedFile: File | null = null;

  eventTypes: EventTypeResponse[] = [];
  loadingEventTypes: boolean = true;
  errorLoadingEventTypes: boolean = false;

  mapZoom: number = 13;
  mapCenter: google.maps.LatLngLiteral = { lat: 45.7559, lng: 21.2298 };
  selectedLocation: google.maps.LatLngLiteral | null = null;

  submitting: boolean = false;

  ngOnInit() {
    this.eventTypeService.fetchActiveEventTypes().subscribe({
      next: (data) => {
        this.eventTypes = data;
        this.loadingEventTypes = false;
      },
      error: (error) => {
        console.error('Failed to load event types:', error);
        this.loadingEventTypes = false;
        this.errorLoadingEventTypes = true;
      }
    });
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
        this.eventForm.patchValue({
          locationAddress: place.formatted_address,
          locationLat: this.selectedLocation.lat,
          locationLong: this.selectedLocation.lng
        });
      }
    });
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
          this.eventForm.patchValue({ locationAddress: results[0].formatted_address });
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

      //this.selectedFile = file;
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

      //this.selectedFile = file;
      this.eventForm.patchValue({ selectedFile: file });
    }
  }

  removeImage() {
    this.selectedImage = null;
    //this.selectedFile = null;
    this.eventForm.patchValue({ selectedFile: null });
    this.triggerImageValidation();
  }

  cancel() {

  }

  submitEvent(): void {
    this.eventForm.markAllAsTouched();

    const formValues = this.eventForm.value;
    const fullStartDateTime = new Date(formValues.startDate);
    const [startHours, startMinutes] = formValues.startTime.split(':').map(Number);
    fullStartDateTime.setHours(startHours, startMinutes);

    const fullEndDateTime = new Date(formValues.endDate);
    const [endHours, endMinutes] = formValues.endTime.split(':').map(Number);
    fullEndDateTime.setHours(endHours, endMinutes);

    const eventData = {
      name: formValues.name,
      typeId: formValues.typeId,
      startDateTime: fullStartDateTime,
      endDateTime: fullEndDateTime,
      maximumParticipants: formValues.maximumParticipants,
      description: formValues.description,
      locationAddress: formValues.locationAddress,
      locationLat: formValues.locationLat,
      locationLong: formValues.locationLong
    };

    console.log("Event Data:", eventData);
  }

  // Custom validator for image
  fileRequiredValidator(control: AbstractControl): ValidationErrors | null {
    return control.value ? null : { requiredFile: 'Please provide an image for event thumbnail.' };
}
}
