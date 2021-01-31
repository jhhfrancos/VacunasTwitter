
import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { BaseService } from 'app/commons/services/base-service.service';


@Injectable({
  providedIn: 'root'
})
export class AdminService extends BaseService {

  loadingCleansingDB$ = new BehaviorSubject<boolean>(false);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) { 
    super(http,baseUrl);
  }

  UpdateDB() : any {
    this.loadingSubject$.next(true);
    let params = new HttpParams();
    params = params.append('limit', "10");
    this.http.get<String[]>(this.baseUrl + 'DataSet/api/updateDB', { params }).subscribe(result => {
      this.payload$.next(result);
      this.loadingSubject$.next(false);
    }, error => console.error(error));
  }

  DataCleansingDB() : any {
    this.loadingCleansingDB$.next(true);
    let params = new HttpParams();
    params = params.append('limit', "10");
    this.http.get<String[]>(this.baseUrl + 'DataSet/api/dataCleansing', { params }).subscribe(result => {
      this.payload$.next(result);
      this.loadingCleansingDB$.next(false);
    }, error => console.error(error));
  }
}