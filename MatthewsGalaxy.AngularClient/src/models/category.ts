import { Article } from "./article";

export class Category {
  id? : string;
  description? : string;
  name? : string;
  urlHandle? : string;
  numberOfArticles? : number;
  correspondingArticles? : Article[];

  constructor(id?: string, description?: string, name?: string, urlHandle?: string) {
    this.id = id;
    this.description = description;
    this.name = name;
    this.urlHandle = urlHandle;
  }

  // Getters and setters for everything

  getId(): string | undefined {
    return this.id;
  }

  setId(id?: string): void {
    this.id = id;
  }

  getDescription(): string | undefined {
    return this.description;
  }

  setDescription(description?: string): void {
    this.description = description;
  }

  getName(): string | undefined {
    return this.name;
  }

  setName(name?: string): void {
    this.name = name;
  }

  getUrlHandle(): string | undefined {
    return this.urlHandle;
  }

  setUrlHandle(urlHandle?: string): void {
    this.urlHandle = urlHandle;
  }

  getNumberOfArticles(): number | undefined {
    return this.numberOfArticles;
  }

  setNumberOfArticles(numberOfArticles?: number): void {
    this.numberOfArticles = numberOfArticles;
  }

  getCorrespondingArticles(): Article[] | undefined {
    return this.correspondingArticles;
  }

  setCorrespondingArticles(correspondingArticles?: Article[]): void {
    this.correspondingArticles = correspondingArticles;
  }

  // Other methods

  toString(): string {
    return `Category: ${this.name} (${this.description})`;
  }

  equals(otherCategory: Category): boolean {
    return this.id === otherCategory.id;
  }

  clone(): Category {
    return new Category(this.id, this.description, this.name, this.urlHandle);
  }

  static fromJson(json: any): Category {
    return new Category(json.id, json.description, json.name, json.urlHandle);
  }

  static toJson(category: Category): any {
    return {
      id: category.id,
      description: category.description,
      name: category.name,
      urlHandle: category.urlHandle
    };
  }

  static fromObject(obj: any): Category {
    return new Category(
      obj.id,
      obj.description,
      obj.name,
      obj.urlHandle
    );
  }

  static toObject(category: Category): any {
    return {
      id: category.id,
      description: category.description,
      name: category.name,
      urlHandle: category.urlHandle
    };
  }


  static fromJsonArray(json: any[]): Category[] {
    return json.map((value) => Category.fromJson(value));
  }

  static toJsonArray(categories: Category[]): any[] {
    return categories.map((value) => Category.toJson(value));
  }

  static fromObjectArray(objs: any[]): Category[] {
    return objs.map((value) => Category.fromObject(value));
  }

  static toObjectArray(categories: Category[]): any[] {
    return categories.map((value) => Category.toObject(value));
  }

  static fromJsonAny(json: any): any {
    return Category.fromJson(json);
  }

  static toJsonAny(category: Category): any {
    return Category.toJson(category);
  }

  static fromObjectAny(obj: any): any {
    return Category.fromObject(obj);
  }

  static toObjectAny(category: Category): any {
    return Category.toObject(category);
  }

  static fromJsonArrayAny(json: any[]): any[] {
    return Category.fromJsonArray(json);
  }

  static toJsonArrayAny(categories: Category[]): any[] {
    return Category.toJsonArray(categories);
  }


}
