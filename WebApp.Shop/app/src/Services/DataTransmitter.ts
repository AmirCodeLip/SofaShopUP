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
        try {
            let request = await fetch(url, { headers: headers });
            let result: TOutput = (await request.json()) as TOutput;
            return result;
        } catch {
            return null;
        }
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
        try {

            let request = await fetch(url, requestInit);
            let result: TOutput = (await request.json()) as TOutput;
            return result;
        } catch (ex) {
            return null;
        }
    }

    static Upload<T>(url: string, file: File, data: any, options: UploadOptions = {}): Promise<T> {
        return new Promise<T>(function (resolve) {
            var fd = new FormData();
            fd.append("file", file);
            if (data)
                for (let key in data) {
                    fd.append(key, data[key])
                }
            var xhr = new XMLHttpRequest();
            xhr.open("POST", url, true);
            let jwt = localStorage.getItem("jwt");
            if (jwt == null) {
                window.location.replace("/identity/login");
                return;
            }
            xhr.setRequestHeader("Authorization", `Bearer ${jwt}`);
            xhr.upload.onprogress = function (e) {
                var percentComplete = Math.ceil((e.loaded / e.total) * 100);
                console.log(percentComplete);
            };
            xhr.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    let jsonData = JSON.parse(xhr.responseText);
                    resolve(jsonData);
                }
            }
            xhr.send(fd);
        })
    }
}

interface UploadOptions {

}

export interface DataTransmitterOptions {
    authorize?: boolean,
    body?: object
}