import DataTransmitter from './../../Services/DataTransmitter'
import { JsonResponseStatus, JsonResponse } from './../../models/JsonResponse';
import { UrlData } from './../shared/GlobalManage';

type uploadDoneType = (data: JsonResponse<undefined>, preFolderId: string) => void;
export default class UploadHandler {
    driveBar: React.RefObject<HTMLDivElement>;
    queryString: UrlData;
    selectionElement: React.RefObject<HTMLDivElement>;
    selectionInfo: SelectionInfo;
    onUploadDone: uploadDoneType;
    onselectFObject: () => void;

    constructor(driveBar: React.RefObject<HTMLDivElement>, queryString: UrlData, selectionElement: React.RefObject<HTMLDivElement>) {
        this.driveBar = driveBar;
        this.selectionElement = selectionElement;
        this.driveBar.current!!.addEventListener("drop", this.dropEvent);
        this.driveBar.current!!.addEventListener("dragover", this.dragoverEvent);
        this.driveBar.current!!.addEventListener("dragenter", this.dragenterEvent);
        window.addEventListener("mousedown", this.driveBarDown.bind(this));
        window.addEventListener("mousemove", this.driveBarMove.bind(this));
        window.addEventListener("mouseup", this.windowClick.bind(this));
        this.queryString = queryString;
        this.selectionDrive = false;
    }

    private _selectionDrive: boolean;
    public get selectionDrive(): boolean {
        return this._selectionDrive;
    }
    public set selectionDrive(v: boolean) {
        this._selectionDrive = v;
        if (!this.selectionElement.current)
            return;

        if (this.selectionElement) {
            if (v) {

                this.selectionElement.current!!.style.display = "block";
            }
            else {
                this.selectionElement.current!!.style.display = "none";
                this.selectionElement.current!!.style.width = "0px";
                // this.selectionElement.current.style.height = "0px";
            }
        }

    }

    dragenterEvent = (ev: DragEvent) => this.dragenter(ev);
    dragoverEvent = (ev: DragEvent) => this.dragover(ev);
    dropEvent = (ev: DragEvent) => this.drop(ev);
    driveBarDown(ev: MouseEvent) {
        if (ev.target === this.driveBar.current || (ev.target as HTMLElement).className[0] === "view-main-center") {
            this.selectionDrive = true;
            let sideGridCenter = document.getElementsByClassName("side-grid-center")[0];
            let headerBar = document.getElementsByClassName("header-bar")[0];

            this.selectionInfo = {
                clientX: ev.clientX,
                clientY: ev.clientY + window.scrollY,
                scrollY: window.scrollY,
                constDifferenceX: sideGridCenter.clientWidth,
                constDifferenceY: headerBar.clientHeight
            };
        }
    }
    driveBarMove(ev: MouseEvent) {
        if (this.selectionDrive) {
            ///drag right
            if (this.selectionInfo.clientX < ev.clientX) {
                let distance = (ev.clientX - this.selectionInfo.clientX);
                this.selectionInfo.left = this.selectionInfo.clientX - this.selectionInfo.constDifferenceX + 10;
                this.selectionInfo.width = distance;
            }
            ///drag left
            else if (this.selectionInfo.clientX > ev.clientX) {

                let distance = (this.selectionInfo.clientX - ev.clientX);
                this.selectionInfo.left = this.selectionInfo.clientX - distance - this.selectionInfo.constDifferenceX + 10;
                this.selectionInfo.width = distance;
            }
            ///drag bottom
            if (this.selectionInfo.clientY < ev.clientY) {

                // this.selectionInfo.top = this.selectionInfo.clientY - this.selectionInfo.constDifferenceY;

                let distance = ev.clientY - window.scrollY - this.selectionInfo.clientY;
                this.selectionInfo.top = this.selectionInfo.clientY - this.selectionInfo.constDifferenceY;

                this.selectionInfo.height = distance;
            }
            ///drag top 
            else {
                let distance = (this.selectionInfo.clientY - window.scrollY - ev.clientY);
                this.selectionInfo.top = this.selectionInfo.clientY - distance - this.selectionInfo.constDifferenceY;
                this.selectionInfo.height = distance;
            }

            this.selectionElement.current!!.style.left = `${this.selectionInfo.left}px`;
            this.selectionElement.current!!.style.width = `${this.selectionInfo.width}px`;
            this.selectionElement.current!!.style.top = `${this.selectionInfo.top}px`;
            this.selectionElement.current!!.style.height = `${this.selectionInfo.height}px`;
            this.onselectFObject();
        }
    }
    windowClick(ev: MouseEvent) {
        this.selectionDrive = false;
    }

    drop(e: DragEvent) {
        this.driveBar.current!!.classList.remove("dragenter");
        if (e.dataTransfer!!.items) {
            for (let i in e.dataTransfer!!.items) {
                let item = e.dataTransfer!!.items[i];
                if (item.kind === 'file') {
                    let file = item.getAsFile();
                    if (file !== null)
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
        if (e.dataTransfer!!.types[0] === "Files") {
            this.driveBar.current!!.classList.add("dragenter");
            e.preventDefault();
        }
    }

    upload(file: File) {
        let url = `FileManager/Base/Upload`;
        let body: any = { folderId: null };
        let folderId = this.queryString.id;
        if (folderId !== "root") {
            body.folderId = this.queryString.id;
        }
        DataTransmitter.Upload<JsonResponse<undefined>>(DataTransmitter.BaseUrl + url, file, body).then(async x => {
            await this.onUploadDone(x, folderId);
        });
    }

    public componentWillUnmount() {
        window.removeEventListener("mouseup", this.windowClick);
        window.removeEventListener("mousemove", this.driveBarMove);
        window.removeEventListener("mousedown", this.driveBarDown);
    }
}
interface SelectionInfo {
    clientX: number,
    clientY: number,
    constDifferenceX: number,
    constDifferenceY: number,
    left?: number,
    width?: number,
    top?: number,
    height?: number,
    scrollY: number
}