
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

    //Result of the tweet
    tweetSubject$: BehaviorSubject<any> = new BehaviorSubject([]);
    //Result of the user
    userSubject$: BehaviorSubject<any> = new BehaviorSubject([]);
    //Result of the transaction
    loadingTweetUserSubject$ = new BehaviorSubject<boolean>(true);

    //Result LDA
    ldaSubject$: BehaviorSubject<any> = new BehaviorSubject([]);
    //Result NER
    nerSubject$: BehaviorSubject<any> = new BehaviorSubject([]);

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        super(http, baseUrl);
    }

    getForceDirectedGraph(): any {
        this.loadingfocesGraphSubject$.next(false);
        let params = new HttpParams();
        params = params.append('limit', "20000");
        this.http.get<Array<[string, number]>>(this.baseUrl + 'DataSet/api/focesGraph', { params }).subscribe(result => {

            this.focesGraphSubject$.next(result);
            this.loadingfocesGraphSubject$.next(true);

        }, error => console.error(error));
    }

    getTweet(id: string): any {
        this.loadingTweetUserSubject$.next(false);
        let params = new HttpParams();
        params = params.append('id', id);
        this.http.get<any>(this.baseUrl + 'DataSet/api/getTweet', { params }).subscribe(result => {
            //if (result != null)
            this.tweetSubject$.next(result);

            this.loadingTweetUserSubject$.next(true);
        }, error => console.error(error));
    }

    getUser(id: string): any {
        this.loadingTweetUserSubject$.next(false);
        let params = new HttpParams();
        params = params.append('id', id);
        this.http.get<any>(this.baseUrl + 'DataSet/api/getUser', { params }).subscribe(result => {

            this.userSubject$.next(result);
            this.loadingTweetUserSubject$.next(true);

        }, error => console.error(error));
    }

    getLDA(text: string): any {
        this.loadingTweetUserSubject$.next(false);
        let params = new HttpParams();
        params = params.append('text', text);
        this.http.get<any>(this.baseUrl + 'DataSet/api/getTweetLDA', { params }).subscribe(result => {

            this.ldaSubject$.next(result);
            this.loadingTweetUserSubject$.next(true);

        }, error => console.error(error));
    }

    getNER(text: string): any {
        this.loadingTweetUserSubject$.next(false);
        let params = new HttpParams();
        params = params.append('text', text);
        this.http.get<any>(this.baseUrl + 'DataSet/api/getTweetNER', { params }).subscribe(result => {

            this.nerSubject$.next(result);
            this.loadingTweetUserSubject$.next(true);

        }, error => console.error(error));
    }
}
