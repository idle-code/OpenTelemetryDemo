import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class WebApiService {
  constructor(private webApi: HttpClient) {
  }

  incrementCounter(id: string): Observable<number> {
    return this.webApi.post<number>(`/counter/${id}/increment`, 1);
  }
}
