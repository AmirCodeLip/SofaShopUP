import * as React from 'react'
import FileManagerOnLoadData from './../../webModels/FileManager/FileManagerOnLoadData'
import FolderInfo from './../../webModels/FileManager/FolderInfo'
import FObjectKind from './../../webModels/FileManager/FObjectKind'
import { FObjectKindComponent } from '../../Services/FileManagerServices'

export type EventClickType = (ev: MouseEvent) => void;

export function FolderLogo() {
    return (
        <>
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 181.73 167.58"
                strokeMiterlimit={10}>
                <path className="folder" d="M4.5,37.82H154.55A22.68,22.68,0,0,1,177.23,60.5v79.9a22.68,22.68,0,0,1-22.68,22.68H27.18A22.68,22.68,0,0,1,4.5,140.4V37.82A0,0,0,0,1,4.5,37.82Z" />
                <path className="folder" d="M4.5,4.5H55.57A32.32,32.32,0,0,1,87.9,36.82v0a0,0,0,0,1,0,0H4.5a0,0,0,0,1,0,0V4.5A0,0,0,0,1,4.5,4.5Z" /></svg>
        </>);
}

export class FileManagerProps {
    model: FileManagerOnLoadData;
    folderId: string;
}


export class FileManagerState {
    showContextMenu: boolean = false;
    fData: Array<FObjectKindComponent> = [];
    clickedSection: ClickedSection | undefined;
}

export interface RightBarItem {
    text: string,
    cmdText: string,
    icon: string,
    refItem: React.RefObject<HTMLDivElement>,
    clickedSection: number;
    clicked: () => void;
}

export enum ClickedSection {
    driveBar, folder, file
}