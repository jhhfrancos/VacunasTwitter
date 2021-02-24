import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { BaseService } from 'app/commons/services/base-service.service';


@Injectable({
  providedIn: 'root'
})
export class LdaService extends BaseService {

  //Result of the transaction word cloud
  wordCloudSubjectProfiles$: BehaviorSubject<any> = new BehaviorSubject([]);
  //State of the transaction word cloud
  loadingWordCloudSubjectProfiles$ = new BehaviorSubject<boolean>(true);

  //Result of the transaction word cloud
  wordCloudSubjectBase$: BehaviorSubject<any> = new BehaviorSubject([]);
  //State of the transaction word cloud
  loadingWordCloudSubjectBase$ = new BehaviorSubject<boolean>(true);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(http, baseUrl);
  }

  getText(): any {
    this.loadingSubject$.next(true);
    let params = new HttpParams();
    params = params.append('limit', "1000");
    this.http.get<String[]>(this.baseUrl + 'DataSet/api/getLDA', { params }).subscribe(result => {
      this.payload$.next(result);
      this.loadingSubject$.next(false);
    }, error => console.error(error));
  }

  
}
