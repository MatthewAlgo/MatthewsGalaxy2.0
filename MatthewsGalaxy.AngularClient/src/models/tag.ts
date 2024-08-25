export class Tag {
    id? : string;
    blogPost? : string;
    tagDescription? : string;
    tagName? : string;

    constructor(id?: string, blogPost?: string, tagDescription?: string, tagName?: string) {
        this.id = id;
        this.blogPost = blogPost;
        this.tagDescription = tagDescription;
        this.tagName = tagName;
    }

    // Getters and setters for everything

    getId(): string | undefined {
        return this.id;
    }

    setId(id?: string): void {
        this.id = id;
    }


    getBlogPost(): string | undefined {
        return this.blogPost;
    }

    setBlogPost(blogPost?: string): void {
        this.blogPost = blogPost;
    }

    getTagDescription(): string | undefined {
        return this.tagDescription;
    }

    setTagDescription(tagDescription?: string): void {
        this.tagDescription = tagDescription;
    }

    getTagName(): string | undefined {
        return this.tagName;
    }

    setTagName(tagName?: string): void {
        this.tagName = tagName;
    }

    // Other methods

    toString(): string {
        return `Tag: ${this.tagName} (${this.tagDescription})`;
    }

    equals(otherTag: Tag): boolean {
        return this.id === otherTag.id;
    }

    clone(): Tag {
        return new Tag(this.id, this.blogPost, this.tagDescription, this.tagName);
    }

    // FromJSON is a factory method that creates a Tag instance from a JSON object
    static fromJSON(json: any): Tag {
        return new Tag(json.id, json.blogPost, json.tagDescription, json.tagName);
    }

    // ToJSON is a method that converts a Tag instance to a JSON object

    toJSON(): any {
        return {
            id: this.id,
            blogPost: this.blogPost,
            tagDescription: this.tagDescription,
            tagName: this.tagName
        };
    }

    // Other methods

    static fromObject(obj: any): Tag {
        return new Tag(
            obj.id,
            obj.blogPost,
            obj.tagDescription,
            obj.tagName
        );
    }

    static toObject(tag: Tag): any {
        return {
            id: tag.id,
            blogPost: tag.blogPost,
            tagDescription: tag.tagDescription,
            tagName: tag.tagName
        };
    }
}
