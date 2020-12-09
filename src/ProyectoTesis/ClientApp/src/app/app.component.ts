import { Component } from '@angular/core';
import { GlobalConstants } from './common/global-constants'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = GlobalConstants.siteTitle;
}
