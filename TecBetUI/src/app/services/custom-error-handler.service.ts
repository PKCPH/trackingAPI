import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable, Injector } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CustomErrorHandlerService implements ErrorHandler {

  constructor(private injector: Injector) { }

//Strictly made to cut out the localhost part of the http request error, was not particularly needed to make a CustomErrorHandler but later on when we have more errors
//And different functions it'll be nice to continue working on this. Works for now though but only handles 1 specific error.

  handleError(error: Error | HttpErrorResponse) {
    let message: string;

    if (error instanceof HttpErrorResponse) {
      message = `${error.message.split('for https://localhost:5001')[0]}`;
    } else {
      message = error.message ? error.message : error.toString();
    }

    return message
  }
}
