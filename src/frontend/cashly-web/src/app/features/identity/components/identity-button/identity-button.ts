import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-identity-button',
  imports: [],
  templateUrl: './identity-button.html',
  styleUrl: './identity-button.scss',
})
export class IdentityButton {
@Input() text: string = 'Enviar';
}
