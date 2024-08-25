import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AppConfigService } from "./configurationService";
import { IdentityUser } from "../models/user";

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  private apiUrl = this.appConfigService.API_SERVER_IP_ADDRESS + '/api/Home'; // Replace with your API URL

  constructor(private http: HttpClient,
    private appConfigService : AppConfigService
  ) { }

  getHomeContent(): any {
    return this.http.get<any>(this.apiUrl);
  }

  getAboutMeContent(): any {
    return this.http.get<any>(this.apiUrl + '/about');
  }

  getCategories(): any {
    return this.http.get<any>(this.apiUrl + '/categories');
  }
}


