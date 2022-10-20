import FormDescriptor from "./FormDescriptor";

export default interface IInputRequired extends FormDescriptor {
  Required: boolean;
  ErrorMessage: string;
}
