import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, ElementRef, inject, ViewChild } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { NavbarComponent } from "../../shared/components/navbar/navbar.component";
import { FormBuilder, FormsModule } from '@angular/forms';
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
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-event-create',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
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
  private http = inject(HttpClient);
  private router = inject(Router);

  @ViewChild('searchBox') searchBox!: ElementRef;
  
  eventImage: string | null = null;
  eventName: string = '';
  eventType: string = '';
  startDate: Date | null = null;
  startTime: string = '';
  endDate: Date | null = null;
  endTime: string = '';
  maxParticipants: number | null = null;
  eventDescription: string = '';

  eventTypes: EventTypeResponse[] = [];
  loadingEventTypes: boolean = true;
  errorLoadingEventTypes: boolean = false;

  mapZoom: number = 13;
  mapCenter: google.maps.LatLngLiteral = { lat: 45.7559, lng: 21.2298 };
  selectedLocation: google.maps.LatLngLiteral | null = null;
  locationAddress: string = '';

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
        this.searchBox.nativeElement.value = place.formatted_address;
      }
    });
  }

  onSearchInput(event: Event) {
    event.preventDefault();
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
          this.locationAddress = results[0].formatted_address;
          this.searchBox.nativeElement.value = results[0].formatted_address;
        } else {
          console.error("Geocoder failed due to: ", status);
        }
      });
    }
  }

  clearLocation() {
    this.searchBox.nativeElement.value = '';
    this.selectedLocation = null;
    this.locationAddress = '';
  }  

  onFileSelected(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.eventImage = e.target?.result as string;
      };
      reader.readAsDataURL(file);
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
      reader.onload = (e) => {
        this.eventImage = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  removeImage() {
    this.eventImage = null;
  }

  cancel() {

  }

  submitEvent(): void {
    if (!this.startDate || !this.startTime || !this.endDate || !this.endTime) {
      console.error("Start and End Date & Time must be selected");
      return;
    }

    // Combine date & time for start time
    const [startHours, startMinutes] = this.startTime.split(':').map(Number);
    const fullStartDateTime = new Date(this.startDate);
    fullStartDateTime.setHours(startHours, startMinutes);

    // Combine date & time for end time
    const [endHours, endMinutes] = this.endTime.split(':').map(Number);
    const fullEndDateTime = new Date(this.endDate);
    fullEndDateTime.setHours(endHours, endMinutes);

    const eventData = {
      name: this.eventName,
      type: this.eventType,
      startDateTime: fullStartDateTime,
      endDateTime: fullEndDateTime,
      maxParticipants: this.maxParticipants,
      description: this.eventDescription,
      image: this.eventImage
    };

    console.log("Event Data:", eventData);
  }
}
