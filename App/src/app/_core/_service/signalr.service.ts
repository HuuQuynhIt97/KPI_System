import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { EnvService } from './env.service';
import { Injectable } from '@angular/core';
import { PaginatedResult } from '../_model/pagination';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  public hubConnection: HubConnection;
  private connectionUrl = environment.hub;
  baseUrl = environment.apiUrl;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(
    public env: EnvService,
    private alertify: AlertifyService
    ) { }

    
    public connect = () => {
      this.startConnection();
    }
    public close = async () => {
       return await this.hubConnection.stop();
    }

    public loadData = () => {
      this.hubConnection.on('ReceiveMessage', (result, message) => {
        this.messageSource.next(result)
      });
    }
    
    public startConnection = () => {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(this.connectionUrl)
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();
      this.hubConnection.start().then(() => console.log('Connection started')).catch(err =>{
      })
    }
    // startConnection = async () => {
    //   this.hubConnection = new HubConnectionBuilder()
    //     .withUrl(this.connectionUrl)
    //     .build();
    //   this.setSignalrClientMethods();
    //   try {
    //     await this.hubConnection.start();
    //     console.log('connect tione');
    //   } catch (error) {
    //     setTimeout(async () => {
    //       await this.startConnection();
    //     }, 5000);
    //   }
    // }
    
    // This method will implement the methods defined in the ISignalrDemoHub inteface in the SignalrDemo.Server .NET solution
    private setSignalrClientMethods(): void {
      this.hubConnection.onreconnected(() => {
        console.log('Restarted signalr!');
      });
      // this.hubConnection.on('Online', (numberOfUser: any) => {
      //   this.online.next(numberOfUser);
      // });
      // this.hubConnection.on('UserOnline', (userNames: any) => {
      //   const userNameList = JSON.stringify(userNames);
      //   localStorage.setItem('userOnline', userNameList);
      //   this.userNames.next(userNames);
      // });
    }

}
