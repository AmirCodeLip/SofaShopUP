import * as React from 'react'
import PVInfoModel from '../../webModels/PVInfoModel'
import CultureInfo from '../../webModels/CultureInfo'
import DataTransmitter from '../../Services/DataTransmitter'
import { KeyValuePair, JsonResponse } from '../../models/JsonResponse'
import { stringify } from 'uuid'

declare global {
    interface Window {
        cultureInfo: undefined | CultureInfo; // whatever type you want to give. (any,number,float etc)
    }
}

export const supportedCultures = {
    enUS: "en-US",
    faIR: "fa-IR"
}

export const GetDefaultCulture = () => {
    window.location.href = "/" + supportedCultures.enUS;
    return (<></>)
}

export function localizorHtml(prop: { txtKey: string }) {
    if (window.cultureInfo) {
        let data = window.cultureInfo;
        // return null;
    }
    return (<span {...{ "not-localized": `${prop.txtKey}}` }}>
        ...
    </span>)
}

export class UrlParser {
    public static getUrlData() {
        let data = window.location.href.split('/').filter((v, i) => v != '' && 2 < i);
        let urlData: UrlData = {
            culture: data[0],
            data: data.filter((v, i) => 0 != i)
        };
        return urlData;
    }
}

export interface UrlData {
    culture: string,
    data: Array<string>
}

export class CultureInfoImplement implements CultureInfo {
    static keyItem = "lang_Data";
    Rtl: boolean;
    Culture: string;
    Version: string;
    data: Array<KeyValuePair<string, string>>;

    constructor(cultureInfo: CultureInfo) {
        if (cultureInfo) {
            this.Culture = cultureInfo.Culture;
            this.Rtl = cultureInfo.Rtl;
            this.Version = cultureInfo.Version;
        }
    }

    GetString(txtKey: string) {
        let data = this.data.find(x => x.Key === txtKey);
        debugger;
        // if (data) {


        // }
    }

    static async Get() {
        let infoSalt = await cookies.pVInfoSetProcess();
        let info = await cookies.parseInfo(infoSalt);
        let ci = localStorage.getItem(this.keyItem);
        let ciData: CultureInfoImplement | null = ci == null ? null : JSON.parse(ci);
        if (ci == null) {
            let response = await DataTransmitter.PostRequest<CultureInfo>(DataTransmitter.BaseUrl + `CultureManager/GetCultureInfo`, {
                body: { Item1: info.Language }
            });
            ciData = new CultureInfoImplement(response);
            localStorage.setItem(this.keyItem, JSON.stringify(ciData));
        }
        // if (!ci) {

        // }
    }
}

export class cookies {
    static keyItem = "pv_info";

    static setCookie(cname: string, cvalue: string, exdays: number) {
        const d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        let expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    }

    static getCookie(cname: string) {
        let name = cname + "=";
        let ca = document.cookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return null;
    }

    static async pVInfoSetProcess() {
        let info = cookies.getCookie(this.keyItem);
        if (info == null) {
            let data: PVInfoModel = { Language: supportedCultures.faIR };
            let response = await DataTransmitter.PostRequest<JsonResponse<string>>(DataTransmitter.BaseUrl + `PVInfo/Set`, {
                body: data
            });
            info = response.TResult001;
            cookies.setCookie(this.keyItem, response.TResult001, 30);
        }
        return info;
    }

    static async parseInfo(data: string) {
        let response = await DataTransmitter.PostRequest<JsonResponse<PVInfoModel>>(DataTransmitter.BaseUrl + `PVInfo/Get`, {
            body: { Item1: data }
        });
        return response.TResult001;
    }
}
