import * as React from 'react'

export const supportedCultures = {
    enUS: "en-US",
    faIR: "fa-IR"
}


export const GetDefaultCulture = () => {
    window.location.href = "/" + supportedCultures.enUS;
    return (<></>)
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
