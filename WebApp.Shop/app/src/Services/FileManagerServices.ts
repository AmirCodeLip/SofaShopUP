import DataTransmitter from './DataTransmitter'
import FileManagerOnLoadData from './../../webModels/FileManager/FileManagerOnLoadData'
import FObjectKind from './../../webModels/FileManager/FObjectKind'
import { OdataSetProtocol } from './OdataServices';
import { async } from 'rxjs';
// import DataTransmitter from './Services/DataTransmitter'

export async function load() {
    return await new OdataSetProtocol<FObjectKind>(DataTransmitter.BaseUrl + "odata/FObjectKind").Execute();
};
export async function FileManagerLoader() {
    return DataTransmitter.GetRequest<FileManagerOnLoadData>(DataTransmitter.BaseUrl + "FileManager/Base/FileManagerOnLoadData");
}
