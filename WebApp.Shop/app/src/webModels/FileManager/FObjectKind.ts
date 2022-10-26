import { FObjectType } from './FObjectType'

export default interface FObjectKind {
    Id: string,
    FolderId: string | null,
    Name: string,
    FObjectType: FObjectType
}
