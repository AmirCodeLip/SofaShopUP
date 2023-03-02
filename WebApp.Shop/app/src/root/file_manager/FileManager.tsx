import * as React from 'react'
import {
    FolderLogo, FileManagerProps, FileManagerState, RightBarItem, EventClickType, ClickedSection
} from './FolderModules';
import { Col, Row } from 'react-bootstrap';
import { Web_Modal, ModalOptions, ModalType } from './../web_modal/Web_Modal';
import { FormModeInput, FormHandler, HiddenModeInput } from '../../mylibraries/asp-communication/components/FormModelItem';
import FolderInfo from './../../webModels/FileManager/FolderInfo';
import { load, editForm, FObjectKindComponent } from '../../Services/FileManagerServices'
import { JsonResponseStatus, JsonResponse } from './../../models/JsonResponse';
import { FObjectType } from './../../webModels/FileManager/FObjectType';
import DataTransmitter from '../../Services/DataTransmitter';
import UploadHandler from './UploadHandler';
import { UrlData } from './../shared/GlobalManage';
import FObjectKind from './../../webModels/FileManager/FObjectKind';
import Layout from './Layout';


export default class FileManager extends React.Component<FileManagerProps, FileManagerState>  {

    driveBar: React.RefObject<HTMLDivElement>;
    selectionElement: React.RefObject<HTMLDivElement>;
    nameFolderOrFile: FormModeInput;
    fObjectInfoFormHandler: FormHandler;
    contextMenuMiddleware: ModalOptions;
    fObjectMenuMiddleware: ModalOptions;
    newData?: FObjectKindComponent;
    queryString: UrlData;
    folderId: string | undefined;
    uploadHandler?: UploadHandler;


    rightBarItems: Array<RightBarItem> =
        [
            {
                text: 'رفرش',
                cmdText: 'Ctrl+R',
                icon: "fa-solid fa-arrows-rotate",
                refItem: React.createRef<HTMLDivElement>(),
                clickedSection: ClickedSection.driveBar,
                clicked: () => { }
            },
            {
                text: 'فولدر جدید',
                cmdText: 'Ctrl+N+D',
                icon: "fa-regular fa-folder",
                refItem: React.createRef<HTMLDivElement>(),
                clickedSection: ClickedSection.driveBar,
                clicked: () => {
                    this.newData = new FObjectKindComponent({
                        // Id: null,
                        FolderId: null,
                        Name: "",
                        FObjectType: FObjectType.Folder,
                        Path: "",
                        TypeKind: ""
                    });
                    this.openEditFolderOrFile()
                }
            },
            {
                text: 'تغییر نام فولدر',
                cmdText: 'Ctrl+R+N',
                icon: "fa-solid fa-pen",
                refItem: React.createRef<HTMLDivElement>(),
                clickedSection: ClickedSection.folder,
                clicked: () => {
                    this.openEditFolderOrFile()
                }
            },
            {
                text: 'تغییر نام فایل',
                cmdText: 'Ctrl+R+N',
                icon: "fa-solid fa-pen",
                refItem: React.createRef<HTMLDivElement>(),
                clickedSection: ClickedSection.file,
                clicked: () => {
                    this.openEditFolderOrFile()
                }
            }
        ];

    constructor(props: FileManagerProps) {
        super(props);
        this.queryString = new UrlData();
        this.driveBar = React.createRef<HTMLDivElement>();
        this.selectionElement = React.createRef<HTMLInputElement>();
        this.nameFolderOrFile = new FormModeInput(props.model.EditFolderOrFileForm, "Name");
        this.fObjectInfoFormHandler = new FormHandler(this.nameFolderOrFile,
            new HiddenModeInput<string>(props.model.EditFolderOrFileForm, "Id"),
            new HiddenModeInput<string>(props.model.EditFolderOrFileForm, "FObjectType"),
            new HiddenModeInput<string>(props.model.EditFolderOrFileForm, "Path"),
            new HiddenModeInput<string>(props.model.EditFolderOrFileForm, "TypeKind"),
            new HiddenModeInput<string>(props.model.EditFolderOrFileForm, "FolderId"));
        this.state =
        {
            showContextMenu: false,
            fData: [],
            clickedSection: undefined
        };
        this.contextMenuMiddleware = new ModalOptions(ModalType.contextModal);
        this.fObjectMenuMiddleware = new ModalOptions(ModalType.defualtModal);
    }

