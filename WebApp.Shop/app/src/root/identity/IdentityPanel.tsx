import * as React from 'react'
import IFormModel from '../../mylibraries/asp-communication/interfaces/IFormModel'
import { FormModeInput, FormHandler } from '../../mylibraries/asp-communication/components/FormModelItem'
import { loginService, registerService } from '../../Services/IdentityServices'
import { JsonResponseStatus } from '../../models/JsonResponse'
import { Container } from 'react-bootstrap'
import TabPanel from 'devextreme-react/tab-panel'
import { identityLoaderModel } from '../../Services/IdentityServices'
import { cookies } from '../shared/GlobalManage'
import LoginModel from '../../webModels/LoginModel'

interface LoginProps {
    data: tabInfo
}
interface IdentityPanelProps {
    model: identityLoaderModel
}
interface tabInfo {
    name: string
    getModel: () => IFormModel
}

export class IdentityPanel extends React.Component<IdentityPanelProps, any> {
    dataSource: Array<tabInfo> = [];
    constructor(props: IdentityPanelProps) {
        super(props);
        this.dataSource.push({ name: "login", getModel: () => props.model.LoginModel });
        this.dataSource.push({ name: "register", getModel: () => props.model.RegisterModel });
    }

    render() {
        return (
            <div className='identity-box'>
                <TabPanel
                    height={260}
                    dataSource={this.dataSource}
                    loop={false}
                    itemTitleRender={this.itemTitleRender}
                    itemComponent={IdentityForm.bind(this.props)}
                    animationEnabled={false}
                    swipeEnabled={false}
                    elementAttr={{ class: "identity-tabs" }}
                />

            </div>
        );
    }
    itemTitleRender(info: tabInfo) {
        return <span>{info.name === "login" ? "ورود" : "عضویت"}</span>;
    }
}


class IdentityForm extends React.PureComponent<LoginProps, any> {
    phoneOrEmailModel: FormModeInput;
    passwordModel: FormModeInput;
    formHandler: FormHandler;
    isLogin = false;
    constructor(props: LoginProps) {
        super(props);
        this.isLogin = this.props.data.name === "login";
        if (this.isLogin) {
            let loginModel = props.data.getModel();
            this.phoneOrEmailModel = new FormModeInput(loginModel, "PhoneOrEmail");
            this.passwordModel = new FormModeInput(loginModel, "Password");
            this.formHandler = new FormHandler(this.phoneOrEmailModel, this.passwordModel);
            this.formHandler.addHiddens(loginModel, "Token");
        } else {
            let registerModel = props.data.getModel();
            this.phoneOrEmailModel = new FormModeInput(registerModel, "PhoneOrEmail");
            this.passwordModel = new FormModeInput(registerModel, "Password");
            this.formHandler = new FormHandler(this.phoneOrEmailModel, this.passwordModel);
            this.formHandler.addHiddens(registerModel, "Token");
        }
        this.formHandler.initRef(React.createRef);

    }
    componentDidMount(): void {
        if (this.isLogin) {
            setTimeout(() => {
                this.formHandler.init();
            }, 50)
        }
        else {
            setTimeout(() => {
                this.formHandler.init();
            }, 50)
        }
    }
    render() {
        if (this.isLogin) {
            return (
                <Container>
                    <div className="epo-form">
                        <label className='epo-right right-item' ref={this.phoneOrEmailModel.refLabel}></label>
                        <input className="epo left-item" ref={this.phoneOrEmailModel.refInput} />
                        <div className="epo-border"></div>
                        <div className="right-item" ref={this.phoneOrEmailModel.refError} ></div>
                    </div>
                    <div className="epo-form">
                        <label className="epo-right right-item" ref={this.passwordModel.refLabel}></label>
                        <input className="epo" ref={this.passwordModel.refInput} />
                        <div className="epo-border"></div>
                        <div className="right-item" ref={this.passwordModel.refError} ></div>
                    </div>
                    <div className="epo-form" onClick={this.login.bind(this)}>
                        <button className="btn btn-outline-001 btn-well">
                            ورود
                        </button>
                    </div>
                </Container>
            );
        }
        else {
            return (
                <Container>
                    <div className="epo-form">
                        <label className='epo-right right-item' ref={this.phoneOrEmailModel.refLabel}></label>
                        <input className="epo left-item" ref={this.phoneOrEmailModel.refInput} />
                        <div className="epo-border"></div>
                        <div className="right-item" ref={this.phoneOrEmailModel.refError} ></div>
                    </div>
                    <div className="epo-form">
                        <label className="epo-right right-item" ref={this.passwordModel.refLabel}></label>
                        <input className="epo" ref={this.passwordModel.refInput} />
                        <div className="epo-border"></div>
                        <div className="right-item" ref={this.passwordModel.refError} ></div>
                    </div>
                    <div className="epo-form" onClick={this.register.bind(this)}>
                        <button className="btn btn-outline-001 btn-well">
                            عضویت
                        </button>
                    </div>
                </Container>
            );
        }
    }

    async register() {
        if (this.formHandler.isValid()) {
            let token = cookies.pVInfoGetRaw();
            this.formHandler.setFormData({ Token: token });
            let registerResult = await registerService(this.formHandler.getFormData<LoginModel>());
            if (registerResult!.Status === JsonResponseStatus.Success) {
                cookies.pVInfoSetRaw(registerResult.TResult001.Token);
                cookies.parseInfo(registerResult.TResult001.Token);
                window.location.href = "/manage_files/root";
            } else {
                for (let key in registerResult!.InfoData) {
                    let data = registerResult!.InfoData[key];
                    this.formHandler.addError(key, data);
                }
            }
        }
    }

    async login() {
        if (this.formHandler.isValid()) {
            let token = cookies.pVInfoGetRaw();
            this.formHandler.setFormData({ Token: token });
            let loginResult = await loginService(this.formHandler.getFormData<LoginModel>());
            if (loginResult!.Status === JsonResponseStatus.Success) {
                cookies.pVInfoSetRaw(loginResult.TResult001.Token);
                cookies.parseInfo(loginResult.TResult001.Token);
                window.location.href = "/manage_files/root";
            } else {
                for (let key in loginResult!.InfoData) {
                    let data = loginResult!.InfoData[key];
                    this.formHandler.addError(key, data);
                }
            }
        }
    }
}



