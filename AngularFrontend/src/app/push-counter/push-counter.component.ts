import { Component, signal } from '@angular/core';

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

    RegisterPush() {
        this.PushCounter.update(value => value + 1);
    }
}