    render() {
        return (
            <Layout>
                <>
                    <Row className="drive-bar" ref={this.driveBar}>
                        {this.state.fData.filter(f => f.model.FObjectType === FObjectType.Folder).map((fData, i) => (
                            <Col xl={2} lg={3} md={4} sm={12} xs={12} className="f-hold" key={fData.id} ref={fData.refObject} >
                                <FolderLogo></FolderLogo>
                                <div className="f-hold-title right-item">
                                    {fData.model.Name}
                                </div>
                                <div className="hover-bar"></div>
                                <div className="select-bar"></div>
                            </Col>
                        ))}
                        {this.state.fData.filter(f => f.model.FObjectType === FObjectType.File).map((fData, i) => (
                            <Col xl={2} lg={3} md={4} sm={12} xs={12} className="f-hold" key={fData.id} ref={fData.refObject} >
                                <img src={DataTransmitter.BaseUrl + "FileManager/Base/GetFileImage/" + fData.model.Id} />
                                <div className="f-hold-title right-item">
                                    {fData.model.Name}
                                </div>
                                <div className="hover-bar"></div>
                                <div className="select-bar"></div>
                            </Col>
                        ))}
                        <div ref={this.selectionElement} style={{ display: 'none' }} className='selection-drive'></div>
                    </Row>
                    <Web_Modal middleware={this.fObjectMenuMiddleware}>
                        <>
                            <div className="epo-form">
                                <label className='epo-right right-item' ref={this.nameFolderOrFile.refLabel}></label>
                                <input className="epo right-item" ref={this.nameFolderOrFile.refInput} />
                                <div className="epo-border"></div>
                                <div className="right-item" ref={this.nameFolderOrFile.refError} ></div>
                            </div>
                            <div className="epo-form" onClick={() => this.editFObject()}>
                                <button className="btn btn-outline-001 btn-well">
                                    ثبت
                                </button>
                            </div>
                        </>
                    </Web_Modal>
                    <Web_Modal middleware={this.contextMenuMiddleware}>
                        <Row>
                            {this.rightBarItems.filter(x => x.clickedSection === this.state.clickedSection).map((item, key) =>
                                <Col lg={12} className='contextlist-each' ref={item.refItem} key={key} >
                                    <div className='contextlist-hold'>
                                        <div className="contextlist-item-text right-item">
                                            {item.text}
                                        </div>
                                        <div className='contextlist-item-icon'>
                                            <i className={item.icon}></i>
                                        </div>
                                    </div>
                                    <div className='command-line'>
                                        {item.cmdText}
                                    </div>
                                </Col>
                            )}
                        </Row>
                    </Web_Modal>
                </>
            </Layout>
        );

    }

    rightClick(ev: MouseEvent) {
        let htmlTarget = (ev.target as HTMLDivElement);
        this.fObjectMenuMiddleware.onLoaded = undefined;
        let preFixElement = (ev.target as HTMLElement);
        let fixedElement = this.getFixedElement(preFixElement);
        this.deselectAll();
        let clickedSection: ClickedSection | undefined;

        if (htmlTarget === (this.driveBar.current)) {
            clickedSection = ClickedSection.driveBar;
        }
        else if (fixedElement.classList[0] === "f-hold") {
            let fModel = this.state.fData.find(x => x.refObject!!.current === fixedElement);
            if (fModel && fModel.model.FObjectType === FObjectType.Folder) {
                clickedSection = ClickedSection.folder;
                if (fModel)
                    fModel.selected = true;
            } else if (fModel && fModel.model.FObjectType === FObjectType.File) {
                clickedSection = ClickedSection.file;
                if (fModel)
                    fModel.selected = true;
            }
        }
        else this.setState({ clickedSection: undefined });
        if (clickedSection !== undefined) {
            // let promise = () => {
            //     for (let item of this.rightBarItems) {
            //         item.refItem.current.style.display = item.clickedSection === this.clickedSection ? "block" : "none";
            //     }
            // }
            if (this.state.clickedSection !== clickedSection) {
                this.setState({ clickedSection: clickedSection })
            }

            if (this.contextMenuMiddleware.enable) {
                // promise();
            }
            else {
                // this.contextMenuMiddleware.onLoaded = () => promise();
                this.contextMenuMiddleware.enable = true;
            }
            this.contextMenuMiddleware.xPos = ev.pageX + "px";
            this.contextMenuMiddleware.yPos = ev.pageY + "px";
            ev.preventDefault();
        }
    }

    async leftDblClick(ev: MouseEvent) {
        let preFixElement = (ev.target as HTMLElement);
        let fixedElement = this.getFixedElement(preFixElement);
        if (fixedElement.classList[0] === "f-hold") {
            let fModel = this.state.fData.find(x => x.refObject!!.current === fixedElement);
            if (fModel!!.model.FObjectType === FObjectType.Folder) {
                await this.setFolder(fModel!!.model.Id!!);
                await this.loadData();
                //to do change root search
            }
        }
    }

