import { Component, OnInit, OnDestroy } from '@angular/core';
import { NerService } from './ner.component.service';
import { Subscription } from 'rxjs';



@Component({
  selector: 'app-ner',
  templateUrl: './ner.component.html',
  styleUrls: ['./ner.component.css']
})
export class NerComponent implements OnInit, OnDestroy {

  public ners: any;
  //Contains all subscription for the component
  private subscriptions : Subscription[] = [];
  loading$ = this.nerService.loadingSubject$.asObservable();

  constructor(private nerService: NerService
  ) {

  }
  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  ngOnInit() {
    this.subscriptions.push(this.nerService.payload$
      .subscribe(result => {
        this.ners = result;
      }));

    this.nerService.getText();
  }


}
