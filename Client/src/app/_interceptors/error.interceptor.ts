import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((e: HttpErrorResponse) => {
        if (e) {
          switch (e.status) {
            case 400:
              // Handle cases of special 400 bad requests (i.e., validation errors)
              if (e.error.errors) {
                const modelStateErrors = [];
                for (const key in e.error.errors) {
                  if (e.error.errors[key]) {
                    modelStateErrors.push(e.error.errors[key])
                  }
                }
                throw modelStateErrors.flat();
              } else {
                this.toastr.error(e.error, e.status.toString());
              }
              break;

            case 401:
              this.toastr.error('Unauthorized', e.status.toString());
              break;

            case 404:
              this.router.navigateByUrl('/not-found');
              break;

            case 500:
              const navigationExtras: NavigationExtras = {state: {error: e.error}};
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;

            default:
              this.toastr.error('Oops! Something went wrong!');
              console.log(e);
              break;
          }
        }
        throw e;
      })
    );
  }
}
