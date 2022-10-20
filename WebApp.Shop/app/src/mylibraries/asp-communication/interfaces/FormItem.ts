import FormDescriptor from "./FormDescriptor";

export default interface FormItem {
  InputType: "Text";
  Name: string;
  FormDescriptors: Array<FormDescriptor>
}
