export default class JsonResponse<TResult> {
    status: JsonResponseStatus;
    infoData: { [k in string]: string };
    tResult001: TResult
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
