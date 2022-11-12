import * as React from 'react'
import { Home } from "./root/Home";
import { Login } from './root/identity/Login'
import { LoginLoader } from './Services/IdentityServices'
import { FileManagerLoader } from './Services/FileManagerServices'
import { PageLoader } from "./root/shared/PageLoader";
import FileManager from './root/file_manager/FileManager'
import { GetDefaultCulture } from './neptons/CultureStructure'

const AppRoutes = [
  // index: true,
  {
    index: true,
    element: <GetDefaultCulture />
  },
  {
    path: '/:culture',
    element: <Home />
  },
  {
    path: '/:culture/identity/login',
    element: <PageLoader PageContainer={Login} pageLoaderOtpions={{ Loading: LoginLoader, allowAnonymous: true }} />
  },
  {
    path: '/:culture/manage_files/:fileId',
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
