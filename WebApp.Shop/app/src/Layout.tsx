import * as React from 'react';
import { Container } from 'react-bootstrap';
import NavMenu from './NavMenu';
import ChildItemModel  from './model_structure/interfaces/ChildItemModel'
export default class Layout extends React.Component<ChildItemModel> 
{
  
  static displayName = Layout.name;
  render() {
    return (
      <>
        <NavMenu />
        {this.props.children}
      </>
    );
  }
}
