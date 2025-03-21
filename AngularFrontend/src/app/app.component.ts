import {Component, signal} from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  // template: `
  //   <div>
  //     <label for="incrementButton">Current clicks: {{counterValue}}</label><br/>
  //     <button id="incrementButton" (click)="IncrementCounter()">Push me!</button>
  //   </div>
  // `,
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'AngularFrontend';

  counterValue = 0;

  IncrementCounter() {
    ++this.counterValue;
  }
}
