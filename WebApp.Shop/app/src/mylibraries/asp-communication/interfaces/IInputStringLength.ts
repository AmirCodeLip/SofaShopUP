import FormDescriptor from "./FormDescriptor";

export default interface InputStringLength extends FormDescriptor {
  ErrorMessage: string;
  MinimumLength: number;
  MaximumLength: number;
}
