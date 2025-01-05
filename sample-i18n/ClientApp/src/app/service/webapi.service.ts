import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WebApiService {

  constructor(private http: HttpClient) {
  }

  getUsername(): Observable<string> {
    return this.http.get('/api/username', {responseType: "text",withCredentials:true});
  }
}
