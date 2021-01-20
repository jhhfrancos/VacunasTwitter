
import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';

export class BaseService {

    //Result of the transaction
    payload$: BehaviorSubject<any> = new BehaviorSubject([]);
    //State of the transaction
    loadingSubject$ = new BehaviorSubject<boolean>(false);

    constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string) { 
    }

  
}