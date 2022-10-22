import DataTransmitter from './DataTransmitter'
import FileManagerOnLoadData from './../../webModels/FileManager/FileManagerOnLoadData'


export async function FileManagerLoader() {
    return DataTransmitter.GetRequest<FileManagerOnLoadData>(DataTransmitter.BaseUrl + "FileManager/Base/FileManagerOnLoadData");
}
