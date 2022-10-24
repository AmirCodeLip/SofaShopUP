import * as React from 'react';
import * as ReactDOM from 'react-dom/client';
import Layout from './Layout';
import { BrowserRouter, Route, Routes, Link } from "react-router-dom";
import AppRoutes from './AppRoutes';
let rootElt = document.getElementById('root');


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
