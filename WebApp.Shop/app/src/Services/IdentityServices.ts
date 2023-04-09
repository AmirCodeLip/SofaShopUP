import IFormModel from './../mylibraries/asp-communication/interfaces/IFormModel'
import DataTransmitter from './DataTransmitter'
import LoginModel from './../webModels/LoginModel'
import LoginOkResult from './../webModels/LoginOkResult';
import { JsonResponse } from '../model_structure/JsonResponse';

export async function loginService(model: LoginModel) {
    let result = await DataTransmitter.PostRequest<JsonResponse<LoginOkResult>>(`${DataTransmitter.BaseUrl}Identity/PostLogin`, { body: model });
    return result;
}

export async function registerService(model: LoginModel) {
    let result = await DataTransmitter.PostRequest<JsonResponse<LoginOkResult>>(`${DataTransmitter.BaseUrl}Identity/PostRegister`, { body: model });
    return result;
}

export async function identityLoader() {
    return DataTransmitter.GetRequest<IFormModel>(DataTransmitter.BaseUrl + "Identity/GetIdentityForm");
}

export interface identityLoaderModel {
    LoginModel: IFormModel;
    RegisterModel: IFormModel;
}