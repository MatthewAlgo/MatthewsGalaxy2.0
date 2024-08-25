export class Article {
  computedHashId?: string;
  title?: string;
  shortDescription?: string;
  description?: string;
  publishedDate?: Date;
  views?: number;
  authorId?: string;
  authorName?: string;
  isVisible?: boolean;
  isFeatured?: boolean;
  likes?: number;
  featuredImageURL?: string;
  urlHandle?: string;
  tags?: string[];
  categories?: string[];

  constructor(
    computedHashId?: string,
    title?: string,
    shortDescription?: string,
    description?: string,
    publishedDate?: Date,
    views?: number,
    authorId?: string,
    isVisible?: boolean,
    isFeatured?: boolean,
    featuredImageURL?: string,
    urlHandle?: string,
    likes?: number,
    authorName?: string
  ) {
    this.computedHashId = computedHashId;
    this.title = title;
    this.shortDescription = shortDescription;
    this.description = description;
    this.publishedDate = publishedDate;
    this.views = views;
    this.likes = likes;
    this.authorId = authorId;
    this.isVisible = isVisible;
    this.isFeatured = isFeatured;
    this.featuredImageURL = featuredImageURL;
    this.urlHandle = urlHandle;
    this.authorName = authorName;
  }

  // Getters and setters for everything

  getComputedHashId(): string | undefined {
    return this.computedHashId;
  }

  setComputedHashId(computedHashId?: string): void {
    this.computedHashId = computedHashId;
  }

  getTitle(): string | undefined {
    return this.title;
  }

  setTitle(title?: string): void {
    this.title = title;
  }

  getShortDescription(): string | undefined {
    return this.shortDescription;
  }

  setShortDescription(shortDescription?: string): void {
    this.shortDescription = shortDescription;
  }

  getDescription(): string | undefined {
    return this.description;
  }

  setDescription(description?: string): void {
    this.description = description;
  }

  getPublishedDate(): Date | undefined {
    return this.publishedDate;
  }

  setPublishedDate(publishedDate?: Date): void {
    this.publishedDate = publishedDate;
  }

  getViews(): number | undefined {
    return this.views;
  }

  setViews(views?: number): void {
    this.views = views;
  }

  getAuthorId(): string | undefined {
    return this.authorId;
  }

  setAuthorId(authorId?: string): void {
    this.authorId = authorId;
  }

  getIsVisible(): boolean | undefined {
    return this.isVisible;
  }

  setIsVisible(isVisible?: boolean): void {
    this.isVisible = isVisible;
  }

  getIsFeatured(): boolean | undefined {
    return this.isFeatured;
  }

  setIsFeatured(isFeatured?: boolean): void {
    this.isFeatured = isFeatured;
  }

  getFeaturedImageURL(): string | undefined {
    return this.featuredImageURL;
  }

  setFeaturedImageURL(featuredImageURL?: string): void {
    this.featuredImageURL = featuredImageURL;
  }

  getUrlHandle(): string | undefined {
    return this.urlHandle;
  }

  setUrlHandle(urlHandle?: string): void {
    this.urlHandle = urlHandle;
  }

  getAuthorName(): string | undefined {
    return this.authorName;
  }

  setAuthorName(authorName?: string): void {
    this.authorName = authorName;
  }

  getLikes(): number | undefined {
    return this.likes;
  }

  setLikes(likes?: number): void {
    this.likes = likes;
  }

  getTags(): string[] | undefined {
    return this.tags;
  }

  setTags(tags?: string[]): void {
    this.tags = tags;
  }

  getCategories(): string[] | undefined {
    return this.categories;
  }

  setCategories(categories?: string[]): void {
    this.categories = categories;
  }

  // Other methods

  static fromObject(obj: any): Article {
    return new Article(
      obj.computedHashId,
      obj.title,
      obj.shortDescription,
      obj.description,
      obj.publishedDate,
      obj.views,
      obj.authorId,
      obj.isVisible,
      obj.isFeatured,
      obj.featuredImageURL,
      obj.urlHandle,
      obj.likes,
      obj.authorName
    );
  }

  toObject(): any {
    return {
      computedHashId: this.computedHashId,
      title: this.title,
      shortDescription: this.shortDescription,
      description: this.description,
      publishedDate: this.publishedDate,
      views: this.views,
      authorId: this.authorId,
      isVisible: this.isVisible,
      isFeatured: this.isFeatured,
      featuredImageURL: this.featuredImageURL,
      urlHandle: this.urlHandle,
      likes: this.likes,
      authorName: this.authorName,
    };
  }
}
