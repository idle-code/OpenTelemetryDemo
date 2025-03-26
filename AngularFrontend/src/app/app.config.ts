import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient, withFetch, withInterceptorsFromDi } from "@angular/common/http";
import { WebApiUrlInterceptor } from "./web-api-url-interceptor.interceptor";
import { environment } from "../environments/environment";

export const appConfig: ApplicationConfig = {
    providers: [
        provideZoneChangeDetection({eventCoalescing: true}),
        provideRouter(routes),
        provideHttpClient(withFetch(), withInterceptorsFromDi()),
        {
            provide: HTTP_INTERCEPTORS,
            useClass: WebApiUrlInterceptor,
            multi: true
        },
        {
            provide: "BASE_API_URL",
            useValue: environment.baseApiUrl
        }
    ]
};
