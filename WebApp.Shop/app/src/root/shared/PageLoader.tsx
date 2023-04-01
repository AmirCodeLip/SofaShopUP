import * as React from 'react';
import { PageLoaderOtpions, PageLoaderModel } from './../../model_structure/interfaces/PageLoaderModel'
import PageLoaderState from './../../model_structure/interfaces/PageLoaderState'
import { CultureInfoImplement } from './GlobalManage'
import { redirect } from "react-router-dom";

export function Loading() {
    return (<div className="lds-ripple"><div></div><div></div></div>);
}

export class PageLoader extends React.Component<PageLoaderModel, PageLoaderState>
{
    constructor(model: PageLoaderModel) {
        super(model);
        this.state = {
            isLoaded: false,
            pageName: ''
        };
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
        if (!window.pVInfo.UserInfoList.find(x=>x.IsDefault)?.Token) {
                window.location.replace("/identity/login");
        }
        return (<> 
            {!this.state.isLoaded && (<Loading></Loading>)}
            {this.state.isLoaded && this.props.PageContainer && <this.props.PageContainer model={this.state.model} />}
        </>)
    }

    componentWillUnmount(): void {

    }

    async componentDidUpdate(prevProps: Readonly<PageLoaderModel>, prevState: Readonly<PageLoaderState>, snapshot?: any) {
        console.log(this.state.pageName)
    }

    async componentDidMount() {
        await this.load();
    }

    async load() {
        if(!this.props.pageLoaderOtpions)
        {
            this.setState({ model: null, isLoaded: true, pageName: this.pageName });
        }
        else if (this.props.pageLoaderOtpions?.Loading) {
            this.props.pageLoaderOtpions.Loading().then(data => {
                if (data == null) {
                    redirect("/identity/login");
                    return;
                }
                else {
                    this.setState({ model: data, isLoaded: true, pageName: this.pageName })
                }
            });
        }
    }
}

