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


export default class FileManager extends React.Component<FileManagerProps, FileManagerState>  {

    driveBar: React.RefObject<HTMLDivElement>;
    fObjectInfoModelInput: FormModeInput;
    fObjectInfoFormHandler: FormHandler;
    contextMenuMiddleware: ModalOptions;
    fObjectMenuMiddleware: ModalOptions;
    newData?: FObjectKindComponent;
    // clickedSection: ClickedSection | undefined;
    queryString: UrlData;
    folderId: string | undefined;
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
                        Id: null,
                        FolderId: null,
                        Name: "",
                        FObjectType: FObjectType.Folder
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
        this.fObjectInfoModelInput = new FormModeInput(props.model.EditFolderOrFileForm, "Name");
        this.fObjectInfoFormHandler = new FormHandler(this.fObjectInfoModelInput,
            new HiddenModeInput<string>(props.model.EditFolderOrFileForm, "Id"),
            new HiddenModeInput<string>(props.model.EditFolderOrFileForm, "FObjectType"),
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
            <>
                <Row className="drive-bar" ref={this.driveBar}>
                    {this.state.fData.filter(f => f.model.FObjectType === FObjectType.Folder).map((fData, i) => (
                        <Col md={4} lg={2} className="f-hold" key={fData.id} ref={fData.refObject} >
                            <FolderLogo></FolderLogo>
                            <div className="f-hold-title right-item">
                                {fData.model.Name}
                            </div>
                            <div className="hover-bar"></div>
                            <div className="select-bar"></div>
                        </Col>
                    ))}
                    {this.state.fData.filter(f => f.model.FObjectType === FObjectType.File).map((fData, i) => (
                        <Col md={4} lg={2} className="f-hold" key={fData.id} ref={fData.refObject} >
                            <img src={DataTransmitter.BaseUrl + "FileManager/Base/GetFileImage/" + fData.model.Id} />
                            <div className="f-hold-title right-item">
                                {fData.model.Name}
                            </div>
                            <div className="hover-bar"></div>
                            <div className="select-bar"></div>
                        </Col>
                    ))}
                </Row>
                <Web_Modal middleware={this.fObjectMenuMiddleware}>
                    <>
                        <div className="epo-form">
                            <label className='epo-right right-item' ref={this.fObjectInfoModelInput.refLabel}></label>
                            <input className="epo right-item" ref={this.fObjectInfoModelInput.refInput} />
                            <div className="epo-border"></div>
                            <div className="right-item" ref={this.fObjectInfoModelInput.refError} ></div>
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
        );

    }
    eventRightClick: EventClickType = (ev) => this.rightClick(ev);
    eventLeftClick: EventClickType = (ev) => this.leftClick(ev);
    eventLeftDblClick: EventClickType = (ev) => this.leftDblClick(ev);
    eventPopstate: (ev: PopStateEvent) => any = (ev) => this.popstate(ev);

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
            let fModel = this.state.fData.find(x => x.refObject.current === fixedElement);
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

    leftDblClick(ev: MouseEvent) {
        let preFixElement = (ev.target as HTMLElement);
        let fixedElement = this.getFixedElement(preFixElement);
        if (fixedElement.classList[0] === "f-hold") {
            let fModel = this.state.fData.find(x => x.refObject.current === fixedElement);
            if (fModel.model.FObjectType === FObjectType.Folder) {
                this.setFolder(fModel.model.Id);
                this.loadData();
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
            let fModel = this.state.fData.find(x => x.refObject.current === fixedElement);
            if (fModel)
                fModel.selected = true;
        }
        else if (fixedElement.classList[0] === "contextlist-each") {
            let clickedItem = this.rightBarItems.find(x => x.refItem.current === fixedElement);
            clickedItem?.clicked();
        }
    }

    popstate(ev: PopStateEvent) {
        this.parseQueryString();
        this.loadData();
    }

    async editFObject() {
        let formData = this.fObjectInfoFormHandler.getFormData<FObjectKind>();
        if (formData.FolderId === null) delete formData.Id;
        var data = await editForm(formData);
        if (data?.Status === JsonResponseStatus.Success) {
            this.fObjectMenuMiddleware.enable = false;
            await this.loadData();
        }
        else {
            for (let index in data.InfoData) {
                this.fObjectInfoFormHandler.addError(index, data.InfoData[index]);
            }
        }
    }

    openEditFolderOrFile(): void {
        this.contextMenuMiddleware.enable = false;
        this.fObjectMenuMiddleware.enable = true;
        let folderOrFile = this.newData ? this.newData : this.state.fData.find(x => x.selected);
        this.newData = undefined;
        if (folderOrFile && folderOrFile !== null) {
            this.fObjectMenuMiddleware.onLoaded = () => {
                this.fObjectInfoFormHandler.setFormData(folderOrFile.model);
            }
        }
        setTimeout(() => {
            if (!this.fObjectInfoModelInput.refLabel.current ||
                !this.fObjectInfoModelInput.refInput || !this.fObjectInfoModelInput.refError)
                return;
            this.fObjectInfoFormHandler.init();
        }, 60);
    }

    async loadData() {
        let data = await load(React.createRef, this.folderId);
        this.setState({
            fData: data
        });
    }

    parseQueryString() {
        this.queryString.update();
        this.folderId = this.queryString.id === "root" ? undefined : this.queryString.id;
    }

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

    componentWillUnmount() {
        document.removeEventListener("contextmenu", this.eventRightClick);
        document.removeEventListener("click", this.eventLeftClick);
        document.removeEventListener("dblclick", this.eventLeftDblClick);
        window.removeEventListener("popstate", this.eventPopstate);
    }
    componentWillMount(): void {
        this.fObjectInfoFormHandler.initRef(React.createRef);
    }

    async componentDidMount() {
        document.addEventListener("contextmenu", this.eventRightClick);
        document.addEventListener("click", this.eventLeftClick);
        document.addEventListener("dblclick", this.eventLeftDblClick);
        window.addEventListener("popstate", this.eventPopstate);

        this.parseQueryString();
        await this.loadData();
        new UploadHandler(this.driveBar, this.queryString);

    }
}
