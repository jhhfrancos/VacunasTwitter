import { Component, OnInit, OnDestroy } from '@angular/core';
import { ResultsService } from './results.component.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.css']
})
export class ResultsComponent implements OnInit {

  //Contains all subscription for the component
  private subscriptions: Subscription[] = [];

  //Force directed graph
  loadingForcesGraph$ = this.resultsService.loadingfocesGraphSubject$.asObservable();
  loadingTweetUser$ = this.resultsService.loadingTweetUserSubject$.asObservable();
  public graph: any = null;
  public user: any = null;
  public tweet: any = null;
  public ners: any = null;
  public ldas: any = null;
  public tabIndex = 0;

  constructor(private resultsService: ResultsService) { }

  ngOnInit() {
    this.subscriptions.push(this.resultsService.focesGraphSubject$
      .subscribe(result => {
        this.graph = result;
      }));

    this.subscriptions.push(this.resultsService.userSubject$
      .subscribe(result => {
        this.user = result;
        this.tweet = null;
        this.tabIndex = 1;
      }));

    this.subscriptions.push(this.resultsService.tweetSubject$
      .subscribe(result => {
        this.tweet = result;
        this.user = result?.user;
        this.tabIndex = 0;
        this.ners = null;
        this.ldas = null;
        if (result != null) {
          this.resultsService.getLDA(result.retweetedStatus?.text || result?.text);
          this.resultsService.getNER(result.retweetedStatus?.text || result?.text);
        }
      }));

    this.subscriptions.push(this.resultsService.nerSubject$
      .subscribe(result => {
        this.ners = result;
      }));

    this.subscriptions.push(this.resultsService.ldaSubject$
      .subscribe(result => {
        this.ldas = result;
      }));

    this.resultsService.getForceDirectedGraph();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  clickItem(item: any) {
    switch (item.group) {
      case "1":
        this.resultsService.getUser(item.id);
        break;
      case "2":
        this.resultsService.getTweet(item.id);
        break;
      default:
        break;
    }
  }

  reeplaceNewLineStrings(text: string): string {
    return text.replace(/\n/gi, "<br>") || "";
  }

}
