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

  //Result of the transaction
  loadingLdaSubject$ = new BehaviorSubject<boolean>(true);
  //Result LDA
  ldaSubject$: BehaviorSubject<any> = new BehaviorSubject([]);

  //Result of the transaction
  loadingTrainnginLdaSubject$ = new BehaviorSubject<boolean>(false);
  //Result trainning LDA
  trainnginLdaSubject$: BehaviorSubject<any> = new BehaviorSubject([]);

  //Result of the transaction
  loadingTsneLdaSubject$ = new BehaviorSubject<boolean>(false);
  //Result TSNE LDA
  tsneLdaSubject$: BehaviorSubject<any> = new BehaviorSubject([]);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(http, baseUrl);
  }

  getTestResultLDA(countResult: string): any {
    this.loadingSubject$.next(true);
    let params = new HttpParams();
    params = params.append('limit', countResult);
    this.http.get<String[]>(this.baseUrl + 'DataSet/api/getTestResultLDA', { params }).subscribe(result => {
      this.payload$.next(result);
      this.loadingSubject$.next(false);
    }, error => console.error(error));
  }

  getTestLDA(text: string): any {
    this.loadingLdaSubject$.next(false);
    let params = new HttpParams();
    params = params.append('text', text);
    this.http.get<any>(this.baseUrl + 'DataSet/api/getTweetLDA', { params }).subscribe(result => {
      this.ldaSubject$.next(result);
      this.loadingLdaSubject$.next(true);
    }, error => console.error(error));
  }

  getTraingLDA(countTrainning: string): any {
    this.loadingTrainnginLdaSubject$.next(true);
    let params = new HttpParams();
    params = params.append('limit', countTrainning);
    params = params.append('numTopics', "300");
    params = params.append('NumOfTerms', "7");
    this.http.get<String[]>(this.baseUrl + 'DataSet/api/getTrainLDA', { params }).subscribe(result => {
      this.trainnginLdaSubject$.next(result);
      this.loadingTrainnginLdaSubject$.next(false);
    }, error => console.error(error));
  }

  getTsneLDA(limit: number, perplexity: number): any {
    this.loadingTsneLdaSubject$.next(true);
    let params = new HttpParams();
    params = params.append('limit', limit.toString());
    params = params.append('perplexity', perplexity.toString());
    this.http.get<String[]>(this.baseUrl + 'DataSet/api/tsne', { params }).subscribe(result => {
      this.tsneLdaSubject$.next(result);
      this.loadingTsneLdaSubject$.next(false);
    }, error => console.error(error));
  }

}
