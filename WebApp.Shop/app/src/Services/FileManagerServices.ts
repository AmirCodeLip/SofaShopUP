import DataTransmitter from './DataTransmitter'
import IFormModel from './../mylibraries/asp-communication/interfaces/IFormModel'


export async function FileManagerLoader() {
    return DataTransmitter.GetRequest<IFormModel>(DataTransmitter.BaseUrl + "Identity/GetLoginForm");
}
