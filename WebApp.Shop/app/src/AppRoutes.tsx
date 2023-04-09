import * as React from 'react'
import { Home } from "./root/Home";
import { IdentityPanel } from './root/identity/IdentityPanel'
import { identityLoader } from './Services/IdentityServices'
import { FileManagerLoader } from './Services/FileManagerServices'
import { PageLoader } from "./root/shared/PageLoader";
import FileManager from './root/file_manager/FileManager'
import { GetDefaultCulture } from './root/shared/GlobalManage'
import { PageLoaderOtpions } from './model_structure/interfaces/PageLoaderModel'


const AppRoutes = [
  {
    index: true,
    element: <GetDefaultCulture />
  },
  {
    path: '/:culture',
    element: <PageLoader PageContainer={Home} pageLoaderOtpions={new PageLoaderOtpions()}></PageLoader>
  },
  {
    path: '/:culture/identity/login_register',
    element: <PageLoader PageContainer={IdentityPanel} pageLoaderOtpions={new PageLoaderOtpions(identityLoader, true, false, false)} />
  },
  {
    path: '/:culture/manage_files/:fileId',
    element: <PageLoader PageContainer={FileManager} pageLoaderOtpions={new PageLoaderOtpions(FileManagerLoader, true, true, true)} />
  }
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