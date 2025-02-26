import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';
import { EventCardComponent } from "../../shared/components/event-card/event-card.component";

@Component({
  selector: 'app-events-browse',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    NavbarComponent,
    EventCardComponent
],
  templateUrl: './events-browse.component.html',
  styleUrl: './events-browse.component.scss'
})
export class EventsBrowseComponent {
  events = [
    {
      id: "7EEBC516-0455-4929-D468-08DD52A1AF94",
      name: "Football Match - Students Friendly",
      enrolledParticipants: 7,
      maximumParticipants: 21,
      startTime: "2025-02-26 15:00:00.0000000",
      locationAddress: "Strada Profesor Doctor Aurel Păunescu Podeanu 2, Timișoara, Romania",
      pictureUrl: "https://univent-storage.s3.eu-central-1.amazonaws.com/26f01916-55de-45d1-b982-d256b7bc2f0f.jpg",
      createdAt: "2025-02-21 18:00:54.5801882",
      updatedAt: "2025-02-21 18:00:54.5802280",
      typeName: "Football Match",
      authorPictureUrl: "https://univent-storage.s3.eu-central-1.amazonaws.com/40786fbc-f410-4bd3-acba-b66406a3cf1f.jpg",
      authorFirstName: "John",
      authorLastName: "Doe",
      authorRating: 4.7
    },
    {
      id: "7EEBC516-0455-4929-D468-08DD52A1AF94",
      name: "Football Match - Students Friendly",
      enrolledParticipants: 7,
      maximumParticipants: 21,
      startTime: "2025-02-26 15:00:00.0000000",
      locationAddress: "Strada Profesor Doctor Aurel Păunescu Podeanu 2, Timișoara, Romania",
      pictureUrl: "https://univent-storage.s3.eu-central-1.amazonaws.com/26f01916-55de-45d1-b982-d256b7bc2f0f.jpg",
      createdAt: "2025-02-21 18:00:54.5801882",
      updatedAt: "2025-02-21 18:00:54.5802280",
      typeName: "Football Match",
      authorPictureUrl: "https://univent-storage.s3.eu-central-1.amazonaws.com/40786fbc-f410-4bd3-acba-b66406a3cf1f.jpg",
      authorFirstName: "John",
      authorLastName: "Doe",
      authorRating: 4.7
    },
    {
      id: "7EEBC516-0455-4929-D468-08DD52A1AF94",
      name: "Football Match - Students Friendly",
      enrolledParticipants: 7,
      maximumParticipants: 21,
      startTime: "2025-02-26 15:00:00.0000000",
      locationAddress: "Strada Profesor Doctor Aurel Păunescu Podeanu 2, Timișoara, Romania",
      pictureUrl: "https://univent-storage.s3.eu-central-1.amazonaws.com/26f01916-55de-45d1-b982-d256b7bc2f0f.jpg",
      createdAt: "2025-02-21 18:00:54.5801882",
      updatedAt: "2025-02-21 18:00:54.5802280",
      typeName: "Football Match",
      authorPictureUrl: "https://univent-storage.s3.eu-central-1.amazonaws.com/40786fbc-f410-4bd3-acba-b66406a3cf1f.jpg",
      authorFirstName: "John",
      authorLastName: "Doe",
      authorRating: 4.7
    },
    {
      id: "7EEBC516-0455-4929-D468-08DD52A1AF94",
      name: "Football Match - Students Friendly",
      enrolledParticipants: 7,
      maximumParticipants: 21,
      startTime: "2025-02-26 15:00:00.0000000",
      locationAddress: "Strada Profesor Doctor Aurel Păunescu Podeanu 2, Timișoara, Romania",
      pictureUrl: "https://univent-storage.s3.eu-central-1.amazonaws.com/26f01916-55de-45d1-b982-d256b7bc2f0f.jpg",
      createdAt: "2025-02-21 18:00:54.5801882",
      updatedAt: "2025-02-21 18:00:54.5802280",
      typeName: "Football Match",
      authorPictureUrl: "https://univent-storage.s3.eu-central-1.amazonaws.com/40786fbc-f410-4bd3-acba-b66406a3cf1f.jpg",
      authorFirstName: "John",
      authorLastName: "Doe",
      authorRating: 4.7
    },
    {
      id: "7EEBC516-0455-4929-D468-08DD52A1AF94",
      name: "Football Match - Students Friendly",
      enrolledParticipants: 7,
      maximumParticipants: 21,
      startTime: "2025-02-26 15:00:00.0000000",
      locationAddress: "Strada Profesor Doctor Aurel Păunescu Podeanu 2, Timișoara, Romania",
      pictureUrl: "https://univent-storage.s3.eu-central-1.amazonaws.com/26f01916-55de-45d1-b982-d256b7bc2f0f.jpg",
      createdAt: "2025-02-21 18:00:54.5801882",
      updatedAt: "2025-02-21 18:00:54.5802280",
      typeName: "Football Match",
      authorPictureUrl: "https://univent-storage.s3.eu-central-1.amazonaws.com/40786fbc-f410-4bd3-acba-b66406a3cf1f.jpg",
      authorFirstName: "John",
      authorLastName: "Doe",
      authorRating: 4.7
    }
  ];
}
