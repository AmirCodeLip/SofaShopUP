import { AspDataType } from "../EnumItems";
import FormDescriptor from "./FormDescriptor";

export default interface IInputDataType extends FormDescriptor {
  DataType: AspDataType;
}
