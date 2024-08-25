export class Project {

  id?: string;
  name?: string;
  description?: string;
  projectType?: string;
  localImageLocation?: string;
  tags?: string[];
  shortDescription?: string;

  constructor(id?: string, name?: string, description?: string, projectType?: string, localImageLocation?: string, tags?: string[], shortDescription?: string) {
      this.id = id;
      this.name = name;
      this.description = description;
      this.projectType = projectType;
      this.localImageLocation = localImageLocation;
      this.tags = tags || [];
      this.shortDescription = shortDescription;
  }

  getId(): string | undefined {
      return this.id;
  }

  setId(id: string): void {
      this.id = id;
  }

  getName(): string | undefined {
      return this.name;
  }

  setName(name: string): void {
      this.name = name;
  }

  getDescription(): string | undefined {
      return this.description;
  }

  setDescription(description: string): void {
      this.description = description;
  }

  getProjectType(): string | undefined {
      return this.projectType;
  }

  setProjectType(projectType: string): void {
      this.projectType = projectType;
  }

  getlocalImageLocation(): string | undefined {
      return this.localImageLocation;
  }

  setlocalImageLocation(localImageLocation: string): void {
      this.localImageLocation = localImageLocation;
  }

  getTags(): string[] | undefined {
      return this.tags;
  }

  setTags(tags: string[]): void {
      this.tags = tags;
  }

  addTag(tag: string): void {
      this.tags?.push(tag);
  }
  getShortDescription(): string | undefined {
      return this.shortDescription;
  }

  isValid() {
      return this.name && this.description && this.projectType && this.localImageLocation && this.tags;
  }

  setShortDescription(shortDescription: string): void {
      this.shortDescription = shortDescription;
  }

  removeTag(tag: string): void {
      const index = this.tags?.indexOf(tag);
      if (index! > -1) {
          this.tags?.splice(index!, 1);
      }
  }

  hasTag(tag: string): boolean | undefined {
      return this.tags?.includes(tag);
  }

  toString(): string {
      return `Project: { id: ${this.id}, name: ${this.name}, description: ${this.description}, projectType: ${this.projectType}, tags: ${this.tags?.join(', ')}
      , localImageLocation: ${this.localImageLocation}, shortDescription: ${this.shortDescription}}`;
  }

  equals(project: Project): boolean {
      return this.id === project.id;
  }

  clone(): Project {
      return new Project(this.id, this.name, this.description, this.projectType, this.localImageLocation, [...this.tags!]);
  }

  static fromJSON(json: any): Project {
      return new Project(json.id, json.name, json.description, json.projectType, json.localImageLocation, json.tags);
  }

  static toJSON(project: Project): any {
      return {
          id: project.id,
          name: project.name,
          description: project.description,
          projectType: project.projectType,
          localImageLocation: project.localImageLocation,
          tags: project.tags,
          shortDescription: project.shortDescription
      };
  }

  static fromJSONArray(json: any[]): Project[] {
      return json.map(Project.fromJSON);
  }

  static toJSONArray(projects: Project[]): any[] {
      return projects.map(Project.toJSON);
  }

  static fromString(json: string): Project {
      return Project.fromJSON(JSON.parse(json));
  }

  static toString(project: Project): string {
      return JSON.stringify(Project.toJSON(project));
  }

  static fromStringArray(json: string): Project[] {
      return Project.fromJSONArray(JSON.parse(json));
  }

  static toStringArray(projects: Project[]): string {
      return JSON.stringify(Project.toJSONArray(projects));
  }

  static compare(a: Project, b: Project): number {
      return (a.name || "").localeCompare(b.name || "");
  }

  static equals(a: Project, b: Project): boolean {
      return a.equals(b);
  }

  static clone(project: Project): Project {
      return project.clone();
  }
}
