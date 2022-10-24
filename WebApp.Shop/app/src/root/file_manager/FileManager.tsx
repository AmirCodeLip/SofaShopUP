import * as React from 'react'
import { FolderLogo, FileManagerProps, FileManagerState, ClickedSection } from './FolderModules';
import { Col, Row } from 'react-bootstrap';
import './file-manager.css';
import { Web_Modal, ModalOptions, ModalType } from './../web_modal/Web_Modal';
import { FormModeInput, FormHandler, HiddenModeInput } from '../../mylibraries/asp-communication/components/FormModelItem';
import FolderInfo from './../../../webModels/FileManager/FolderInfo'
import { load, editForm } from '../../Services/FileManagerServices'
import { JsonResponseStatus, JsonResponse } from './../../models/JsonResponse';



type EventClickType = (ev: MouseEvent) => void;

export default class FileManager extends React.Component<FileManagerProps, FileManagerState>  {
    driveBar: React.MutableRefObject<HTMLDivElement | undefined>;
    folderInfoModelInput: FormModeInput;
    folderInfoFormHandler: FormHandler;
    contextMenuMiddleware: ModalOptions;
    folderMenuMiddleware: ModalOptions;

    constructor(props: FileManagerProps) {
        super(props);
        this.driveBar = React.createRef<HTMLDivElement>();
        this.folderInfoModelInput = new FormModeInput(props.model.EditFolderForm, "FolderName");
        let id = new HiddenModeInput<string>(props.model.EditFolderForm, "Id");
        this.folderInfoFormHandler = new FormHandler(this.folderInfoModelInput, id);
        this.state =
        {
            showContextMenu: false,
            fData: []
        };
        this.contextMenuMiddleware = new ModalOptions(ModalType.contextModal);
        this.folderMenuMiddleware = new ModalOptions(ModalType.defualtModal);
    }

    render() {
        let folders = [1, 2];
        return (
            <>
                <Row className="drive-bar" ref={this.driveBar}>
                    {this.state.fData.map((fData, i) => (
                        <Col md={4} lg={2} className="f-hold" key={i} >
                            <FolderLogo></FolderLogo>
                            <div className="f-hold-title right-item">
                                {fData.Name}
                            </div>
                            <div className="select-bar"></div>
                            <div className="hover-bar"></div>
                        </Col>
                    ))}
                </Row>
                <Web_Modal middleware={this.folderMenuMiddleware}>
                    <>
                        <div className="epo-form">
                            <label className='epo-right right-item' ref={this.folderInfoModelInput.refLabel}></label>
                            <input className="epo right-item" ref={this.folderInfoModelInput.refInput} />
                            <div className="epo-border"></div>
                            <div className="right-item" ref={this.folderInfoModelInput.refError} ></div>
                        </div>
                        <div className="epo-form" onClick={() => this.editFolder()}>
                            <button className="btn btn-outline-001 btn-well">
                                ثبت
                            </button>
                        </div>
                    </>
                </Web_Modal>
                <Web_Modal middleware={this.contextMenuMiddleware}>
                    <Row>
                        <Col lg={12} className='contextlist-each'>
                            <div className='contextlist-hold'>
                                <div className="contextlist-item-text right-item">
                                    رفرش
                                </div>
                                <div className='contextlist-item-icon'>
                                    icon
                                </div>
                            </div>
                            <div className='command-line'>
                                Ctrl+R
                            </div>
                        </Col>
                        <Col lg={12} className='contextlist-each' command="newFolder">
                            <div className='contextlist-hold'>
                                <div className="contextlist-item-text right-item">
                                    فولدرجدید
                                </div>
                                <div className='contextlist-item-icon'>
                                    icon
                                </div>
                            </div>
                            <div className='command-line'>
                                Ctrl+R
                            </div>
                        </Col>
                        <Col lg={12} className='contextlist-each'>
                            <div className='contextlist-hold'>
                                <div className="contextlist-item-text right-item">
                                    فایل جدید
                                </div>
                                <div className='contextlist-item-icon'>
                                    icon
                                </div>
                            </div>
                            <div className='command-line'>
                                Ctrl+R
                            </div>
                        </Col>
                    </Row>
                </Web_Modal>
            </>
        );
    }

    eventRightClick: EventClickType = (ev) => this.rightClick(ev);
    eventLeftClick: EventClickType = (ev) => this.leftClick(ev);

    rightClick(ev: MouseEvent) {
        let clickedSection: ClickedSection | undefined;
        if ((ev.target as HTMLDivElement) === (this.driveBar.current)) {
            clickedSection = ClickedSection.driveBar;
        }
        if (clickedSection !== undefined) {
            this.contextMenuMiddleware.enable = true;
            this.contextMenuMiddleware.xPos = ev.pageX + "px";
            this.contextMenuMiddleware.yPos = ev.pageY + "px";
            ev.preventDefault();
        }
    }

    leftClick(ev: MouseEvent) {
        let fixedElement = this.getFixedElement(ev.target as HTMLElement);
        if ((ev.target as HTMLDivElement) === (this.driveBar.current)) {
            this.contextMenuMiddleware.enable = false;
        }
        else if (fixedElement.classList[0] === "contextlist-each") {
            let command = fixedElement.getAttribute("command");
            switch (command) {
                case "newFolder":
                    this.newFolder();
                    break;
            }
        }
    }

    async editFolder() {
        var data = await editForm(this.folderInfoFormHandler.getFormData<FolderInfo>());
        if (data.status === JsonResponseStatus.Success) {
            this.folderMenuMiddleware.enable = false;
            await this.loadData();
        }
    }
    
    newFolder(): void {
        this.contextMenuMiddleware.enable = false;
        this.folderMenuMiddleware.enable = true;
        setTimeout(() => {
            if (!this.folderInfoModelInput.refLabel.current ||
                !this.folderInfoModelInput.refInput || !this.folderInfoModelInput.refError)
                return;
            this.folderInfoFormHandler.init();
        }, 60);
    }

    componentWillMount(): void {
        this.folderInfoFormHandler.initRef(React.createRef);
    }

    async componentDidMount() {
        document.addEventListener("contextmenu", this.eventRightClick);
        document.addEventListener("click", this.eventLeftClick);
        await this.loadData();

    }

    async loadData() {
        let data = await load();
        this.setState({
            fData: data
        });
    }

    componentWillUnmount() {
        document.removeEventListener("contextmenu", this.eventRightClick);
        document.removeEventListener("contextmenu", this.eventLeftClick);
    }

    deselectAll() {

    }

    getFixedElement(elemet: HTMLElement): HTMLElement {
        return elemet;
    }
    // driveBarClick(this: GlobalEventHandlers, e: MouseEvent) {
    //     if ((e.target as HTMLDivElement) === (this as HTMLDivElement)) {

    //     }
    // }
}

// https://stackoverflow.com/questions/52448143/how-to-deal-with-a-ref-within-a-loop
