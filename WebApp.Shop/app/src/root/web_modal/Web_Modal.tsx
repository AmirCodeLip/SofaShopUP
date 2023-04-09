import * as React from 'react'

type emptyEvent<TType> = (value: TType, preValue: TType) => void;

export enum ModalType {
    defualtModal,
    contextModal
}

export class ModalOptions {
    private _enable: boolean = false;

    constructor(modalType: ModalType) {
        if (modalType == ModalType.contextModal)
            this.headIsActive = false;
        this.modalType = modalType;
    }

    public readonly modalType: ModalType;
    public get enable(): boolean {
        return this._enable;
    }
    public set enable(v: boolean) {
        this.onEnableChange!(v, this._enable);
        this._enable = v;
    }

    private _xPos: string;
    public get xPos(): string {
        return this._xPos;
    }
    public set xPos(v: string) {
        this.onXPosChange!(v, this.xPos);
        this._xPos = v;
    }

    private _yPos: string;
    public get yPos(): string {
        return this._yPos;
    }
    public set yPos(v: string) {
        this.onYPosChange!(v, this._yPos);
        this._yPos = v;
    }
    headIsActive = true;
    outClickClose = true;
    onLoaded?: () => void;
    onEnableChange?: emptyEvent<boolean>;
    onXPosChange?: emptyEvent<string>;
    onYPosChange?: emptyEvent<string>;
}

class ModalProps {
    children: JSX.Element;
    middleware: ModalOptions;
}

interface ModalState {
    enable: boolean
}

export class Web_Modal extends React.Component<ModalProps, ModalState> {
    className = "";
    modalRef: React.RefObject<HTMLDivElement>;
    constructor(props: ModalProps) {
        super(props);
        this.state = {
            enable: false
        }
        switch (props.middleware.modalType) {
            case ModalType.contextModal:
                this.className = "web-modal-context";
                break;
            case ModalType.defualtModal:
                this.className = "web-modal";
                break;
        };
        this.modalRef = React.createRef<HTMLDivElement>();
        props.middleware.onXPosChange = (value, preValue) => {
            if (preValue === value) {
                return;
            }
            if (!this.modalRef || this.modalRef.current === null) {
                setTimeout(() => {
                    props.middleware.onXPosChange!(value, preValue);
                }, 10)
            }
            else
                this.modalRef.current.style.left = value;
        }
        props.middleware.onYPosChange = (value, preValue) => {
            if (preValue === value) {
                return;
            }
            if (!this.modalRef || this.modalRef.current === null) {
                setTimeout(() => {
                    props.middleware.onYPosChange!(value, preValue);
                }, 10);
            }
            else
                this.modalRef.current.style.top = value;
        }
        (props.middleware.onEnableChange = (value, preValue) => {
            if (preValue === value) {
                return;
            }
            this.modalRef = React.createRef<HTMLDivElement>();
            this.setState({ enable: value });
        }).bind(this);
    }

    componentDidUpdate(prevProps: ModalProps) {
        if (this.state.enable) {
            if (this.props.middleware.onLoaded)
                this.props.middleware.onLoaded();
        }

    }
    componentDidMount() {

    }

    componentWillUnmount() {
    }

    onModalClicked(ev: React.MouseEvent<HTMLDivElement, MouseEvent>) {
        let target = (ev.target as HTMLElement);
        if (this.props.middleware.outClickClose && target === this.modalRef.current) {
            this.props.middleware.enable = false
        }
    }
    render() {
        if (this.state.enable) {
            return (<div className={this.className} ref={this.modalRef} onClick={this.onModalClicked.bind(this)}>
                <div className='web-modal-content'>
                    {this.props.middleware.headIsActive &&
                        <div className='web-modal-head'>
                            <span className='web-modal-close' onClick={() => this.props.middleware.enable = false}>
                                <i className="fa-solid fa-xmark"></i>
                            </span>
                        </div>
                    }
                    <div className='web-modal-body'>
                        {this.props.children}
                    </div>
                </div>
            </div >)
        }
        else
            return (<></>);
    }

}
