import * as React from 'react'
import PVInfoModel from '../../webModels/PVInfoModel'
import CultureInfo from '../../webModels/CultureInfo'
import DataTransmitter from '../../Services/DataTransmitter'
import { KeyValuePair, JsonResponse } from '../../models/JsonResponse'
import { stringify } from 'uuid'
declare global {
    interface Window {
        cultureInfo: undefined | CultureInfoImplement;
        pVInfo: PVInfoModel;
    }
}
var lastTransmitWord: Date = null;

export const supportedCultures = {
    enUS: "en-US",
    faIR: "fa-IR"
}

export const supportedCultureList: Array<string> = [supportedCultures.enUS, supportedCultures.faIR];

function getDefault() {
    window.location.href = "/" + supportedCultures.enUS + window.location.pathname
}

export const GetDefaultCulture = () => {
    getDefault();
    return (<></>)
}

export function localizorHtml(prop: { txtKey: string }) {
    if (window.cultureInfo) {
        let data = window.cultureInfo;
        let word = data.GetString(prop.txtKey);
        if (word != null) {
            return (<p>{word}</p>);
        }
    }
    return (<span {...{ "not-localized": `${prop.txtKey}` }}>
        ...
    </span>)
}

export class UrlData {
    constructor() {
        this.update();
    }
    update() {
        let data = window.location.href.split('/').filter((v, i) => v != '' && 2 < i);
        this.culture = data[0];
        if (!supportedCultureList.includes(this.culture)) {
            this.culture = undefined;
            data = [undefined, ...data];
        }
        this.data = data.filter((v, i) => 0 != i);
        this.id = data.length >= 3 ? data[2] : null;
    }
    culture: string;
    data: Array<string>;
    id: string;
}

export class CultureInfoImplement {
    static keyItem = "lang_Data";
    data: Array<KeyValuePair<string, string>>;
    promiseWord: Array<string>;
    inProgress: boolean;
    cultureInfo: CultureInfo;
    constructor(cultureInfo: CultureInfo) {
        if (cultureInfo) {
            this.cultureInfo = cultureInfo;
            this.data = [];
            this.promiseWord = [];
            this.inProgress = false;
            setInterval(() => this.TransmitWord(), 3000);
        }
    }

    flush() {
        let listDt = [];
        for (let i of this.promiseWord) {
            if (!this.data.find(x => x.Key == i)) {
                listDt.push(i);
            }
        }
        this.promiseWord = listDt;
    }

    async TransmitWord() {
        try {
            let lastTry = new Date();
            if (lastTransmitWord === null)
                lastTransmitWord = lastTry;
            else {
                let diff = (lastTry.getTime() - (lastTransmitWord!!).getTime());
                if (diff < 2000)
                    return;
                lastTransmitWord = lastTry;
            }
            if (this.inProgress)
                return;
            this.inProgress = true;
            this.flush();
            new Date()
            await this.LoadList(this.promiseWord);
            document.querySelectorAll("[not-localized]").forEach(notLocalized => {
                let key = notLocalized.getAttribute("not-localized");
                let dt = this.data.find(x => x.Key == key);
                if (dt && notLocalized) {
                    try {
                        notLocalized.parentElement.replaceWith(notLocalized, document.createTextNode(dt.Value));
                        notLocalized.remove();
                        // notLocalized.parentElement.appendChild(document.createTextNode(dt.Value))
                    } catch (e) {
                        debugger;
                    }
                }
            });
        }
        finally {
            this.inProgress = false;
        }
    }

    async LoadList(words: Array<string>) {
        if (words.length === 0)
            return;
        let data = await DataTransmitter.PostRequest<JsonResponse<Array<KeyValuePair<string, string>>>>(`${DataTransmitter.BaseUrl}CultureManager/GetList`, {
            body: {
                Words: words,
                CultureInfo: this.cultureInfo
            }
        });
        data.TResult001.forEach(x => {
            this.data.push(x);
        });
    }

    public GetStrings(...params: Array<string>) {
        for (let txtKey of params) {
            this.GetString(txtKey);
        }
    }

    public GetString(txtKey: string): string | null {
        let data = this.data.find(x => x.Key === txtKey);
        if (data) {
            return data.Value;
        }
        else {
            if (!this.promiseWord.find(x => x == txtKey))
                this.promiseWord.push(txtKey);
            return null;
        }
    }

    static async Get() {
        if (window.cultureInfo)
            return window.cultureInfo;
        let infoSalt = await cookies.pVInfoSetProcess();
        let info = await cookies.parseInfo(infoSalt);
        if (!supportedCultureList.includes(info.Language)) {
            getDefault();
            return;
        }
        if (!window.cultureInfo) {
            let response = await DataTransmitter.PostRequest<CultureInfo>(DataTransmitter.BaseUrl + `CultureManager/GetCultureInfo`, {
                body: info
            });
            window.cultureInfo = new CultureInfoImplement(response);
            return window.cultureInfo;
        }
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

    static pVInfoGetRaw() {
        let info = localStorage.getItem(this.keyItem);
        return info;
    }

    static pVInfoSetRaw(token: string) {
        localStorage.setItem(this.keyItem, token);
    }

    static async pVInfoSetProcess(newInfo?: PVInfoModel) {
        let info = localStorage.getItem(this.keyItem);
        // let info = cookies.getCookie(this.keyItem);
        if (info == null) {
            let urlData = new UrlData();
            if (!urlData.culture) {
                getDefault();
                return;
            }
            let data: PVInfoModel = { Language: urlData.culture, UserInfoList: [] };
            let response = await DataTransmitter.PostRequest<JsonResponse<string>>(DataTransmitter.BaseUrl + `PVInfo/Set`, {
                body: data
            });
            info = response.TResult001;
            localStorage.setItem(this.keyItem, response.TResult001);
            // cookies.setCookie(this.keyItem, response.TResult001, 30);
        }
        else if (newInfo) {
            let response = await DataTransmitter.PostRequest<JsonResponse<string>>(DataTransmitter.BaseUrl + `PVInfo/Set`, {
                body: newInfo
            });
            info = response.TResult001;
            localStorage.setItem(this.keyItem, response.TResult001);
            // cookies.setCookie(this.keyItem, response.TResult001, 30);
        }
        return info;
    }

    static async parseInfo(data?: string) {
        if (!data) {
            if (window.pVInfo)
                return window.pVInfo;
            else return window.pVInfo;
        }
        let response = await DataTransmitter.PostRequest<JsonResponse<PVInfoModel>>(DataTransmitter.BaseUrl + `PVInfo/Get`, {
            body: { Item1: data }
        });
        let urlData = new UrlData();
        if (urlData.culture && urlData.culture !== response.TResult001.Language) {
            response.TResult001.Language = urlData.culture;
            await this.pVInfoSetProcess(response.TResult001);
            window.location.reload();
        }
        window.pVInfo = response.TResult001;
        return response.TResult001;
    }
}
