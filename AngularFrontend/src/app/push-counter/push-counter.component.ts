import { Component, signal } from '@angular/core';
import { WebApiService } from "../web-api.service";
import { FormsModule } from "@angular/forms";

@Component({
    selector: 'app-push-counter',
    imports: [
        FormsModule
    ],
    template: `
        <div>
            <label for="counterName">Counter name:</label><br/>
            <input id="counterName" [(ngModel)]="PushCounterName" (change)="UpdateCounter()"/><br/>
            <label for="incrementButton">Current clicks: {{ PushCounter() }}</label><br/>
            <button id="incrementButton" (click)="RegisterPush()">Push me!</button>
        </div>
    `,
    styleUrl: './push-counter.component.scss'
})
export class PushCounterComponent {
    PushCounterName = "rabarbar";
    PushCounter = signal<number>(0);

    constructor(private webApi: WebApiService) {
        this.webApi = webApi;
        this.UpdateCounter();
    }

    UpdateCounter()
    {
        console.log(`Fetching current value of ${this.PushCounterName}`);
        this.webApi.getCounterValue(this.PushCounterName).subscribe({
                next: namedCounter => {
                    this.PushCounter.set(namedCounter.value);
                },
                error: err => {
                    this.PushCounter.set(0);
                }
            }
        );
    }

    RegisterPush() {
        console.log("RegisterPush executed");
        this.webApi.incrementCounter(this.PushCounterName).subscribe(namedCounter => {
            this.PushCounter.set(namedCounter.value);
        });
    }
}
