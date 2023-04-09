import * as React from 'react';
import * as ReactDOM from 'react-dom/client';
import Layout from './root/file_manager/Layout';
import { BrowserRouter, Route, Router, Routes, Link } from "react-router-dom";
import AppRoutes from './AppRoutes';
import { CultureInfoImplement } from './root/shared/GlobalManage';

let rootElt = document.getElementById('root');

// interface History {
//     action: string,
//     location: string
// }
interface Props {
    children?: React.ReactNode;
    history: any
    // any props that come into the component
}

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
    cssItems.push("/css/dxcss/dx.light.css");
    cssItems.push("/css/input-pack.css");
    cssItems.push("/css/styles.css");
    cssItems.push("/css/nav-menu.css");
    cssItems.push("/css/area/identity/login_register.css");
    cssItems.push("/css/area/file_manager/file_manager.css");
    cssItems.push("/css/area/file_manager/layout.css");
    cssItems.push("/css/web_modal.css");
    if (window.cultureInfo!!.cultureInfo.Rtl) {
        cssItems.push("/css/nav-menu.rtl.css");
        cssItems.push("/css/area/file_manager/layout.rtl.css");
        cssItems.push("/css/input-pack.rtl.css");
        cssItems.push("/css/styles.rtl.css");
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
                    <Routes>
                        {AppRoutes.map((route, index) => {
                            const { element, ...rest } = route;
                            return <Route key={index} {...rest} element={element} />;
                        })}
                    </Routes>
                </BrowserRouter>
            </body>
        </>
    );
}());