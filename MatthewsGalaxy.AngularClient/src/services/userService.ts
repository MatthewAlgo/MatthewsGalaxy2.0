import { Injectable } from "@angular/core";
import { AuthenticationService } from "./authenticationService";
import { AppConfigService } from "./configurationService";
import { HttpClient } from "@angular/common/http";
import { IdentityUser } from "../models/user";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor( private http: HttpClient, private appConfigService: AppConfigService, private authenticationService: AuthenticationService) { }

  // Implement the getCurrentUser() method
  private apiUrl = this.appConfigService.API_SERVER_IP_ADDRESS + '/api/Users'; // Replace with your API URL


  // Get the current logged in user
  getCurrentUser(): Observable<IdentityUser> {
    return this.http.get<IdentityUser>(this.apiUrl + '/current_user', );
  }

  getAllUsers(): Observable<IdentityUser[]> {
    return this.http.get<IdentityUser[]>(this.apiUrl);
  }

  removeUser(id: string) : Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}
