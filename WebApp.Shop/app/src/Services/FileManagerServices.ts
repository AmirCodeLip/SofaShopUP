import DataTransmitter from './DataTransmitter'
import FileManagerOnLoadData from './../webModels/FileManager/FileManagerOnLoadData'
import FObjectKind from './../webModels/FileManager/FObjectKind'
import FolderInfo from './../webModels/FileManager/FolderInfo'
import { OdataSetProtocol } from './OdataServices';
import { JsonResponse } from '../models/JsonResponse';
import { v4 as uuidv4 } from 'uuid';

type userRefType = {
    <T>(initialValue: T): React.MutableRefObject<T>;
    <T>(initialValue: T): React.RefObject<T>;
    <T = undefined>(): React.MutableRefObject<T>;
};

// import DataTransmitter from './Services/DataTransmitter'

export class FObjectKindComponent {
    constructor(model: FObjectKind, refObject: React.MutableRefObject<HTMLDivElement>) {
        this.id = uuidv4();
        this.model = model;
        this.refObject = refObject;
    }
    id: string;
    model: FObjectKind;
    refObject: React.MutableRefObject<HTMLDivElement>;

    private _selected: boolean = false;
    public get selected(): boolean {
        return this._selected;
    }
    public set selected(v: boolean) {
        let currentItem = this.refObject.current;
        if (currentItem !== null)
            if (v)
                currentItem.classList.add("selected-object-kind");
            else
                currentItem.classList.remove("selected-object-kind");
        this._selected = v;
    }


}

export async function load(createRef: userRefType, folderID: string | undefined) {
    let data = await new OdataSetProtocol<FObjectKind>(DataTransmitter.BaseUrl + "odata/FObjectKind" +
        (folderID === undefined ? "" : `?folderId=${folderID}`)).Execute({ authorize: true });
    let componentItems: Array<FObjectKindComponent> = [];
    for (let i of data) {
        i.FObjectType = i.FObjectType === "Folder" ? 1 : 0;
        componentItems.push(new FObjectKindComponent(i, createRef<HTMLDivElement>()))
    }
    return componentItems;
};



export async function FileManagerLoader() {
    return DataTransmitter.GetRequest<FileManagerOnLoadData>(DataTransmitter.BaseUrl + "FileManager/Base/FileManagerOnLoadData", { authorize: true });
}

export async function editForm(formData: FObjectKind) {
    return await DataTransmitter.PostRequest<JsonResponse<undefined>>(DataTransmitter.BaseUrl + "FileManager/Base/EditFObject", { authorize: true, body: formData });
}
