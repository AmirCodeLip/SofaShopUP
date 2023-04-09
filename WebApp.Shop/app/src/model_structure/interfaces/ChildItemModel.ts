import { Location } from 'react-router-dom'
import { LoaderCommunicator } from './../../model_structure/BaseLoader'

export default interface ChildItemModel {
    children: JSX.Element;
    loaderCommunicator: LoaderCommunicator;
}