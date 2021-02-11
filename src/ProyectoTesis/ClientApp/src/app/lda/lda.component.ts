import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { LdaService } from './lda.component.service';

@Component({
  selector: 'app-lda',
  templateUrl: './lda.component.html',
  styleUrls: ['./lda.component.css']
})
export class LdaComponent implements OnInit {


  //Contains all subscription for the component
  private subscriptions: Subscription[] = [];
  loading$ = this.ldaService.loadingSubject$.asObservable();
  loadingWordCloud$ = this.ldaService.loadingWordCloudSubject$.asObservable();
  public wordCloudData: Array<[string, number]> = null;
  public ldas: String[];

  constructor(private ldaService: LdaService) { }

  ngOnInit() {
    this.subscriptions.push(this.ldaService.payload$
      .subscribe(result => {
        this.ldas = result;
      }));

    this.subscriptions.push(this.ldaService.wordCloudSubject$
      .subscribe(result => {
        this.wordCloudData = result;
      }));

    this.ldaService.getText();
    this.ldaService.getWordCloud();

  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }



}
