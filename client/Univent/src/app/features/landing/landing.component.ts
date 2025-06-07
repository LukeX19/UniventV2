import { Component } from '@angular/core';
import { CustomButtonComponent } from '../../shared/components/custom-button/custom-button.component';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [
    CustomButtonComponent
  ],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss'
})
export class LandingComponent {

}
