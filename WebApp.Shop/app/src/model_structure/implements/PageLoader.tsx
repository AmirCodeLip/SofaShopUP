import * as React from 'react';
import { PageLoaderOtpions, PageLoaderModel } from './../interfaces/PageLoaderModel'
import PageLoaderState from './../interfaces/PageLoaderState'

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
            return;
        }
        if (this.props.pageLoaderOtpions.allowAnonymous === false) {
            let jwt = localStorage.getItem("jwt");
            if (jwt == null)
                window.location.replace("/identity/login");
        }
        return (<>
            {!this.state.isLoaded && (
                <div>
                    ...loding
                </div>
            )}
            {this.state.isLoaded && <this.props.PageContainer model={this.state.model} />}
        </>)
    }

    componentWillUnmount(): void {

    }

    async componentDidUpdate(prevProps: Readonly<PageLoaderModel>, prevState: Readonly<PageLoaderState>, snapshot?: any) {
        // await this.load();
    }

    async componentDidMount() {
        await this.load();
    }

    async load() {
        if (this.props.pageLoaderOtpions.Loading) {
            this.props.pageLoaderOtpions.Loading().then(data => {
                this.setState({ model: data, isLoaded: true, pageName: this.pageName })
            });
        }
    }
}

