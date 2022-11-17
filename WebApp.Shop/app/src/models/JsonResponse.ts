export class JsonResponse<TResult> {
    Status: JsonResponseStatus;
    InfoData: { [k in string]: string };
    TResult001: TResult
}

export interface KeyValuePair<TKey, TValue> {
    Key: TKey;
    Value: TValue;
}

export enum JsonResponseStatus {
    Success,
    HaveError,
    NotFound
}
