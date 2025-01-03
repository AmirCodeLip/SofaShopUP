import DataTransmitter from './DataTransmitter'
import FileManagerOnLoadData from './../webModels/FileManager/FileManagerOnLoadData'
import FObjectKind from './../webModels/FileManager/FObjectKind'
import { OdataSetProtocol } from './OdataServices'
import { JsonResponse } from '../model_structure/JsonResponse'
import { v4 as uuidv4 } from 'uuid'
import * as globalManage from './../root/shared/GlobalManage'

type userRefType = {
    <T>(initialValue: T): React.MutableRefObject<T>;
    <T>(initialValue: T): React.RefObject<T>;
    <T = undefined>(): React.MutableRefObject<T>;
};

export class FObjectKindComponent {
    constructor(model: FObjectKind, refObject?: React.MutableRefObject<HTMLDivElement>) {
        this.id = uuidv4();
        this.model = model;
        this.refObject = refObject;
    }
    id: string;
    model: FObjectKind;
    refObject?: React.MutableRefObject<HTMLDivElement>;

    private _selected: boolean = false;
    public get selected(): boolean {
        return this._selected;
    }
    public set selected(v: boolean) {
        let currentItem = this.refObject!!.current;
        if (currentItem !== null)
            if (v)
                currentItem.classList.add("selected-object-kind");
            else
                currentItem.classList.remove("selected-object-kind");
        this._selected = v;
    }
}

export async function load(createRef: userRefType, folderID: string | undefined) {
    let rq = new OdataSetProtocol<FObjectKind>(DataTransmitter.BaseUrl + "odata/FObjectKind" +
        (folderID === undefined ? "" : `?folderId=${folderID}`));
    if (folderID === "images")
        rq.Where(x => x.TypeKind === "1");
    if (folderID === "videos")
        rq.Where(x => x.TypeKind === "2");
    if (folderID === "audios")
        rq.Where(x => x.TypeKind === "3");
    let data = await rq.Execute({ authorize: true });
    let componentItems: Array<FObjectKindComponent> = [];
    if (data != null)
        for (let i of data) {
            i.FObjectType = i.FObjectType === "Folder" ? 1 : 0;
            componentItems.push(new FObjectKindComponent(i, createRef<HTMLDivElement>()))
        }
    return componentItems;
};

//export async function loadSingle(id: string) {
// let data = await new OdataSetProtocol<FObjectKind>(DataTransmitter.BaseUrl + "odata/FObjectKind")).Execute({ authorize: true });
//}


export async function FileManagerLoader() {
    let cultureInfo = (await globalManage.CultureInfoImplement.Get())!!;
    cultureInfo.GetStrings("PublicWord001.key012", "PublicWord001.key013", "PublicWord001.key015", "PublicWord001.key016");
    return DataTransmitter.GetRequest<FileManagerOnLoadData>(DataTransmitter.BaseUrl + "FileManager/Base/FileManagerOnLoadData", { authorize: true });
}

export async function editForm(formData: FObjectKind) {
    return await DataTransmitter.PostRequest<JsonResponse<undefined>>(DataTransmitter.BaseUrl + "FileManager/Base/EditFObject", { authorize: true, body: formData });
}

