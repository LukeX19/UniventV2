import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';

export interface EditTextDialogData {
  title?: string;
  initialValue: string;
  confirmText?: string;
  cancelText?: string;
}

@Component({
  selector: 'app-edit-text-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './edit-text-dialog.component.html',
  styleUrl: './edit-text-dialog.component.scss'
})
export class EditTextDialogComponent {
  value: string;

  constructor(
    public dialogRef: MatDialogRef<EditTextDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditTextDialogData
  ) {
    this.value = data.initialValue;
  }

  cancel(): void {
    this.dialogRef.close();
  }

  confirm(): void {
    this.dialogRef.close(this.value);
  }
}
