import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

export interface DialogEventParticipant {
  firstName: string;
  lastName: string;
  pictureUrl?: string | null;
  rating: number;
}

export interface InfoDialogData {
  title?: string;
  message?: string;
  participants?: DialogEventParticipant[];
  buttonText?: string;
}

@Component({
  selector: 'app-info-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    MatDialogModule,
    MatButtonModule
  ],
  templateUrl: './info-dialog.component.html',
  styleUrl: './info-dialog.component.scss'
})
export class InfoDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<InfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: InfoDialogData
  ) {}

  onClose(): void {
    this.dialogRef.close();
  }

  getInitials(firstName: string, lastName: string): string {
    const firstNameInitial = firstName?.charAt(0).toUpperCase() || '';
    const lastNameInitial = lastName?.charAt(0).toUpperCase() || '';
    return `${firstNameInitial}${lastNameInitial}`;
  }  
}
