import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { starComponent } from './star.component';
import { FormsModule } from '@angular/forms';
import { convertToSpacesPipe } from './convert-to-spaces.pipe';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    starComponent,
    convertToSpacesPipe
  ],
  exports: [
    starComponent,
    convertToSpacesPipe,
    CommonModule,
    FormsModule
  ]
})
export class SharedModule { }
