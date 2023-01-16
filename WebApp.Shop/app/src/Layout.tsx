import * as React from 'react';
import { Container } from 'react-bootstrap';
import NavMenu from './NavMenu';
import ChildItemModel from './model_structure/interfaces/ChildItemModel';
import { Col, Row } from 'react-bootstrap';
import * as  globalManage from './root/shared/GlobalManage'
import { Link } from "react-router-dom";
import { Loading } from './root/shared/PageLoader';

interface LayoutState {
  navMode: "openFullSide" | "close",
  sideWidth: number,
  load: Boolean
}

export default class Layout extends React.Component<ChildItemModel, LayoutState>
{
  spaceDescription = '{0} used of {1}';
  changeSide: boolean;
  static displayName = Layout.name;
  sideItemRef: React.RefObject<HTMLDivElement>;


  constructor(props: ChildItemModel) {
    super(props);
    this.state = { navMode: "openFullSide", sideWidth: 19, load: false };
    document.addEventListener("mousemove", this.checkSideNavMove.bind(this));
    this.sideItemRef = React.createRef<HTMLDivElement>();
    window.document.addEventListener("mouseup", this.mouseup.bind(this));

  }

  render() {
    if (!this.state.load)
      return (<Loading></Loading>);

    return (
      <div className='shutter-view'>
        <div className='main-loader hide'><div className='main-loader-spin'></div></div>
        <div className={this.getClassItem("view-side-center")} style={this.getSideNavStyle()}>
          <div className='side-grid-center'>
            <div className='link-list'>
              <Link className={this.getClassItem('link-list-item')} to={`/${window.cultureInfo.cultureInfo.Culture}/manage_files/root`}>
                <i className={this.getClassItem("link-list-logo") + ' fa-solid fa-folder'}></i>
                <span className={this.getClassItem('link-list-text')}>
                  <globalManage.localizorHtml txtKey={'PublicWord001.key008'}></globalManage.localizorHtml>
                </span>
              </Link>
              <Link className={this.getClassItem('link-list-item')}  to={`/${window.cultureInfo.cultureInfo.Culture}/manage_files/images`}>
                <i className={this.getClassItem("link-list-logo") + ' fa-solid fa-image'}></i>
                <span className={this.getClassItem('link-list-text')}>
                  <globalManage.localizorHtml txtKey={'PublicWord001.key009'}></globalManage.localizorHtml>
                </span>
              </Link>
              <Link className={this.getClassItem('link-list-item')}  to={`/${window.cultureInfo.cultureInfo.Culture}/manage_files/videos`}>
                <i className={this.getClassItem("link-list-logo") + ' fa-solid fa-video'}></i>
                <span className={this.getClassItem('link-list-text')}>
                  <globalManage.localizorHtml txtKey={'PublicWord001.key010'}></globalManage.localizorHtml>
                </span>
              </Link>
              <Link className={this.getClassItem('link-list-item')}  to={`/${window.cultureInfo.cultureInfo.Culture}/manage_files/audios`}>
                <i className={this.getClassItem("link-list-logo") + ' fa-solid fa-music'}></i>
                <span className={this.getClassItem('link-list-text')}>
                  <globalManage.localizorHtml txtKey={'PublicWord001.key011'}></globalManage.localizorHtml>
                </span>
              </Link>
            </div>
            <div className={this.getClassItem('drive-info')}>
              <div className="progress-view">
                <div className="progress-view-fill"></div>
                <div className="progress-view-bar"></div>
              </div>
              <div className='drive-info-description'>
                {this.spaceDescription.replace('{0}', '272 MB').replace('{1}', '5 GB')}
              </div>
            </div>
            <div className='right-border' onMouseDown={this.rightBorderMouseDown.bind(this)} ref={this.sideItemRef}></div>
          </div>
        </div>
        <div className={this.getMain()} style={this.getMainStyle()}>
          {this.props.children}
        </div>
      </div>
    );
  }

  componentDidMount() {
    var readyStateCheckInterval = setInterval((function () {
      if (document && document.readyState === 'complete') { // Or 'interactive'
        clearInterval(readyStateCheckInterval);
        setTimeout(() => {
          this.setState({ load: true });
        }, 300);
      }
    }).bind(this), 10);
  }

  componentWillUnmount() {
  }

  rightBorderMouseDown(ev: MouseEvent) {
    this.changeSide = true
    if (this.state.navMode !== "openFullSide") {
      this.setState({ navMode: "openFullSide" });
    }
  }

  mouseup(ev: MouseEvent) {
    if (!this.changeSide) return;
    this.changeSide = false;
    if (ev.clientX < 115) {
      this.setState({ navMode: "close", sideWidth: 4 });
    }
  }

  checkSideNavMove(ev: MouseEvent) {
    if (!this.changeSide) return;
    let clientX = ev.clientX;
    if (clientX > 300) {
      clientX = 300;
    }
    let width = (100 * clientX) / document.body.offsetWidth;
    this.setState({
      sideWidth: width,
    });
  }

  getClassItem(className: string): string {
    switch (this.state.navMode) {
      case "close": return `${className} close`;
      case "openFullSide":
      default: return className;
    }
  }

  getSideNavStyle(): object {
    let result: React.CSSProperties = {};
    switch (this.state.navMode) {
      case "close": result.width = "0%";
      case "openFullSide":
      default: result.width = this.state.sideWidth + "%";
    }
    return result;
  }

  getMainStyle() {
    let result: React.CSSProperties = {};
    switch (this.state.navMode) {
      case "close": result.width = "100%";
      case "openFullSide":
      default: result.width = (100 - this.state.sideWidth) + "%";
    }
    return result;
  }

  getMain(): string {
    switch (this.state.navMode) {
      case "close": return "view-main-center close";
      case "openFullSide":
      default: return "view-main-center";
    }
  }



}
