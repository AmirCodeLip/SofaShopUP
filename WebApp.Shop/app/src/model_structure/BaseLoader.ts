
export type LoaderCommunicator = {
    makeSure: (txt:string) => Promise<boolean>;
    searchDrive?:React.RefObject<HTMLInputElement>
}

export class BaseLoader<TModel> {
    loaderCommunicator: LoaderCommunicator;
    model?: TModel;
}