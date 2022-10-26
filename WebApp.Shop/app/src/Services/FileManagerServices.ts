import DataTransmitter from './DataTransmitter'
import FileManagerOnLoadData from './../webModels/FileManager/FileManagerOnLoadData'
import FObjectKind from './../webModels/FileManager/FObjectKind'
import { FObjectType } from './../webModels/FileManager/FObjectType'
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

export interface FObjectKindComponent {
    id: string,
    model: FObjectKind,
    refObject: React.MutableRefObject<HTMLDivElement>
}

export async function load(createRef: userRefType) {
    let data = await new OdataSetProtocol<FObjectKind>(DataTransmitter.BaseUrl + "odata/FObjectKind").Execute({ authorize: true });
    let componentItems: Array<FObjectKindComponent> = [];
    for (let i of (data)) {
        i.FObjectType = i.FObjectType === "Folder" ? 1 : 0;
        let item: FObjectKindComponent = {
            id: uuidv4(),
            model: i,
            refObject: createRef<HTMLDivElement>()
        };
        componentItems.push(item)
    }

    return componentItems;
};



export async function FileManagerLoader() {
    return DataTransmitter.GetRequest<FileManagerOnLoadData>(DataTransmitter.BaseUrl + "FileManager/Base/FileManagerOnLoadData", { authorize: true });
}

export async function editForm(formData: FolderInfo) {
    debugger;
    return await DataTransmitter.PostRequest<JsonResponse<undefined>>(DataTransmitter.BaseUrl + "FileManager/Base/EditFolder", { authorize: true, body: formData });
}
