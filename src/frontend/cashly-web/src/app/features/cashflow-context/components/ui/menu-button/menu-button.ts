import { Component, input, output} from '@angular/core';

@Component({
  selector: 'app-menu-button',
  imports: [],
  templateUrl: './menu-button.html',
  styleUrl: './menu-button.scss',
})
export class MenuButton {
 label = input.required<string>();

 clicked = output<void>();

 onCLick(){
  this.clicked.emit()
 }
}
