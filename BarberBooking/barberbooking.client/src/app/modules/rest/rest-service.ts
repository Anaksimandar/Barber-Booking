import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, catchError, throwError } from "rxjs";
import { enviroment } from '../../../enviroments/enviroment';
@Injectable({
  providedIn: 'root'
})
export class RestService {
  private readonly apiUrl: string | null;
  private httpOptions!: object;
  constructor(private httpClient:HttpClient) {
    this.apiUrl = enviroment.apiBaseUrl;
    this.httpOptions = {
      Headers: new HttpHeaders({
        'Content-Type':'application/json'
      })
    }
  }

  get(endpoint: string):Observable<any[]> { // reservation/service-type/account
    return this.httpClient.get<any[]>(`${this.apiUrl}/${endpoint}`).pipe(
      catchError(this.errorHandler)
    )
  }

  getById(endpoint: string, id: number) {
    return this.httpClient.get<any>(`${this.apiUrl}/${endpoint}/${id}`).pipe(
      catchError(this.errorHandler)
    )
  }

  post(endpoint: string, body: any):Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/${endpoint}`, JSON.stringify(body), this.httpOptions).pipe(
      catchError(this.errorHandler)
    )
  }

  update(endpoint: string, id: number | null, body: any):Observable<any> {
    return this.httpClient.put<any>(`${this.apiUrl}/${endpoint}/${id}`, body, this.httpOptions).pipe(
      catchError(this.errorHandler)
    )
  }

  delete(endpoint: string, id: number) {
    return this.httpClient.delete(`${this.apiUrl}/${endpoint}/${id}`, this.httpOptions).pipe(
      catchError(this.errorHandler)
    )
  }

  errorHandler(error: any) {
    let errorMessage = '';
    if (error instanceof HttpErrorResponse) {
      errorMessage = error.error.message;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(errorMessage);
  }
}
