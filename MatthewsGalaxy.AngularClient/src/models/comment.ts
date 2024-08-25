import { IdentityUser } from "./user";

export class Comment {
  id: string;
  blogPostId: string;
  title: string;
  date: Date;
  content: string;
  authorId: string;
  author: IdentityUser | undefined;
  authorName?: string;
  isLoggedUsersComment: boolean = false;

  constructor(id: string, blogPostId: string, title: string, date: Date, content: string, authorId: string, author?: IdentityUser, isLoggedUsersComment: boolean = false) {
    this.id = id;
    this.blogPostId = blogPostId;
    this.title = title;
    this.date = date;
    this.content = content;
    this.authorId = authorId;
    this.authorName = author?.userName;
    this.author = author;
    this.isLoggedUsersComment = isLoggedUsersComment;
  }

  // Getters and setters for everything
  getId(): string {
    return this.id;
  }

  setId(id: string): void {
    this.id = id;
  }

  getBlogPostId(): string {
    return this.blogPostId;
  }

  setBlogPostId(blogPostId: string): void {
    this.blogPostId = blogPostId;
  }

  getTitle(): string {
    return this.title;
  }

  setTitle(title: string): void {
    this.title = title;
  }

  getDate(): Date {
    return this.date;
  }

  setDate(date: Date): void {
    this.date = date;
  }

  getContent(): string {
    return this.content;
  }

  setContent(content: string): void {
    this.content = content;
  }

  getAuthorId(): string {
    return this.authorId;
  }

  getAuthorName(): string | undefined {
    return this.authorName;
  }

  setAuthorName(authorName: string): void {
    this.authorName = authorName;
  }

  setAuthorId(authorId: string): void {
    this.authorId = authorId;
  }

  getAuthor(): IdentityUser | undefined {
    return this.author;
  }

  setAuthor(author: IdentityUser): void {
    this.author = author;
  }

  // Other methods
  toString(): string {
    return `Comment: ${this.title} - ${this.content}`;
  }

  fromObject(obj: any): Comment {
    'id' in obj ? this.id = obj.id : this.id = '';
    'blogPostId' in obj ? this.blogPostId = obj.blogPostId : this.blogPostId = '';
    'title' in obj ? this.title = obj.title : this.title = '';
    'date' in obj ? this.date = obj.date : this.date = new Date();
    'content' in obj ? this.content = obj.content : this.content = '';
    'authorId' in obj ? this.authorId = obj.authorId : this.authorId = '';
    'authorName' in obj ? this.authorName = obj.authorName : this.authorName = '';
    'author' in obj ? this.author = obj.author : this.author = undefined;
    'isLoggedUsersComment' in obj ? this.isLoggedUsersComment = obj.isLoggedUsersComment : this.isLoggedUsersComment = false
    return this;
  }

  toObject(): any {
    return {
      id: this.id,
      blogPostId: this.blogPostId,
      title: this.title,
      date: this.date,
      content: this.content,
      authorId: this.authorId,
      authorName: this.authorName,
    };
  }

  static fromObject(obj: any): Comment {
    return new Comment(obj.id, obj.blogPostId, obj.title, obj.date, obj.content, obj.authorId, obj.author);
  }

  toJson(): string {
    return JSON.stringify(this);
  }

  equals(comment: Comment): boolean {
    return this.blogPostId === comment.blogPostId;
  }

  copy(): Comment {
    return new Comment(this.id, this.blogPostId, this.title, this.date, this.content, this.authorId, this.author, this.isLoggedUsersComment);
  }

  fromJson(json: string): Comment {
    const obj = JSON.parse(json);
    return new Comment(this.id, obj.blogPostId, obj.title, obj.date, obj.content, obj.authorId, obj.author, obj.isLoggedUsersComment);
  }

}
