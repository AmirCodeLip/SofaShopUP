import * as React from 'react'
import { LoaderCommunicator } from './../../model_structure/BaseLoader'
import UserInfo from './../../webModels/UserInfo'
import { cookies } from './../shared/GlobalManage'

class Props {
    loaderCommunicator: LoaderCommunicator;
}

export function UserInfoFrame(props: Props) {
    const [visible, setVisible] = React.useState<boolean>(false);
    let userInfoFrame = React.useRef<HTMLInputElement>();
    let placeHolder = React.useRef<HTMLInputElement>();
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
    let logout = async (userInfo: UserInfo) => {
        let txt = window.cultureInfo.GetString("PublicWord001.key019");
        txt = txt.replace("{0}", userInfo.Name);
        let result = await props.loaderCommunicator.makeSure(txt);
        if (result) {
            window.pVInfo.UserInfoList = window.pVInfo.UserInfoList.filter(x => x.Token !== userInfo.Token);
            let current = window.pVInfo.UserInfoList.find(x => x.IsDefault);
            if (!current && window.pVInfo.UserInfoList.length > 0) {
                current = window.pVInfo.UserInfoList[0];
                current.IsDefault = true;
            }
            await cookies.pVInfoSetProcess(window.pVInfo);
            window.location.reload();
        }
    }
    let changeAccount = async (userInfo: UserInfo) => {
        setVisibleEvent(false);
        let current = window.pVInfo.UserInfoList.find(x => x.IsDefault);
        let txt = window.cultureInfo.GetString("PublicWord001.key018");
        txt = txt.replace("{0}", current.Name);
        txt = txt.replace("{1}", userInfo.Name);
        let result = await props.loaderCommunicator.makeSure(txt);
        if (result) {
            current.IsDefault = false;
            userInfo.IsDefault = true;
            await cookies.pVInfoSetProcess(window.pVInfo);
            window.location.reload();
        }
    }
    React.useEffect(() => {
        if (props.loaderCommunicator.searchDrive && props.loaderCommunicator.searchDrive !== null) {
            props.loaderCommunicator.searchDrive.current.addEventListener("keyup", function (e) {
                let target = e.target as HTMLInputElement;
                if (target.value.length === 0)
                    placeHolder.current.style.display = "block";
                else
                    placeHolder.current.style.display = "none";
            });
            placeHolder.current.onclick = function () {
                props.loaderCommunicator.searchDrive.current.focus();
            }
        }
        return () => {
            window.removeEventListener("click", checkParent);
        };
    }, []);
    let currentUser = window.pVInfo.UserInfoList.find(x => x.IsDefault);
    return (<>
        <div className='header-bar border-hide-bottom'>
            <div className='logo-box'>
                <img src="/logo192.png" />
                <div className='content-title'>
                    file manager
                </div>
            </div>
            {(props.loaderCommunicator.searchDrive ?
                <div className='search-box'>
                    <button>
                        <i className='fa fa-search'></i>
                    </button>
                    <input className="search-drive" ref={props.loaderCommunicator.searchDrive} />
                    <div className="s-place-holder" ref={placeHolder}>search here</div>
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
                    <div className="profile-box" key={currentUser.Token}>
                        <div className="profile-icon">
                            <img src="/personal.jpg"></img>
                        </div>
                        <div className="profile-name">
                            {currentUser.Name}
                        </div>
                        <div className="profile-user-name">
                            {currentUser.UserName}
                        </div>
                        <ul className="profile-setting">
                            <li><i className="fa-solid fa-gear"></i></li>
                            <li onClick={() => logout(currentUser)}><i className="fa-solid fa-power-off"></i></li>
                        </ul>
                    </div>
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
                                        <li onClick={() => changeAccount(x)}><i className="fa-solid fa-arrows-rotate"></i></li>
                                        <li onClick={() => logout(x)}><i className="fa-solid fa-power-off"></i></li>
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