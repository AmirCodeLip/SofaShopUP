import * as React from 'react';
import { Container } from 'react-bootstrap';
import * as  globalManage from './root/shared/GlobalManage'
import {
    useNavigation,
    Form,
    redirect, Link
} from "react-router-dom";
interface UrlContent {
    title: string,
    path: string,
}

export default class NavMenu extends React.Component {
    render() {
        let urlContents: UrlContent[] = []
        urlContents.push({
            path: "/",
            title: "PublicWord001.key003",
        });
        urlContents.push({
            path: `/${window.cultureInfo!!.cultureInfo.Culture}/manage_files/root`,
            title: "PublicWord001.key004",
        }); 
        return (<div className='nav-menue'>
            <div className="header-line">
                <div className="weblogo">
                    <img src="/logo192.png" />
                </div>
            </div>
            <div className="nav-web">
                <ul>
                    {
                        urlContents.map((urlContent, index) =>
                            <li key={index}>
                                <Link className="nav-router" to={urlContent.path}>
                                    <globalManage.localizorHtml txtKey={urlContent.title}></globalManage.localizorHtml>
                                </Link>
                            </li>
                        )
                    }
                </ul>
                <div className="web-authentication">
                    <a>
                        <globalManage.localizorHtml txtKey="PublicWord001.key005"></globalManage.localizorHtml>
                    </a>
                    <ul>
                        <li>
                            <Link className="nav-router" to={window.cultureInfo!!.cultureInfo.Culture + "/identity/login"}>
                                <globalManage.localizorHtml txtKey="PublicWord001.key006"></globalManage.localizorHtml>
                            </Link>
                        </li>
                        <li>
                            <Link className="nav-router" to={window.cultureInfo!!.cultureInfo.Culture + "/identity/login"}>
                                <globalManage.localizorHtml txtKey="PublicWord001.key007"></globalManage.localizorHtml>
                            </Link>
                        </li>
                    </ul>
                </div>
            </div>
        </div>);
    }
}
