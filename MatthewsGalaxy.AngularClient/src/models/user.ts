export class IdentityUser {
  id: string;
  userName: string;
  email: string;
  emailConfirmed: boolean;
  roles: string[] | undefined;

  constructor(id: string, userName: string, email: string, emailConfirmed: boolean, roles?: string[]) {
    this.id = id;
    this.userName = userName;
    this.email = email;
    this.emailConfirmed = emailConfirmed;
    this.roles = roles;
  }

  getId(): string {
    return this.id;
  }

  setId(id: string): void {
    this.id = id;
  }

  getUserName(): string {
    return this.userName;
  }

  setUserName(userName: string): void {
    this.userName = userName;
  }

  getEmail(): string {
    return this.email;
  }

  setEmail(email: string): void {
    this.email = email;
  }

  getEmailConfirmed(): boolean {
    return this.emailConfirmed;
  }

  setEmailConfirmed(emailConfirmed: boolean): void {
    this.emailConfirmed = emailConfirmed;
  }

  getRole(): string[] | undefined {
    return this.roles;
  }

  setRole(roles: string[] | undefined): void {
    this.roles = roles;
  }

  toString(): string {
    return `IdentityUser: ${this.userName} (${this.email})`;
  }

  equals(IdentityUser: IdentityUser): boolean {
    return this.id === IdentityUser.id;
  }

  copy(): IdentityUser {
    return new IdentityUser(this.id, this.userName, this.email, this.emailConfirmed, this.roles);
  }

  toJson(): string {
    return JSON.stringify(this);
  }

  fromJson(json: string): IdentityUser {
    const obj = JSON.parse(json);
    return new IdentityUser(obj.id, obj.userName, obj.email, obj.emailConfirmed, obj.role);
  }

  static fromObject(obj: any): IdentityUser {
    return new IdentityUser(obj.id, obj.userName, obj.email, obj.emailConfirmed, obj.role);
  }

  toObject(): any {
    return {
      id: this.id,
      userName: this.userName,
      email: this.email,
      emailConfirmed: this.emailConfirmed,
      role: this.roles
    };
  }

  clone(): IdentityUser {
    return new IdentityUser(this.id, this.userName, this.email, this.emailConfirmed, this.roles);
  }

  validate(): boolean {
    return this.userName.length > 0 && this.email.length > 0;
  }

  static validate(IdentityUser: IdentityUser): boolean {
    return IdentityUser.userName.length > 0 && IdentityUser.email.length > 0;
  }
}
