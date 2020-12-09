import { Component } from '@angular/core';

@Component({
  selector: 'app-side-nav-menu',
  templateUrl:'./nav-side-menu.component.html',
  styleUrls: ['./nav-side-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
