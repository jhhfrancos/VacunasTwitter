import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: String[];

  /*constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<String[]>(baseUrl + 'WeatherForecast/api/getFirst').subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }*/
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    let params = new HttpParams();
    params = params.append('limit', "10");
    http.get<String[]>(baseUrl + 'DataSet/api/getAll', {params}).subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
