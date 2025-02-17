import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { environment } from './environments/environment';

function loadGoogleMapsAPI(): Promise<void> {
  return new Promise((resolve, reject) => {
    if ((window as any).google) {
      resolve();
      return;
    }

    const script = document.createElement('script');
    script.src = `https://maps.googleapis.com/maps/api/js?key=${environment.googleMapsApiKey}&libraries=places`;
    script.async = true;
    script.defer = true;
    script.onload = () => resolve();
    script.onerror = () => reject(new Error("Google Maps API failed to load"));
    document.head.appendChild(script);
  });
}

loadGoogleMapsAPI()
  .then(() => bootstrapApplication(AppComponent, appConfig))
  .catch((err) => console.error("Error bootstrapping application:", err));
