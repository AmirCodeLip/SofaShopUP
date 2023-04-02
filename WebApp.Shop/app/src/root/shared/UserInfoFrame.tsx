import * as React from 'react'
class Props {
    isFileManager: boolean;
    searchDrive?: React.RefObject<HTMLInputElement>;
}

export function UserInfoFrame(props: Props) {
    const [visible, setVisible] = React.useState<boolean>(false);
    let userInfoFrame = React.useRef<HTMLInputElement>();
    function checkParent(ev: MouseEvent) {
        if (userInfoFrame.current && !userInfoFrame.current.contains(ev.target as Node)) {
            setVisibleEvent(false);
        }
        return false;
    }
    let setVisibleEvent = (v: boolean) => {
        setVisible(v);
        setTimeout(() => {
            if (v) {
                window.addEventListener("click", checkParent);
            } else {
                window.removeEventListener("click", checkParent);
            }
        }, 100);
    };
    React.useEffect(() => {
        return () => {
            window.removeEventListener("click", checkParent)
        };
    }, []);
    return (<>
        <div className='header-bar border-hide-bottom'>
            <div className='logo-box'>
                <img src="/logo192.png" />
                <div className='content-title'>
                    file manager
                </div>
            </div>
            {(props.isFileManager ?
                <div className='search-box'>
                    <button>
                        <i className='fa fa-search'></i>
                    </button>
                    <input className="search-drive" ref={props.searchDrive} />
                    <div className="s-place-holder">search here</div>
                </div> :
                <div className='search-box-empty'></div>
            )}
            <div className='adjustment-toolbar'>
                <div className='user-settings'>
                    <img className='user-icon' onClick={() => setVisibleEvent(true)} src='/personal.jpg' />
                </div>
            </div>
        </div>
        {
            visible &&
            (<>
                <div className='user-info-frame' ref={userInfoFrame}>
                    <div className='user-info-header'>
                        <img className="user-info-img" src="/personal.jpg"></img>
                        <div className="user-info-profile">
                            amir code lip
                        </div>
                        <div className='user-setting'>
                            <i className="fa-solid fa-gear"></i>
                        </div>
                    </div>
                    {/* <div className="setting-devider"></div> */}
                    {
                        window.pVInfo.UserInfoList.filter(x => !x.IsDefault).map(x => {
                            return (
                                <div className="profile-box" key={x.Token}>
                                    <div className="setting-devider"></div>
                                    <div className="profile-icon">
                                        <img src="/personal.jpg"></img>
                                    </div>
                                    <div className="profile-name">
                                        {x.Name}
                                    </div>
                                    <div className="profile-user-name">
                                        {x.UserName}
                                    </div>
                                    <ul className="profile-setting">
                                        <li><i className="fa-solid fa-arrows-rotate"></i></li>
                                        <li><i className="fa-solid fa-power-off"></i></li>
                                    </ul>
                                </div>
                            )
                        })
                    }
                </div>
            </>)
        }
    </>);
}