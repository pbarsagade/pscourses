import { Component } from "@angular/core";

@Component({
    selector: 'events-list',
    template: `
    <div>
    <h1>
        Upcoming Angular Events
    </h1>
    <hr>
    <event-thumbnail [event]="event" #thumbnail (eventClick)="handleEventClicked($event);"></event-thumbnail>   
</div>
<h3>{{thumbnail.someProperty}}</h3>
<button class="btn btn-primary" (click)="thumbnail.logFoo()"> Click Me!</button>
    `
})
export class EventsListComponent {
    event = {
        id: 1,
        name: 'Angular Connect',
        date: '9/26/2017',
        time: '10:00 AM',
        price: 599.99,
        imageUrl: '/app/assets/images/angularconnect-shield.png',
        location: {
            address: '1057 DT',
            city: 'London',
            country: 'England'
        }
    }
    handleEventClicked(msg) {
       console.log(msg);
    }
}
