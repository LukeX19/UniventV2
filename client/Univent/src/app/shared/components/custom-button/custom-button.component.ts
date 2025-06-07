import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-custom-button',
  standalone: true,
  imports: [
    CommonModule, 
    MatIconModule
  ],
  templateUrl: './custom-button.component.html',
  styleUrl: './custom-button.component.scss'
})
export class CustomButtonComponent {
  @Input() label: string = 'Button';
  @Input() variant: 'primary' | 'secondary' | 'warning' = 'primary';
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  
  @Input() backgroundColor?: string;
  @Input() textColor?: string;
  @Input() width: string = 'auto';
  @Input() minWidth: string = '110px';

  @Input() icon?: string;
  @Input() iconPosition: 'left' | 'right' = 'left';

  @Input() disabled: boolean = false;
  @Input() border?: string;

  @Output() click = new EventEmitter<Event>();

  computedBackgroundColor!: string;
  computedTextColor!: string;
  computedBorder!: string;

  ngOnInit(): void {
    const styleMap = {
      primary: {
        bg: '#312A8D',
        text: '#FFFFFF',
        border: 'none'
      },
      secondary: {
        bg: '#FFFFFFF',
        text: '#393E46',
        border: '1px solid #393E46'
      },
      warning: {
        bg: '#DC3544',
        text: '#FFFFFF',
        border: 'none'
      }
    };

    const variantStyle = styleMap[this.variant];

    this.computedBackgroundColor = this.backgroundColor ?? variantStyle.bg;
    this.computedTextColor = this.textColor ?? variantStyle.text;
    this.computedBorder = this.border ?? variantStyle.border;
  }

  handleClick(event: Event): void {
    if (!this.disabled) {
      this.click.emit(event);
    }
  }
}
