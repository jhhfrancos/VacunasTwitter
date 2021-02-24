
import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { BaseService } from 'app/commons/services/base-service.service';


@Injectable({
  providedIn: 'root'
})
export class ResultsService extends BaseService {


  //Result of the transaction force directed
  focesGraphSubject$: BehaviorSubject<any> = new BehaviorSubject([]);
  //State of the transaction force directed
  loadingfocesGraphSubject$ = new BehaviorSubject<boolean>(true);


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(http, baseUrl);
  }

  getForceDirectedGraph(): any {
    this.loadingfocesGraphSubject$.next(false);
    let params = new HttpParams();
    params = params.append('limit', "10000");
    this.http.get<Array<[string, number]>>(this.baseUrl + 'DataSet/api/focesGraph', { params }).subscribe(result => {
      this.focesGraphSubject$.next(result);
      this.loadingfocesGraphSubject$.next(true);
    }, error => console.error(error));
  }
}
