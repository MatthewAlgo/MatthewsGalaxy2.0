import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {
  public readonly API_SERVER_IP_ADDRESS = 'http://localhost:7263';

  constructor() {}
}
