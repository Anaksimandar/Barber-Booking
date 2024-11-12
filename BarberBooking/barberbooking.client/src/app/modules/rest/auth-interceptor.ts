import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem("token");

    if (!token) {
      return next.handle(req);
    }

    const newReq = req.clone({
      headers: req.headers.set("Authorization", `Bearer ${token}`)
    })
    console.log(newReq);
    return next.handle(newReq);
  }
}
