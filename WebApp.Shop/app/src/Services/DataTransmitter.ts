export default class DataTransmitter {
    static BaseUrl = "http://localhost:5220/";
    static async GetRequest<TOutput>(url: string) {
        let request = await fetch(url);
        let result: TOutput = (await request.json()) as TOutput;
        return result;
    }
    static async PostRequest<TOutput>(url: string, data: object) {
        let request = await fetch(url, {
            method: "POST",
            headers:
            {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });
        let result: TOutput = (await request.json()) as TOutput;
        return result;
    }
}