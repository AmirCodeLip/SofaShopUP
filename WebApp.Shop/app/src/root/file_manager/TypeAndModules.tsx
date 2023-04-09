import * as React from 'react'
import FileManagerOnLoadData from '../../webModels/FileManager/FileManagerOnLoadData'
import FolderInfo from '../../webModels/FileManager/FolderInfo'
import FObjectKind from '../../webModels/FileManager/FObjectKind'
import { FObjectKindComponent } from '../../Services/FileManagerServices'
import { BaseLoader } from '../../model_structure/BaseLoader'
import { LoaderCommunicator } from '../../model_structure/BaseLoader'


export function FolderLogo() {
    return (
        <>
            <svg xmlns="http://www.w3.org/2000/svg" width="2000" height="2000" viewBox="0 0 1200 1000">
                <path className="folder" d="M549.95,556.19L490.989,601l0.285-90Z" />
                <path className="folder" d="M258.812,560H941.188a0,0,0,0,1,0,0V865.844a20,20,0,0,1-20,20H278.812a20,20,0,0,1-20-20V560A0,0,0,0,1,258.812,560Z" />
                <path className="folder" d="M259.733,511.374l233.056,0.35h0l0.1,49.3h0l-233.055-.35h0l-0.105-49.3h0Z" />
            </svg>

            {/* <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 181.73 167.58"
                strokeMiterlimit={10}>
                <path className="folder" d="M4.5,37.82H154.55A22.68,22.68,0,0,1,177.23,60.5v79.9a22.68,22.68,0,0,1-22.68,22.68H27.18A22.68,22.68,0,0,1,4.5,140.4V37.82A0,0,0,0,1,4.5,37.82Z" />
                <path className="folder" d="M4.5,4.5H55.57A32.32,32.32,0,0,1,87.9,36.82v0a0,0,0,0,1,0,0H4.5a0,0,0,0,1,0,0V4.5A0,0,0,0,1,4.5,4.5Z" /></svg> */}
        </>);
}

export type EventClickType = (ev: MouseEvent) => void;

export class FileManagerProps extends BaseLoader<FileManagerOnLoadData> {
    // model: ;
    folderId: string;
    
}

export class FileManagerState {
    showContextMenu: boolean = false;
    fData: Array<FObjectKindComponent> = [];
    clickedSection: ClickedSection | undefined;
}

export interface RightBarItem {
    text: JSX.Element,
    cmdText: string,
    icon: string,
    refItem: React.RefObject<HTMLDivElement>,
    clickedSection: number;
    clicked: () => void;
}

export enum ClickedSection {
    driveBar, folder, file
}

export interface EventType {
    action: (...params: Array<any>) => any | void;
    name: string;
}

export interface PLayout {
    children: JSX.Element;
    loaderCommunicator: LoaderCommunicator;
    invokeEvent: <T>(name: string, ...params: Array<any>) => T;
}

export interface LayoutState {
    navMode: "openFullSide" | "close",
    sideWidth: number,
    load: Boolean,
    phoneOrTablet: boolean
}

export interface SearchItemHolder {
    val: string,
    time: number
}

export interface SelectionInfo {
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