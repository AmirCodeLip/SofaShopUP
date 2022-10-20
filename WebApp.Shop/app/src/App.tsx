import * as React from 'react';
import Layout from './Layout'
import { BrowserRouter, Route, Routes, Link } from "react-router-dom";
import AppRoutes from './AppRoutes';
import { OdataSetProtocol } from './../src/Services/OdataServices';
import DataTransmitter from './Services/DataTransmitter'
class Test {
  name = "amir";
  age = 24;
}
(async function () {
  console.log(await new OdataSetProtocol<Test>(DataTransmitter.BaseUrl + "odata/FObjectKind").Execute())
}());
// new OdataSetProtocol<Test>("").Where(function (x) {
//   return x.name === "amir";
// })

export default class App extends React.Component {
  render() {
    return (
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
    );
  }
}
