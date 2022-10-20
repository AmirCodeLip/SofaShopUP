import * as React from 'react'


interface ModalProps {
    children: JSX.Element
}
interface ModalState {
    enable: boolean
}

export default class FileManager extends React.Component<ModalProps, ModalState> {
    constructor(props: ModalProps) {
        super(props);
    }
    render() {
        return (<div className='web-modal'>
            <div className='web-modal-content'>
                <div className='web-modal-head'>
                    <span className='web-modal-close'>&times;</span>
                </div>
                <div className='web-modal-body'>
                    {this.props.children}
                </div>
            </div>
        </div>)
    }
}
