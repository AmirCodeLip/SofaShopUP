import * as React from 'react'
import { Home } from "./root/Home";
import { Login } from './root/identity/Login'
import { LoginLoader } from './Services/IdentityServices'
import { FileManagerLoader } from './Services/FileManagerServices'
import { PageLoader } from "./model_structure/implements/PageLoader";
import FileManager from './root/file_manager/FileManager'
 

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/identity/login',
    element: <PageLoader PageContainer={Login} pageLoaderOtpions={{ Loading: LoginLoader, allowAnonymous: true }} />
  },
  {
    path: '/manage_files',
    element: <PageLoader PageContainer={FileManager} pageLoaderOtpions={{ Loading: FileManagerLoader, allowAnonymous: false }} />
  },
  // {
  //   path: '/counter',
  //   element: <Counter />
  // },
  // {
  //   path: '/fetch-data',
  //   element: <FetchData />
  // }
];

export default AppRoutes;
