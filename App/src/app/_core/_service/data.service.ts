import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class DataService {
  messageSource = new BehaviorSubject<number>(0);
  SourceLang = new BehaviorSubject<any>('en');
  currentMessage = this.messageSource.asObservable();
  currentSourceLang = this.SourceLang.asObservable();

  messageSources = new BehaviorSubject<number>(0);
  messageSourcesTarget = new BehaviorSubject<number>(0);
  messageSourcesCheckbox = new BehaviorSubject<number>(0);
  messageSourcesTD = new BehaviorSubject<number>(0);
  messageSourcesRemove = new BehaviorSubject<any>(null);

  currentMessages = this.messageSources.asObservable();
  currentMessagesTarget = this.messageSourcesTarget.asObservable();
  currentMessagesCheckbox = this.messageSourcesCheckbox.asObservable();
  currentMessagesTD = this.messageSourcesTD.asObservable();
  currentMessagesEl = this.messageSourcesRemove.asObservable();

  public info = new BehaviorSubject<boolean>(false);
  locale = new BehaviorSubject<string>(null);
  // method này để change source message

  constructor() { }
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }

  changeMessageTarget(message) {
    this.messageSourcesTarget.next(message);
  }

  changeMessageCheckbox(message) {
    this.messageSourcesCheckbox.next(message);
  }

  changeMessageTD(message) {
    this.messageSourcesTD.next(message);
  }

  changeMessageRemoveEl(message) {
    this.messageSourcesRemove.next(message);
  }

  changeMessages(message) {
    this.messageSources.next(message);
  }

  changeLang(message) {
    this.SourceLang.next(message);
  }
  public setValue(message): void {
    this.info.next(message);
  }
  public getValue(): Observable<boolean> {
    return this.info.asObservable();
  }
  public setValueLocale(message): void {
    this.locale.next(message);
  }
  public getValueLocale(): Observable<string> {
    return this.locale.asObservable();
  }
}
