import * as React from 'react'
import * as  globalManage from '../shared/GlobalManage'
import { Loading } from '../shared/PageLoader'
import { useNavigation, Route, Routes, Link } from "react-router-dom"
import { BootstrapRecognizer, BTSizes } from "./../../mylibraries/BootstrapRecognizer"
import { UserInfoFrame } from "./../shared/UserInfoFrame"
import { LoaderCommunicator } from './../../model_structure/BaseLoader'
import { LayoutState, PLayout } from './TypeAndModules'


export default class Layout extends React.Component<PLayout, LayoutState>
{
  rootRegix = new RegExp('root(\/[a-zA-Z]{1,}){1,}');
  spaceDescription = '{0} used of {1}';
  changeSide: boolean;
  // static displayName = Layout.name;
  bootstrapRecognizer = new BootstrapRecognizer();
  sideItemRef: React.RefObject<HTMLDivElement> = React.createRef<HTMLDivElement>();
  loaderCommunicator: LoaderCommunicator;

  constructor(props: PLayout) {
    super(props);
    this.state = { navMode: "openFullSide", sideWidth: 19, load: false, phoneOrTablet: false };
    this.loaderCommunicator = props.loaderCommunicator;
    this.bootstrapRecognizer.events.push(((d: BTSizes) => { this.setState({ phoneOrTablet: d.phoneOrTablet }) }).bind(this));
    document.addEventListener("mousemove", this.checkSideNavMove.bind(this));
    document.addEventListener("mouseup", this.mouseup.bind(this));
  }

  render() {
    if (!this.state.load)
      return (<Loading></Loading>);

    return (<>
      <UserInfoFrame loaderCommunicator={this.loaderCommunicator}></UserInfoFrame>
      <div className='shutter-view'>
        <div className='main-loader hide'><div className='main-loader-spin'></div></div>
        <div className={this.getClassItem("view-side-center")} style={this.getSideNavStyle()}>
          <div className='side-grid-center'>
            <div className='link-list'>
              <a className={this.getClassItem('link-list-item')} onClick={() => this.changeTab.bind(this)("root")}>
                <i className={this.getClassItem("link-list-logo") + ' fa-solid fa-folder'}></i>
                <span className={this.getClassItem('link-list-text')}>
                  {/* root */}
                  <globalManage.localizorHtml txtKey={'PublicWord001.key008'}></globalManage.localizorHtml>
                </span>
              </a>
              <a className={this.getClassItem('link-list-item')} onClick={() => this.changeTab.bind(this)("images")}>
                <i className={this.getClassItem("link-list-logo") + ' fa-solid fa-image'}></i>
                <span className={this.getClassItem('link-list-text')}>
                  {/* images */}
                  <globalManage.localizorHtml txtKey={'PublicWord001.key009'}></globalManage.localizorHtml>
                </span>
              </a>
              <a className={this.getClassItem('link-list-item')} onClick={() => this.changeTab.bind(this)("videos")}>
                <i className={this.getClassItem("link-list-logo") + ' fa-solid fa-video'}></i>
                <span className={this.getClassItem('link-list-text')}>
                  {/* videos */}
                  <globalManage.localizorHtml txtKey={'PublicWord001.key010'}></globalManage.localizorHtml>
                </span>
              </a>
              <a className={this.getClassItem('link-list-item')} onClick={() => this.changeTab.bind(this)("audios")}>
                <i className={this.getClassItem("link-list-logo") + ' fa-solid fa-music'}></i>
                <span className={this.getClassItem('link-list-text')}>
                  {/* audios */}
                  <globalManage.localizorHtml txtKey={'PublicWord001.key011'}></globalManage.localizorHtml>
                </span>
              </a>
              <div className={this.getClassItem('drive-info')} style={this.getDriveInfoStyle()}>
                <div className='top-divider'></div>
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
        </div>
        <div className={this.getMain()} style={this.getMainStyle()}>
          {this.props.children}
        </div>
      </div>
    </>);
  }

  componentDidMount() {
    this.bootstrapRecognizer.involve();
    var readyStateCheckInterval = setInterval((function () {
      if (document && document.readyState === 'complete') { // Or 'interactive'
        clearInterval(readyStateCheckInterval);
        setTimeout(() => {
          this.setState({ load: true });
        }, 300);
      }
    }).bind(this), 10);
  }

  flexClient(ev: MouseEvent): number {
    let clientX = window.cultureInfo.cultureInfo.Rtl ? document.body.offsetWidth - ev.clientX : ev.clientX;
    if (clientX > 300) {
      clientX = 300;
    }
    return clientX;
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
    if (this.flexClient(ev) < 115) {
      this.setState({ navMode: "close", sideWidth: 4 });
    }
  }

  checkSideNavMove(ev: MouseEvent) {
    if (!this.changeSide) return;
    let rtl = window.cultureInfo.cultureInfo.Rtl;
    let width = 0;
    let clientX = this.flexClient(ev);
    if (rtl) {
      width = (100 * clientX) / document.body.offsetWidth;
    }
    else {
      width = (100 * clientX) / document.body.offsetWidth;
    }
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
    if (this.state.phoneOrTablet) {
      // result.display = "none";
    }
    else {
      switch (this.state.navMode) {
        case "close": result.width = "0%";
        case "openFullSide":
        default: result.width = this.state.sideWidth + "%";
      }
    }
    return result;
  }

  getDriveInfoStyle(): object {
    let result: React.CSSProperties = {};
    switch (this.state.navMode) {
      case "close": result.width = "0%";
      case "openFullSide":
      default: result.width = "unset";
    }
    return result;
  }

  getMainStyle() {
    let result: React.CSSProperties = {};
    if (this.state.phoneOrTablet) {
      result.width = "100%";
    }
    else {
      switch (this.state.navMode) {
        case "close": result.width = "100%";
        case "openFullSide":
        default: result.width = (100 - this.state.sideWidth) + "%";
      }
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



  checkPathKeys(e: any) {
    let wrongKey = [9, 187, 188, 190, 192, 220, 222];
    if (wrongKey.includes(e.keyCode)) {
      e.preventDefault();
      return;
    }
  }

  ///change page by click on side link
  async changeTab(floderId: string) {
    await this.props.invokeEvent("SetFolderId", floderId);
  }

}
