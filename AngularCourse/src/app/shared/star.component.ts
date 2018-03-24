import { Component, OnChanges, Input, EventEmitter, Output } from '@angular/core';

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'pm-star',
    templateUrl: 'star.component.html',
    styleUrls: ['star.component.css']

})
// tslint:disable-next-line:class-name
export class starComponent implements OnChanges {

    @Input() rating: number;
    starWidth: number;
    @Output() ratingClicked: EventEmitter<string> = new EventEmitter<string>();
    ngOnChanges(): void {
        this.starWidth = this.rating * 86 / 5;
    }
    onClick(): void {
       this.ratingClicked.emit(`The rating ${this.rating} was clicked!`);
    }
}
