import { HttpClient } from "@angular/common/http";
import { AppConfigService } from "./configurationService";
import { Observable } from "rxjs";
import { Project } from "../models/project";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  private apiUrl = this.appConfigService.API_SERVER_IP_ADDRESS + '/api/Project'; // Replace with your API URL

  constructor(
    private http: HttpClient,
    private appConfigService: AppConfigService,
  ) { }

  createProject(project: Project): Observable<any> {
    const projectData = {
      name: project.name,
      description: project.description,
      projectType: project.projectType,
      localImageLocation: project.localImageLocation,
      shortDescription: project.shortDescription,
    };
    return this.http.post(`${this.apiUrl}/`, projectData);
  }

  getProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(`${this.apiUrl}/`);
  }

  getProjectById(id: string): Observable<Project> {
    return this.http.get<Project>(`${this.apiUrl}/${id}`);
  }

  updateProject(project: Project): Observable<any> {
    return this.http.put(`${this.apiUrl}/`, project);
  }

  deleteProject(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

}


