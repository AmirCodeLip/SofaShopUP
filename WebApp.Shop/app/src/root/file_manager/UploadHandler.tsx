import DataTransmitter from './../../Services/DataTransmitter'
import { JsonResponseStatus, JsonResponse } from '../../model_structure/JsonResponse';
import { UrlData } from './../shared/GlobalManage';
import {SelectionInfo} from './TypeAndModules'

export default class UploadHandler {
    driveBar: React.RefObject<HTMLDivElement>;
    queryString: UrlData;
    selectionElement: React.RefObject<HTMLDivElement>;
    selectionInfo: SelectionInfo;
    invokeEvent: (<T>(name: "SelectFObject" | "UploadDone") => T);
    // onUploadDone: uploadDoneType;
    // onselectFObject: () => void;

    constructor(driveBar: React.RefObject<HTMLDivElement>, queryString: UrlData,
        selectionElement: React.RefObject<HTMLDivElement>, invokeEvent: (<T>(name: string) => T)) {
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
        this.invokeEvent = invokeEvent;
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
                clientY: ev.clientY,
                scrollY: window.scrollY,
                constDifferenceX: sideGridCenter.clientWidth,
                constDifferenceY: headerBar.clientHeight
            };
        }
    }
    driveBarMove(ev: MouseEvent) {
        if (this.selectionDrive) {
            let isRtl = window.cultureInfo.cultureInfo.Rtl;
            ///drag right
            if (this.selectionInfo.clientX < ev.clientX) {
                let distance = (ev.clientX - this.selectionInfo.clientX);
                this.selectionInfo.left = this.selectionInfo.clientX - (isRtl ? 0 : this.selectionInfo.constDifferenceX) + 10;
                this.selectionInfo.width = distance;
            }
            ///drag left
            else if (this.selectionInfo.clientX > ev.clientX) {

                let distance = (this.selectionInfo.clientX - ev.clientX);
                this.selectionInfo.left = this.selectionInfo.clientX - distance - (isRtl ? 0 : this.selectionInfo.constDifferenceX) + 10;
                this.selectionInfo.width = distance;
            }
            ///drag bottom
            if ((this.selectionInfo.clientY) < (ev.clientY + (window.scrollY - this.selectionInfo.scrollY))) {
                let distance = (ev.clientY - this.selectionInfo.clientY);
                this.selectionInfo.top = this.selectionInfo.clientY;
                this.selectionInfo.height = distance;
                if (this.selectionInfo.scrollY !== 0) {
                    let diff = window.scrollY - this.selectionInfo.scrollY;
                    this.selectionInfo.top += this.selectionInfo.scrollY - this.selectionInfo.constDifferenceY;
                    this.selectionInfo.height += diff;
                }
                else {
                    this.selectionInfo.top -= this.selectionInfo.constDifferenceY;
                    this.selectionInfo.height += window.scrollY;
                }
            }
            ///drag top 
            else {
                let distance = (this.selectionInfo.clientY - ev.clientY);
                this.selectionInfo.top = this.selectionInfo.clientY - distance;
                this.selectionInfo.height = distance;
                if (this.selectionInfo.scrollY !== 0) {
                    let diff = this.selectionInfo.scrollY - window.scrollY;
                    if (diff > 0) {
                        this.selectionInfo.top += this.selectionInfo.scrollY - diff - this.selectionInfo.constDifferenceY;
                        this.selectionInfo.height += diff;

                    } else {
                        this.selectionInfo.top += this.selectionInfo.scrollY - this.selectionInfo.constDifferenceY;
                    }
                }
                else {
                    this.selectionInfo.top -= this.selectionInfo.constDifferenceY;
                    //  this.selectionInfo.height += this.selectionInfo.constDifferenceY;
                }
            }

            this.selectionElement.current!!.style.left = `${this.selectionInfo.left}px`;
            this.selectionElement.current!!.style.width = `${this.selectionInfo.width}px`;
            this.selectionElement.current!!.style.top = `${this.selectionInfo.top}px`;
            this.selectionElement.current!!.style.height = `${this.selectionInfo.height}px`;
            // this.onselectFObject();
            this.invokeEvent("SelectFObject");
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
            // await this.onUploadDone(x, folderId);
            await this.invokeEvent("UploadDone");
        });
    }

    public componentWillUnmount() {
        window.removeEventListener("mouseup", this.windowClick);
        window.removeEventListener("mousemove", this.driveBarMove);
        window.removeEventListener("mousedown", this.driveBarDown);
    }
}
