import { AspDataType } from "../EnumItems";
import { KeyValuePair } from "./KeyValuePair";

export interface IInfoInput {
  DisplayName: string;
  Name: string;
  AspDataType: AspDataType,
  MinLength?: number;
  MaxLength?: number;
  ErrorMsgs: Array<KeyValuePair<string, string>>
}

export class InfoInput implements IInfoInput {
  DisplayName: string = "";
  Name: string = "";
  AspDataType: AspDataType = AspDataType.Text;
  MinLength?: number | undefined;
  MaxLength?: number | undefined;
  ErrorMsgs: KeyValuePair<string, string>[] = [];
  public errorListeners?: (error: string) => void;
}
