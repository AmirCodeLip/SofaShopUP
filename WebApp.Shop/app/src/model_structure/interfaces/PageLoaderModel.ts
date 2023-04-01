export interface PageLoaderModel {
    PageContainer?: React.ElementType;
    pageLoaderOtpions?: PageLoaderOtpions;
}
export interface PageLoaderOtpions {
    Loading?: () => Promise<any>;
    allowAnonymous?: boolean;
}