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
  public graph: any = null;

  constructor(private resultsService: ResultsService) { }

  ngOnInit() {
    this.subscriptions.push(this.resultsService.focesGraphSubject$
      .subscribe(result => {
        this.graph = result;
      }));

      this.resultsService.getForceDirectedGraph();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  clickItem(item: any) {
    alert(`click the ${item.group}.`);
  }

}
