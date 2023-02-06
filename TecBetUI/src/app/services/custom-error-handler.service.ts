import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable, Injector } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CustomErrorHandlerService implements ErrorHandler {

  constructor(private injector: Injector) { }

  handleError(error: Error | HttpErrorResponse) {
    let message: string;

    if (error instanceof HttpErrorResponse) {
      // message = `Error Status: ${error.status}\nMessage: ${error.message.split('for https://localhost:7276')[0]}`;
      message = `${error.message.split('for https://localhost:7142')[0]}`;
    } else {
      message = error.message ? error.message : error.toString();
    }

    return message
  }
}
