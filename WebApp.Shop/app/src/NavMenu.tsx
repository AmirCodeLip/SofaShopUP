import * as React from 'react';
import { Container } from 'react-bootstrap';
import { Link } from "react-router-dom";

interface UrlContent {
    title: string,
    path: string,
}

export default class NavMenu extends React.Component {
    render() {
        let urlContents: UrlContent[] = []
        urlContents.push({
            path: "/",
            title: "خانه",
        });
        urlContents.push({
            path: "/counter",
            title: "یک",
        });
        urlContents.push({
            path: "/manage_files",
            title: "فایل",
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
                                    {urlContent.title}
                                </Link>
                            </li>
                        )
                    }
                </ul>
                <div className="web-authentication">
                    <a>
                        ورود/عضویت
                    </a>
                    <ul>
                        <li>
                            <Link className="nav-router" to={"identity/login"}>
                                ورود
                            </Link>
                        </li>
                        <li>
                            <a className="nav-router" asp-area="Identity" asp-page="/Account/Register">
                                عضویت
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>);
    }
}
