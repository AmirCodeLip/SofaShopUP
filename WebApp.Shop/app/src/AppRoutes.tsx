import * as React from 'react'
import { Home } from "./root/Home";
import { IdentityPanel } from './root/identity/Login'
import { identityLoader } from './Services/IdentityServices'
import { FileManagerLoader } from './Services/FileManagerServices'
import { PageLoader } from "./root/shared/PageLoader";
import FileManager from './root/file_manager/FileManager'
import { GetDefaultCulture } from './root/shared/GlobalManage'

const AppRoutes = [
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
    element: <PageLoader PageContainer={IdentityPanel} pageLoaderOtpions={{ Loading: identityLoader, allowAnonymous: true }} />
  },
  {
    path: '/:culture/manage_files/:fileId',
    element: <PageLoader PageContainer={FileManager} pageLoaderOtpions={{ Loading: FileManagerLoader, allowAnonymous: false }} />
  },
];
const extera: Array<any> = [];
for (let route of AppRoutes) {
  if (route.index)
    continue;
  extera.push({
    path: route.path!!.replace('/:culture', ''),
    element: <GetDefaultCulture />
  })
}

export default [...extera, ...AppRoutes];
