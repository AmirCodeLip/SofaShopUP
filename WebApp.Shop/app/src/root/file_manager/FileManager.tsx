import * as React from 'react'
import { FolderLogo, FileManagerProps, FileManagerState } from './FolderModules';
import { Col, Row } from 'react-bootstrap';
import './file-manager.css';
import Web_Modal from './../web_modal/Web_Modal';
import { FormModeInput, FormHandler } from '../../mylibraries/asp-communication/components/FormModelItem';




export default class FileManager extends React.Component<FileManagerProps, FileManagerState>  {
    driveBar: React.MutableRefObject<HTMLDivElement | undefined>;

    constructor(props: FileManagerProps) {
        super(props);
        this.driveBar = React.createRef<HTMLDivElement>();
        let folderModel = new FormModeInput(model, "PhoneOrEmail");
    }

    render() {
        let folders = [1, 2];
        return (
            <>
                <Row className="drive-bar" ref={this.driveBar}>
                    {folders.map(o => (
                        <Col md={4} lg={2} className="f-hold">
                            <FolderLogo></FolderLogo>
                            <div className="f-hold-title right-item">
                                فولدر یک
                            </div>
                            <div className="select-bar"></div>
                            <div className="hover-bar"></div>
                        </Col>
                    ))}
                </Row>
                <Web_Modal>
                    <div>
                        re
                    </div>
                </Web_Modal>
            </>
        );
    }

    componentDidMount(): void {
        this.driveBar.current.onclick = this.driveBarClick;
    }

    deselectAll() {

    }

    driveBarClick(this: GlobalEventHandlers, e: MouseEvent) {
        if ((e.target as HTMLDivElement) === (this as HTMLDivElement)) {

        }
    }
}
// https://stackoverflow.com/questions/52448143/how-to-deal-with-a-ref-within-a-loop
