import * as React from 'react';
import Layout from './Layout'
import { BrowserRouter, Route, Routes, Link } from "react-router-dom";
import AppRoutes from './AppRoutes';

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
