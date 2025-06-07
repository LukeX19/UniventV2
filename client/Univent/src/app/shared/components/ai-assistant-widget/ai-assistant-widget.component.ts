import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { AiAssistantService } from '../../../core/services/ai-assistant.service';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-ai-assistant-widget',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatIconModule
  ],
  templateUrl: './ai-assistant-widget.component.html',
  styleUrl: './ai-assistant-widget.component.scss'
})
export class AiAssistantWidgetComponent {
  private aiAssistantService = inject(AiAssistantService);

  isOpen = false;
  selectedOption: 'interests' | 'location' | 'time' | 'weather' | null = null;
  prompt: string = '';
  response: string = '';
  isLoading = false;
  hasError = false;

  messages: { role: 'user' | 'assistant', content: string }[] = [];

  toggleWidget(): void {
    this.isOpen = !this.isOpen;
    if (!this.isOpen) {
      this.resetState();
    }
  }

  selectOption(option: 'interests' | 'location' | 'time' | 'weather'): void {
    this.selectedOption = option;
    this.prompt = '';
    this.response = '';
    this.hasError = false;
  }

  submitPrompt(): void {
    if (!this.selectedOption || (this.selectedOption !== 'weather' && !this.prompt.trim())) return;

    this.isLoading = true;
    this.response = '';
    this.hasError = false;

    let request;

    switch (this.selectedOption) {
      case 'interests':
        request = this.aiAssistantService.askForInterestsBasedRecommendations(this.prompt);
        break;
      case 'location':
        request = this.aiAssistantService.askForLocationBasedRecommendations(this.prompt);
        break;
      case 'time':
        request = this.aiAssistantService.askForTimeBasedRecommendations(this.prompt);
        break;
      case 'weather':
        request = this.aiAssistantService.askForWeatherBasedRecommendations();
        break;
    }

    request.subscribe({
      next: (res) => {
        this.response = res;
        this.isLoading = false;
      },
      error: () => {
        this.hasError = true;
        this.isLoading = false;
      }
    });
  }

  private resetState(): void {
    this.selectedOption = null;
    this.prompt = '';
    this.response = '';
    this.isLoading = false;
    this.hasError = false;
  }
}
