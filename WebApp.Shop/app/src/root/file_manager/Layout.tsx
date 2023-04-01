import * as React from 'react';
import ChildItemModel from '../../model_structure/interfaces/ChildItemModel';
import * as  globalManage from '../shared/GlobalManage'
import { Loading } from '../shared/PageLoader';
import { useNavigation, Route, Routes, Link } from "react-router-dom";
import { BootstrapRecognizer, BTSizes } from "./../../mylibraries/BootstrapRecognizer"
interface LayoutState {
  navMode: "openFullSide" | "close",
  sideWidth: number,
  load: Boolean,
  phoneOrTablet: boolean
}

interface SearchItemHolder {
  val: string,
  time: number
}
export default class Layout extends React.Component<ChildItemModel, LayoutState>
{
  rootRegix = new RegExp('root(\/[a-zA-Z]{1,}){1,}');
  spaceDescription = '{0} used of {1}';
  changeSide: boolean;
  // static displayName = Layout.name;
  bootstrapRecognizer = new BootstrapRecognizer();
  sideItemRef: React.RefObject<HTMLDivElement> = React.createRef<HTMLDivElement>();
  /** search bar input element */
  searchDrive: React.RefObject<HTMLInputElement> = React.createRef<HTMLInputElement>();
  /** 
       * every time some one search in search box 
       * we count every second and put in this
       * */
  sendSearchRequestTimer?: NodeJS.Timer;
  /**
   * every time some one search in search box 
   * we put their search in this
   */
  searchItems: Array<SearchItemHolder> = [];

  constructor(props: ChildItemModel) {
    super(props);
    this.state = { navMode: "openFullSide", sideWidth: 19, load: false, phoneOrTablet: false };
    this.bootstrapRecognizer.events.push(((d: BTSizes) => { this.setState({ phoneOrTablet: d.phoneOrTablet }) }).bind(this));
    document.addEventListener("mousemove", this.checkSideNavMove.bind(this));
    document.addEventListener("mouseup", this.mouseup.bind(this));
  }

  render() {
    if (!this.state.load)
      return (<Loading></Loading>);

    return (<>
      <div className='header-bar border-hide-bottom'>
        <div className='logo-box'>
          <img src="/logo192.png" />
          <div className='content-title'>
            file manager
          </div>
        </div>
        <div className='search-box'>
          <button>
            <i className='fa fa-search'></i>
          </button>
          <input className="search-drive" ref={this.searchDrive} onKeyDown={this.checkPathKeys} onKeyUp={this.search.bind(this)} />
          <div className="s-place-holder">search here</div>
        </div>
        <div className='adjustment-toolbar'>
          <div className='user-settings'>
            <img className='user-icon' src='/personal.jpg' />
          </div>
        </div>
      </div>
      <div className='user-info-frame'>
        <div className='user-info-header'>
          <img className="user-info-img" src="/personal.jpg"></img>
          <div className="user-info-profile">
            amir code lip
          </div>
          <div className='user-setting'>
            <i className="fa-solid fa-gear"></i>
          </div>
        </div>
        <div className="setting-devider"></div>
      </div>
      <div className='shutter-view'>
        <div className='main-loader hide'><div className='main-loader-spin'></div></div>
        <div className={this.getClassItem("view-side-center")} style={this.getSideNavStyle()}>
          <div className='side-grid-center'>
            <div className='link-list'>
              <Link className={this.getClassItem('link-list-item')} to={`/${window.cultureInfo?.cultureInfo.Culture}/manage_files/root`}>
                <i className={this.getClassItem("link-list-logo") + ' fa-solid fa-folder'}></i>
                <span className={this.getClassItem('link-list-text')}>
                  <globalManage.localizorHtml txtKey={'PublicWord001.key008'}></globalManage.localizorHtml>
                </span>
              </Link>
              <Link className={this.getClassItem('link-list-item')} to={`/${window.cultureInfo?.cultureInfo.Culture}/manage_files/images`}>
                <i className={this.getClassItem("link-list-logo") + ' fa-solid fa-image'}></i>
                <span className={this.getClassItem('link-list-text')}>
                  <globalManage.localizorHtml txtKey={'PublicWord001.key009'}></globalManage.localizorHtml>
                </span>
              </Link>
              <Link className={this.getClassItem('link-list-item')} to={`/${window.cultureInfo?.cultureInfo.Culture}/manage_files/videos`}>
                <i className={this.getClassItem("link-list-logo") + ' fa-solid fa-video'}></i>
                <span className={this.getClassItem('link-list-text')}>
                  <globalManage.localizorHtml txtKey={'PublicWord001.key010'}></globalManage.localizorHtml>
                </span>
              </Link>
              <Link className={this.getClassItem('link-list-item')} to={`/${window.cultureInfo?.cultureInfo.Culture}/manage_files/audios`}>
                <i className={this.getClassItem("link-list-logo") + ' fa-solid fa-music'}></i>
                <span className={this.getClassItem('link-list-text')}>
                  <globalManage.localizorHtml txtKey={'PublicWord001.key011'}></globalManage.localizorHtml>
                </span>
              </Link>
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

  search(e: any) {
    let val = e.target.value;
    if (!this.sendSearchRequestTimer) {
      this.sendSearchRequestTimer = setInterval(this.sendSearchRequest.bind(this), 1500);
    }
    this.searchItems.push({
      val: val,
      time: new Date().getTime()
    });
  }
  /**
   * every time some one search in search box 
   * first we get last time their search
   * then if it not exist we clear search process
   * if process is equal to  root and we aren't in root 
   * we set folder to root and load data again
   */
  sendSearchRequest() {
    let lastItem = this.searchItems[this.searchItems.length - 1];
    this.searchItems = this.searchItems.filter(c => c.time > lastItem.time)
  }

}

