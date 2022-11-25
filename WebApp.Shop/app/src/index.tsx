import * as React from 'react';
import * as ReactDOM from 'react-dom/client';
import Layout from './Layout';
import { BrowserRouter, Route, Routes, Link } from "react-router-dom";
import AppRoutes from './AppRoutes';
import { CultureInfoImplement } from './root/shared/GlobalManage'
let rootElt = document.getElementById('root');

(async function () {
    let cultureInfo = await CultureInfoImplement.Get();
    await cultureInfo.GetStrings("PublicWord001.key003", "PublicWord001.key004", "PublicWord001.key005", "PublicWord001.key006", "PublicWord001.key007");
    await cultureInfo.TransmitWord();
    ReactDOM.createRoot(rootElt).render(
        <>
            <BrowserRouter>
                <Layout>
                    <Routes>
                        {AppRoutes.map((route, index) => {
                            const { element, ...rest } = route;
                            return <Route key={index} {...rest} element={element} />;
                        })}
                    </Routes>
                </Layout>
            </BrowserRouter>
        </>
    );
}());