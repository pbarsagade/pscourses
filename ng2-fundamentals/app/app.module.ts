import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser'
import { EventAppComponent } from './event-app.component';
import { EventsListComponent } from './events/events-list.component';
import { EventThumnNailComponent } from './events/events-thumbnail.component';

@NgModule({
    imports: [BrowserModule],
    declarations: [
        EventAppComponent,
        EventsListComponent,
        EventThumnNailComponent
    ],
    bootstrap: [EventAppComponent]
})
export class AppModule { }