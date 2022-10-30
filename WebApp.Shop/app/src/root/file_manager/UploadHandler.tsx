import DataTransmitter from './../../Services/DataTransmitter'
import { JsonResponseStatus, JsonResponse } from './../../models/JsonResponse';

export default class UploadHandler {
    driveBar: React.RefObject<HTMLDivElement>;
    constructor(driveBar: React.RefObject<HTMLDivElement>) {
        this.driveBar = driveBar;
        this.driveBar.current.addEventListener("drop", this.dropEvent)
        this.driveBar.current.addEventListener("dragover", this.dragoverEvent)
        this.driveBar.current.addEventListener("dragenter", this.dragenterEvent)

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
        DataTransmitter.Upload<JsonResponse<undefined>>(DataTransmitter.BaseUrl + "FileManager/Base/Upload", 
        file, { id: 123 }).then(x => {
            console.log(x);

        });
    }
}