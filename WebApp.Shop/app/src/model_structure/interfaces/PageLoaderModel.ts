export default interface PageLoaderModel {
    PageContainer: React.ElementType;
    Loading?: () => Promise<any>;
}