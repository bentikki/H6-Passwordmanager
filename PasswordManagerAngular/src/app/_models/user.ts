export class User {
    id: number;
    username: string;
    tokenSet?: TokenSet;
}

export class TokenSet {
    accessToken : string;
}