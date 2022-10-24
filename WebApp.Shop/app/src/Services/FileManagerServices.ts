import DataTransmitter from './DataTransmitter'
import FileManagerOnLoadData from './../../webModels/FileManager/FileManagerOnLoadData'
import FObjectKind from './../../webModels/FileManager/FObjectKind'
import { OdataSetProtocol } from './OdataServices';
import { JsonResponse } from '../models/JsonResponse';
// import DataTransmitter from './Services/DataTransmitter'
import FolderInfo from '../../webModels/FileManager/FolderInfo'
export async function load() {
    return await new OdataSetProtocol<FObjectKind>(DataTransmitter.BaseUrl + "odata/FObjectKind").Execute({ authorize: true });
};

export async function FileManagerLoader() {
    return DataTransmitter.GetRequest<FileManagerOnLoadData>(DataTransmitter.BaseUrl + "FileManager/Base/FileManagerOnLoadData", { authorize: true });
}

export async function editForm(formData: FolderInfo) {
    debugger;
    return await DataTransmitter.PostRequest<JsonResponse<undefined>>(DataTransmitter.BaseUrl + "FileManager/Base/EditFolder", { authorize: true, body: formData });
}
