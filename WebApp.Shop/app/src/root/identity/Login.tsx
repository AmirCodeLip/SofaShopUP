import * as React from 'react';
import DataTransmitter from '../../Services/DataTransmitter'
import IFormModel from './../../mylibraries/asp-communication/interfaces/IFormModel'
import { FormModeInput, FormHandler } from '../../mylibraries/asp-communication/components/FormModelItem';
import { loginService } from '../../Services/IdentityServices'
import { JsonResponseStatus } from '../../models/JsonResponse';
export const Login: React.FC<{ model: IFormModel }> = ({ model }) => {
    let phoneOrEmailModel: FormModeInput = new FormModeInput(model, "PhoneOrEmail");
    let passwordModel = new FormModeInput(model, "Password");
    let formHandler = new FormHandler(phoneOrEmailModel, passwordModel);
    formHandler.initRef(React.useRef);
    React.useEffect(() => {
        formHandler.init();
    }, []);

    async function login() {
        if (formHandler.isValid()) {
            let loginResult = await loginService({
                PhoneOrEmail: phoneOrEmailModel.refInput.current!.value,
                Password: passwordModel.refInput.current!.value
            });
            if (loginResult!.status === JsonResponseStatus.Success) {
                console.log(loginResult?.tResult001);
                localStorage.setItem("jwt", loginResult!.tResult001!.token)
            } else {
                for (let key in loginResult!.infoData) {
                    let data = loginResult!.infoData[key];
                    formHandler.addError(key, data);
                }
            }
        }
    }

    return (
        <>
            <div className="epo-form">
                <label className='epo-right right-item' ref={phoneOrEmailModel.refLabel}></label>
                <input className="epo left-item" ref={phoneOrEmailModel.refInput} />
                <div className="epo-border"></div>
                <div className="right-item" ref={phoneOrEmailModel.refError} ></div>
            </div>
            <div className="epo-form">
                <label className="epo-right right-item" ref={passwordModel.refLabel}></label>
                <input className="epo" ref={passwordModel.refInput} />
                <div className="epo-border"></div>
                <div className="right-item" ref={passwordModel.refError} ></div>
            </div>
            <div className="epo-form" onClick={login}>
                <button className="btn btn-outline-001 btn-well">
                    ورود
                </button>
            </div>
        </>
    );
}


