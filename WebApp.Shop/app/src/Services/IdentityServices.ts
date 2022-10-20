import IFormModel from './../mylibraries/asp-communication/interfaces/IFormModel'
import DataTransmitter from './DataTransmitter'
import LoginModel from './../../webModels/LoginModel'
import LoginOkResult from '../../webModels/LoginOkResult';
import JsonResponse from '../models/JsonResponse';

export async function loginService(model: LoginModel) {
    let result = await DataTransmitter.PostRequest<JsonResponse<LoginOkResult>>(`${DataTransmitter.BaseUrl}Identity/PostLogin`, model);
    return result;
}

export async function LoginLoader() {
    return DataTransmitter.GetRequest<IFormModel>(DataTransmitter.BaseUrl + "Identity/GetLoginForm");
}

