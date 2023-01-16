import * as React from 'react';
import * as ReactDOM from 'react-dom/client';
import Layout from './Layout';
import { BrowserRouter, Route, Routes, Link } from "react-router-dom";
import AppRoutes from './AppRoutes';
import { CultureInfoImplement } from './root/shared/GlobalManage';

let rootElt = document.getElementById('root');

(async function () {
    let cultureInfo = (await CultureInfoImplement.Get())!!;
    await cultureInfo.GetStrings("PublicWord001.key003", "PublicWord001.key004", "PublicWord001.key005",
        "PublicWord001.key006", "PublicWord001.key007", "PublicWord001.key008", "PublicWord001.key009", "PublicWord001.key010",
        "PublicWord001.key011");
    await cultureInfo.TransmitWord();
    let cssItems: Array<string> = [];
    cssItems.push("/css/loading.css");
    cssItems.push("/css/bootstrap.min.css");
    cssItems.push("/css/fontawesome/all.min.css");
    cssItems.push("/css/styles.css");
    cssItems.push("/css/input-pack.css");
    cssItems.push("/css/nav-menu.css");
    cssItems.push("http://localhost:5220/staticdirectories/src/root/file_manager/file-manager.css");
    cssItems.push("http://localhost:5220/staticdirectories/src/root/web_modal/Web_Modal.css");
    cssItems.push("http://localhost:5220/staticdirectories/src/layout.css");
    if (window.cultureInfo!!.cultureInfo.Rtl) {
        cssItems.push("/css/nav-menu.rtl.css");
        cssItems.push("http://localhost:5220/staticdirectories/src/layout.rtl.css");
    }
    ReactDOM.createRoot(document.getElementsByTagName("html")[0]).render(
        <>
                <head>
                    <meta charSet="utf-8" />
                    <meta name="viewport" content="width=device-width, initial-scale=1" />
                    <link rel="apple-touch-icon" href="/logo192.png" />
                    <link rel="manifest" href="/manifest.json" />
                    <link rel="stylesheet" href="" />
                    {cssItems.map((item, key) => {
                        return (<link rel="stylesheet" href={item} key={key} />)
                    })}
                    <title>React App</title>
                </head>
                <body>

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
                </body>
        </>
    );
}());