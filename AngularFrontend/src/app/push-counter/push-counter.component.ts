import { Component, signal } from '@angular/core';
import { WebApiService } from "../web-api.service";

@Component({
    selector: 'app-push-counter',
    imports: [],
    template: `
        <div>
            <label for="incrementButton">Current clicks: {{ PushCounter() }}</label><br/>
            <button id="incrementButton" (click)="RegisterPush()">Push me!</button>
        </div>
    `,
    styleUrl: './push-counter.component.scss'
})
export class PushCounterComponent {
    PushCounter = signal<number>(0);

    constructor(private webApi: WebApiService) {
        this.webApi = webApi;
    }

    RegisterPush() {
        console.log("RegisterPush executed");
        this.webApi.incrementCounter("rabarbarowany").subscribe(namedCounter => {
            this.PushCounter.set(namedCounter.value);
        });
    }
}
