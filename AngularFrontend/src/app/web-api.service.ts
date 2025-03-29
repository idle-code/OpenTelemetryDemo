import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class WebApiService {
  constructor(private webApi: HttpClient) {
  }

  incrementCounter(id: string): Observable<NamedCounter> {
    return this.webApi.post<NamedCounter>(`/counter/${id}/increment`, 1);
  }
}

export class NamedCounter {
  readonly id: string;
  readonly value: number;

  constructor(id: string, value: number) {
    this.id = id;
    this.value = value;
  }
}