    leftClick(ev: MouseEvent) {
        let preFixElement = (ev.target as HTMLElement);
        let fixedElement = this.getFixedElement(preFixElement);
        if (preFixElement.classList[0] === "hover-bar") {
            this.deselectAll();
            this.contextMenuMiddleware.enable = false;
        }
        if ((ev.target as HTMLDivElement) === (this.driveBar.current)) {
            this.deselectAll();
            this.contextMenuMiddleware.enable = false;
        }
        else if (fixedElement.classList[0] === "f-hold") {
            let fModel = this.state.fData.find(x => x.refObject!!.current === fixedElement);
            if (fModel)
                fModel.selected = true;
        }
        else if (fixedElement.classList[0] === "contextlist-each") {
            let clickedItem = this.rightBarItems.find(x => x.refItem.current === fixedElement);
            clickedItem?.clicked();
        }
    }

    /**
     when time some one click on back button if
     there is exist past location we will load data
     again
    */
    async popstate(ev: PopStateEvent) {
        this.parseQueryString();
        this.loadData();
        //to do change root search
        // this.searchDrive.current!!.value = await parseId(this.queryString.id);
    }

    /** for editing file or folder*/
    async editFObject() {
        let formData = this.fObjectInfoFormHandler.getFormData<FObjectKind>();
        if (formData.FolderId === null) delete formData.Id;
        var data = await editForm(formData);
        if (data?.Status === JsonResponseStatus.Success) {
            this.fObjectMenuMiddleware.enable = false;
            await this.loadData();
        }
        else {
            for (let index in data!!.InfoData) {
                this.fObjectInfoFormHandler.addError(index, data!!.InfoData[index]);
            }
        }
    }

    /** open edit modal*/
    openEditFolderOrFile(): void {
        this.contextMenuMiddleware.enable = false;
        this.fObjectMenuMiddleware.enable = true;
        let folderOrFile = this.newData ? this.newData : this.state.fData.find(x => x.selected);
        this.newData = undefined;
        if (folderOrFile && folderOrFile !== null) {
            this.fObjectMenuMiddleware.onLoaded = () => {
                this.fObjectInfoFormHandler.setFormData(folderOrFile!!.model);
            }
        }
        setTimeout(() => {
            if (!this.nameFolderOrFile.refLabel.current ||
                !this.nameFolderOrFile.refInput || !this.nameFolderOrFile.refError)
                return;
            this.fObjectInfoFormHandler.init();
        }, 60);
    }

    /** get or refresh data */
    async loadData() {
        let data = await load(React.createRef, this.folderId);
        this.setState({
            fData: data
        });
    }

    /** parse url of the page */
    parseQueryString() {
        this.queryString.update();
        this.folderId = this.queryString.id;
    }

    /**
     * when some one change folder we reload
     * the data of the page
     * @param folderId 
     */
    setFolder(folderId: string) {
        window.history.pushState(null, "changeFolder", `/${this.queryString.culture}/manage_files/${folderId}`);
        this.parseQueryString();
    }

    deselectAll() {
        for (let fKind of this.state.fData) {
            fKind.selected = false;
        }
    }



    getFixedElement(elemet: HTMLElement): HTMLElement {
        if (elemet.classList[0] === "hover-bar" || elemet.classList[0] === "select-bar") {
            return elemet!.parentElement as HTMLElement;
        }
        return elemet;
    }

    linkListItemClick(e: Event) {
        let target: HTMLElement | any = e.target!! as HTMLElement;
        target = target.tagName === 'P' ? target.parentElement!!.parentElement!! : target;
        target = target.tagName === 'SPAN' || target.tagName === 'I' ? target.parentElement!! : target;
        let paths = target.href.split("/");
        let id = paths[paths.length - 1];

        e.preventDefault();
    }

    componentWillUnmount() {
        document.removeEventListener("contextmenu", this.rightClick);
        document.removeEventListener("click", this.leftClick);
        document.removeEventListener("dblclick", this.leftDblClick);
        window.removeEventListener("popstate", this.popstate);
        let items = document.getElementsByClassName("link-list-item");
        for (let index = 0; index < items.length; index++) {
            items[index].removeEventListener("click", this.linkListItemClick);
        }
        this.uploadHandler!!.componentWillUnmount();
    }

    componentDidMount() {
        if (this.fObjectInfoFormHandler)
            this.fObjectInfoFormHandler.initRef(React.createRef);
        document.addEventListener("contextmenu", this.rightClick.bind(this));
        document.addEventListener("click", this.leftClick.bind(this));
        document.addEventListener("dblclick", this.leftDblClick.bind(this));
        let items = document.getElementsByClassName("link-list-item");
        for (let index = 0; index < items.length; index++)
            items[index].addEventListener("click", this.linkListItemClick.bind(this));
        window.addEventListener("popstate", this.popstate.bind(this));
        this.parseQueryString();
        setTimeout(async () => {
            this.uploadHandler = new UploadHandler(this.driveBar, this.queryString, this.selectionElement, this.onUploadDone.bind(this));
            await this.loadData();
        }, 1000);
    }

    async onUploadDone(response: JsonResponse<undefined>, preFolderId: string) {
        if (this.queryString.id === preFolderId) {
            await this.loadData();
        }
    }
}
