import DataTransmitter from './../../Services/DataTransmitter'
import { JsonResponseStatus, JsonResponse } from './../../models/JsonResponse';
import { UrlData } from './../shared/GlobalManage';

export default class UploadHandler {
    driveBar: React.RefObject<HTMLDivElement>;
    queryString: UrlData;
    constructor(driveBar: React.RefObject<HTMLDivElement>, queryString: UrlData) {
        this.driveBar = driveBar;
        this.driveBar.current.addEventListener("drop", this.dropEvent)
        this.driveBar.current.addEventListener("dragover", this.dragoverEvent)
        this.driveBar.current.addEventListener("dragenter", this.dragenterEvent)
        this.queryString = queryString;
    }

    dragenterEvent = (ev: DragEvent) => this.dragenter(ev);
    dragoverEvent = (ev: DragEvent) => this.dragover(ev);
    dropEvent = (ev: DragEvent) => this.drop(ev);
    drop(e: DragEvent) {
        this.driveBar.current.classList.remove("dragenter");
        if (e.dataTransfer.items) {
            for (let i in e.dataTransfer.items) {
                let item = e.dataTransfer.items[i];
                if (item.kind === 'file') {
                    const file = item.getAsFile();
                    this.upload(file);
                }
            }
        }
        e.preventDefault();
    }

    dragover(e: DragEvent) {
        // console.log("over");
        e.preventDefault();
    }

    dragenter(e: DragEvent) {
        if (e.dataTransfer.types[0] === "Files") {
            this.driveBar.current.classList.add("dragenter");
            e.preventDefault();
        }
    }

    upload(file: File) {
        let url = `FileManager/Base/Upload`;
        let body: any = { folderId: null };
        if (this.queryString.id !== "root") {
            body.folderId = this.queryString.id;
        }
        DataTransmitter.Upload<JsonResponse<undefined>>(DataTransmitter.BaseUrl + url, file, body).then(x => {
                console.log(x);
        });
    }
}