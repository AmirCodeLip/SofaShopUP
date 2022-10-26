export default class DataTransmitter {
    static BaseUrl = "http://localhost:5220/";
    static async GetRequest<TOutput>(url: string, options?: DataTransmitterOptions) {
        if (!options)
            options = {};
        let headers: HeadersInit =
        {
        };
        if (options.authorize) {
            let jwt = localStorage.getItem("jwt");
            if (jwt == null) {
                window.location.replace("/identity/login");
                return;
            }
            headers["Authorization"] = `Bearer ${jwt}`;
        }
        let request = await fetch(url, { headers: headers });
        let result: TOutput = (await request.json()) as TOutput;
        return result;
    }

    static async PostRequest<TOutput>(url: string, options?: DataTransmitterOptions) {
        if (!options)
            options = {};
        let headers: HeadersInit =
        {
            'Content-Type': 'application/json',
        };
        if (options.authorize) {
            let jwt = localStorage.getItem("jwt");
            if (jwt == null) {
                window.location.replace("/identity/login");
                return;
            }
            headers["Authorization"] = `Bearer ${jwt}`;
        }
        let requestInit: RequestInit = {
            method: "POST",
            headers: headers
        };
        if (options.body)
            requestInit.body = JSON.stringify(options.body);
        let request = await fetch(url, requestInit);
        let result: TOutput = (await request.json()) as TOutput;
        return result;
    }
}

export interface DataTransmitterOptions {
    authorize?: boolean,
    body?: object
}