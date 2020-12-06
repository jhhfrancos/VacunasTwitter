import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent {
  public forecasts: String[];

  /*constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<String[]>(baseUrl + 'WeatherForecast/api/getFirst').subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }*/
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    let params = new HttpParams();
    params = params.append('limit', "10");
    http.get<String[]>(baseUrl + 'DataSet/api/getNER', { params }).subscribe(result => {
      console.log(result);

      for (const key in result) {
        let value = result[key];
        let texto = key;
        // Use `key` and `value`
      }
      this.forecasts = result;
    }, error => console.error(error));
  }
}

