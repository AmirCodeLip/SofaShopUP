import * as React from 'react'
import { PageLoaderOtpions, PageLoaderModel } from './../../model_structure/interfaces/PageLoaderModel'
import PageLoaderState from './../../model_structure/interfaces/PageLoaderState'
import { redirect } from "react-router-dom"
import { UserInfoFrame } from "./../shared/UserInfoFrame"
import { UrlData } from './../shared/GlobalManage'
import { Web_Modal, ModalOptions, ModalType } from './../web_modal/Web_Modal'
import * as  globalManage from '../shared/GlobalManage'
import { LoaderCommunicator } from './../../model_structure/BaseLoader'

export function Loading() {
    return (<div className="lds-ripple"><div></div><div></div></div>);
}

export class PageLoader extends React.Component<PageLoaderModel, PageLoaderState>
{
    loaderCommunicator: LoaderCommunicator;
    yesBtn = React.createRef<HTMLButtonElement>();
    noBtn = React.createRef<HTMLButtonElement>();
    makeSureText = React.createRef<HTMLDivElement>();
    makeSureOptions = new ModalOptions(ModalType.defualtModal);
    queryString: UrlData = new UrlData();
    allowAnonymous = false;
    constructor(model: PageLoaderModel) {
        super(model);
        this.allowAnonymous = model.pageLoaderOtpions.allowAnonymous;
        this.state = {
            isLoaded: false,
            pageName: ''
        };
        this.loaderCommunicator = {
            makeSure: this.makeSure.bind(this),
        };
        if (model.pageLoaderOtpions.hasSearchDrive) {
            this.loaderCommunicator.searchDrive = React.createRef<HTMLInputElement>();
        }
        (async function () {
            let cultureInfo = (await globalManage.CultureInfoImplement.Get())!!;
            cultureInfo.GetStrings("PublicWord001.key014", "PublicWord001.key017", "PublicWord001.key018", "PublicWord001.key019");
        }())
    }

    public get pageName(): string {
        let name = '';
        if (typeof (this.props.PageContainer) === "function")
            name = (this.props.PageContainer as Function).name;
        return name;
    }

    render() {
        if (this.state.isLoaded && this.state.pageName !== this.pageName) {
            this.load();
            return (<Loading></Loading>);
        }
        if (!this.allowAnonymous && !window.pVInfo.UserInfoList.find(x => x.IsDefault)?.Token) {
            window.location.replace("/identity/login_register");
        }
        return (<>
            {!this.state.isLoaded && (<Loading></Loading>)}
            {this.state.isLoaded && this.props.PageContainer &&
                <>
                    {
                        this.state.pageName !== "FileManager" &&
                        <UserInfoFrame loaderCommunicator={this.loaderCommunicator}></UserInfoFrame>
                    }
                    <this.props.PageContainer model={this.state.model} loaderCommunicator={this.loaderCommunicator} />
                </>}
            <Web_Modal middleware={this.makeSureOptions}>
                <>
                    <div className="epo-form" style={{ height: "150px" }} ref={this.makeSureText}></div>
                    <div className="epo-btn-group epo-en-right">
                        <button className="btn btn-outline-002 btn-well" ref={this.noBtn}>
                            <globalManage.localizorHtml txtKey={'PublicWord001.key017'}></globalManage.localizorHtml>
                        </button>
                        <button className="btn btn-outline-001 btn-well" ref={this.yesBtn}>
                            <globalManage.localizorHtml txtKey={'PublicWord001.key014'}></globalManage.localizorHtml>
                        </button>
                    </div>
                </>
            </Web_Modal>
        </>)
    }

    makeSure(txt: string): Promise<boolean> {
        return new Promise((resolve, reject) => {
            this.makeSureOptions.enable = true;
            let closeTab = (function (stateData: boolean) {
                this.makeSureOptions.enable = false;
                resolve(stateData);
            }).bind(this);
            setTimeout(() => {
                this.makeSureText.current.innerHTML = txt;
                this.yesBtn.current.onclick = () => { closeTab(true); };
                this.noBtn.current.onclick = () => { closeTab(false); };
            }, 200);

        });
    }

    componentWillUnmount(): void {

    }

    async componentDidUpdate(prevProps: Readonly<PageLoaderModel>, prevState: Readonly<PageLoaderState>, snapshot?: any) {
        console.log(this.state.pageName === "FileManager")
    }

    async componentDidMount() {
        // console.log(await this.makeSure());
        await this.load();
    }

    async load() {
        if (!this.props.pageLoaderOtpions) {
            this.setState({ model: null, isLoaded: true, pageName: this.pageName });
        }
        else if (!this.props.pageLoaderOtpions?.loading) {
            this.setState({ model: undefined, isLoaded: true, pageName: this.pageName })
        }
        else if (this.props.pageLoaderOtpions?.loading) {
            this.props.pageLoaderOtpions.loading().then(data => {
                if (data == null) {
                    redirect("/identity/login_register");
                    return;
                }
                else {
                    this.setState({ model: data, isLoaded: true, pageName: this.pageName })
                }
            });
        }
    }
}

