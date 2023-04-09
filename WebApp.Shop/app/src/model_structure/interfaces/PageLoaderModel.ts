
export interface PageLoaderModel {
    PageContainer?: React.ElementType;
    pageLoaderOtpions?: PageLoaderOtpions;
}
export class PageLoaderOtpions {
    constructor(loading?: () => Promise<any>, allowAnonymous = true, hasSearchDrive = false, showNavbar = true) {
        this.allowAnonymous = allowAnonymous;
        this.hasSearchDrive = hasSearchDrive;
        this.showNavbar = showNavbar;
        this.loading = loading;
    }
    loading?: () => Promise<any>;
    allowAnonymous: boolean;
    hasSearchDrive: boolean;
    showNavbar: boolean;
}