import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Inject, Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class WebApiUrlInterceptor implements HttpInterceptor {
    constructor(@Inject('BASE_API_URL') private baseUrl: string) {
        console.log(baseUrl);
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const request = req.clone({
            url: `${this.baseUrl}${req.url}`
        });
        console.log(`Intercepting ${req.url}`);
        return next.handle(request);
    }
}
