import { EnvService } from './env.service';
import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class VersionService {
  baseUrl = environment.apiUrlEC;
  ModalNameSource = new BehaviorSubject<number>(0);
  currentModalName = this.ModalNameSource.asObservable();
  constructor(
    private http: HttpClient,
    public env: EnvService
  ) { }

  getAllVersion() {
    return this.http.get(this.env.apiUrl + 'Version/GetAll', {});
  }
  getById(id) {
    return this.http.get(this.env.apiUrl + 'Version/getById/' + id, {});
  }

  create(model) {
    return this.http.post(this.env.apiUrl + 'Version/Create', model);
  }
  update(model) {
    return this.http.put(this.env.apiUrl + 'Version/Update', model);
  }
  delete(id: number) {
    return this.http.delete(this.env.apiUrl + 'Version/Delete/' + id);
  }
}
