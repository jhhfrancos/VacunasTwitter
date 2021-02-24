
import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { BaseService } from 'app/commons/services/base-service.service';


@Injectable({
  providedIn: 'root'
})
export class NerService extends BaseService {

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
    this.http.get<String[]>(this.baseUrl + 'DataSet/api/getNER', { params }).subscribe(result => {
      this.payload$.next(result);
      this.loadingSubject$.next(false);
    }, error => console.error(error));
  }

  //Tweets_Profiles_Clean or Tweets_Base_Clean
  getWordCloud(db: string): any {
    if (db == "Tweets_Profiles_Clean")
      this.loadingWordCloudSubjectProfiles$.next(false);
    else
      this.loadingWordCloudSubjectBase$.next(false);

    let params = new HttpParams();
    params = params.append('limit', "1000");
    params = params.append('db', db);
    this.http.get<Array<[string, number]>>(this.baseUrl + 'DataSet/api/wordCloud', { params }).subscribe(result => {
      if (db == "Tweets_Profiles_Clean") {
        this.wordCloudSubjectProfiles$.next(result);
        this.loadingWordCloudSubjectProfiles$.next(true);
      }
      else {
        this.wordCloudSubjectBase$.next(result);
        this.loadingWordCloudSubjectBase$.next(true);
      }
    }, error => console.error(error));
  }
}
