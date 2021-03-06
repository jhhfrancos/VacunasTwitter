
import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { BaseService } from 'app/commons/services/base-service.service';


@Injectable({
  providedIn: 'root'
})
export class TableroService extends BaseService {

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(http, baseUrl);
  }

  getStatistics(): any {
    this.loadingSubject$.next(true);
    let params = new HttpParams();
    params = params.append('limit', "1000");
    this.http.get<any>(this.baseUrl + 'DataSet/api/dbStatistics', { params }).subscribe(result => {
      this.payload$.next(result);
      this.loadingSubject$.next(false);
    }, error => console.error(error));
  }

  
}
