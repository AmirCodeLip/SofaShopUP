import * as React from 'react';
import PageLoaderModel from './../interfaces/PageLoaderModel'
import PageLoaderState from './../interfaces/PageLoaderState'

export const PageLoader: React.FC<PageLoaderModel> = function (PageLoaderModel) {
    const [state, setState] = React.useState<PageLoaderState>({ isLoaded: false });
    React.useEffect(() => {
        if (PageLoaderModel.Loading) {
            PageLoaderModel.Loading().then(data => {
                setState({ model: data, isLoaded: true })
            });
        }
    },[]);
    return (
        <>
            {!state.isLoaded && (
                <div>
                    ...loding
                </div>
            )}
            {state.isLoaded && <PageLoaderModel.PageContainer model={state.model} />}
        </>
    )

}
