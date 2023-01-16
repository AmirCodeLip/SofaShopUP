import {FObjectType} from './FObjectType'

export default interface FObjectKind {
Id?: string | null,
FolderId: string | null,
Name: string,
Path: string,
FObjectType: FObjectType,
TypeKind: string
}
