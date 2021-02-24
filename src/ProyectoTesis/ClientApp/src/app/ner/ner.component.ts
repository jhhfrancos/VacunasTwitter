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
  private subscriptions: Subscription[] = [];
  loading$ = this.nerService.loadingSubject$.asObservable();

  //Words clouds
  loadingWordCloudProfiles$ = this.nerService.loadingWordCloudSubjectProfiles$.asObservable();
  public wordCloudDataProfiles: Array<[string, number]> = null;

  loadingWordCloudBase$ = this.nerService.loadingWordCloudSubjectBase$.asObservable();
  public wordCloudDataBase: Array<[string, number]> = null;


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

    this.subscriptions.push(this.nerService.wordCloudSubjectProfiles$
      .subscribe(result => {
        this.wordCloudDataProfiles = result;
      }));

    this.subscriptions.push(this.nerService.wordCloudSubjectBase$
      .subscribe(result => {
        this.wordCloudDataBase = result;
      }));

    this.nerService.getWordCloud("Tweets_Profiles_Clean");
    this.nerService.getWordCloud("Tweets_Base_Clean");

    this.nerService.getText();

  }


}
