import IFormModel from './../interfaces/IFormModel'
import IInputDisplay from './../interfaces/IInputDisplay'
import FormItem from './../interfaces/FormItem';
import IInputRequired from './../interfaces/IInputRequired';
import IInputDataType from './../interfaces/IInputDataType';
import InputStringLength from './../interfaces/IInputStringLength';
import { AspDataType } from "../EnumItems";

type userRefType = {
    <T>(initialValue: T): React.MutableRefObject<T>;
    <T>(initialValue: T): React.RefObject<T>;
    <T = undefined>(): React.MutableRefObject<T>;
};

export class FormHandler {
    constructor(...args: Array<FormModelItem>) {
        this.formModelItems = args;
    }

    formModelItems: Array<FormModelItem>;
    isInited: boolean = false;

    addError(key: string, error: string) {
        this.formModelItems.find(x => x.name === key)!.addError(error);
    }

    initRef(refInfo: userRefType) {
        this.formModelItems.forEach(formModelItem => formModelItem.initRef(refInfo));
    }

    init() {
        this.isInited = true;
        this.formModelItems.forEach(formModelItem => formModelItem.init());
    }

    isValid() {
        let valid = true;
        for (let formModelItem of this.formModelItems) {
            formModelItem.validate();
            if (!formModelItem.isValid) {
                valid = false;
                break;
            }
        }
        return valid;
    }

    setFormData(model: any): void {
        for (let name in model) {
            let item = this.formModelItems.find(x => x.name === name);
            if (item) {
                item.setValue(model[name]);
                console.log(item.getValue());
            }
            else {
                console.log(name);
            }
        }
    }

    getFormData<FormType>() {
        var model: any = {};
        for (let formModelItem of this.formModelItems) {
            switch (formModelItem.dataType) {
                case AspDataType.Text:
                    model[formModelItem.name] = formModelItem.getValue();
                    break;
            }
        }
        return model as FormType;
    }
}

abstract class FormModelItem {
    constructor(formModels: IFormModel, itemName: string) {
        this.name = itemName;
        this.formModel = formModels.find(model => model.Name === itemName)!;
        for (let formDescriptors of this.formModel.FormDescriptors) {
            switch (formDescriptors.ClassName) {
                case "InputDisplay":
                    let inputDisplay: IInputDisplay = <IInputDisplay>formDescriptors;
                    this.displayName = inputDisplay.Name;
                    break;
                case "InputRequired":
                    let inputRequired: IInputRequired = <IInputRequired>formDescriptors;
                    this.required = inputRequired.Required;
                    this.requiredErrorMsg = inputRequired.ErrorMessage;
                    break;
                case "InputDataType":
                    let inputDataType: IInputDataType = <IInputDataType>formDescriptors;
                    this.dataType = inputDataType.DataType;
                    break;
                case "InputStringLength":
                    let inputStringLength: InputStringLength = <InputStringLength>formDescriptors;

                    // validators.push(Validators.minLength(inputStringLength.MinimumLength));
                    // validators.push(Validators.maxLength(inputStringLength.MaximumLength));
                    // infoInput.MaxLength = inputStringLength.MaximumLength;
                    // infoInput.MinLength = inputStringLength.MinimumLength;
                    // infoInput.ErrorMsgs.push({ Key: "stringLength", Value: inputStringLength.ErrorMessage });
                    break;
                default:
                    console.log("please implement " + formDescriptors.ClassName);
            }
        }
    }
    name: string;
    formModel: FormItem;
    displayName: string | undefined;
    required: boolean;
    requiredErrorMsg: string;
    dataType: AspDataType | undefined;
    abstract isValid: boolean;
    abstract getValue(): any;
    abstract setValue(value: any): void;
    abstract validate(): void;
    abstract initRef(refInfo: userRefType): void;
    abstract init(): void;
    abstract addError(error: string): void;
}
export class HiddenModeInput<TType> extends FormModelItem {
    constructor(formModels: IFormModel, itemName: string) {
        super(formModels, itemName);
    }
    isValid: boolean;
    private value: TType;

    getValue() {
        return this.value;
    }

    setValue(value: TType | any) {
        this.value = value as TType;
    }

    validate(): void {
        this.isValid = true;
    }
    initRef(refInfo: userRefType): void { }
    init(): void {
        if (!this.dataType)
            this.dataType = AspDataType.Text;
    }
    addError(error: string): void { }

}

export class FormModeInput extends FormModelItem {
    constructor(formModels: IFormModel, itemName: string) {
        super(formModels, itemName);
    }

    private touched: boolean;
    get Touched(): boolean {
        return this.touched;
    }
    set Touched(value: boolean) {
        if (value)
            this.refLabel.current!.classList.add("fm-touched");
        else
            this.refLabel.current!.classList.remove("fm-touched");
        this.touched = value;
    }

    private haveValue: boolean;
    get HaveValue(): boolean {
        return this.haveValue;
    }
    set HaveValue(value: boolean) {
        if (value)
            this.refLabel.current!.classList.add("fm-have-val");
        else
            this.refLabel.current!.classList.remove("fm-have-val");
        this.haveValue = value;
    }

    refInput: React.RefObject<HTMLInputElement>;
    refError: React.RefObject<HTMLDivElement>;
    refLabel: React.RefObject<HTMLLabelElement>;
    errorList: Array<string>;
    isValid: boolean;

    initRef(refInfo: userRefType) {
        this.refError = refInfo<HTMLDivElement>();
        this.refInput = refInfo<HTMLInputElement>();
        this.refLabel = refInfo<HTMLLabelElement>();
    }

    init() {
        if (this.displayName) {
            this.refLabel.current!.textContent = this.displayName;
        }
        if (!this.dataType)
            this.dataType = AspDataType.Text;
        if (this.dataType) {
            switch (this.dataType) {
                case AspDataType.Password:
                    this.refInput.current!.type = "password";
                case AspDataType.Text:
                    this.refInput.current!.type = "text";
            }
        }
        this.refInput.current!.addEventListener("focus", () => {
            this.Touched = true;
        });
        this.refInput.current!.addEventListener("focusout", () => {
            this.Touched = false;
        });
        this.refLabel.current!.focus = () => {
            this.refInput.current!.focus();
        }
        this.refInput.current!.onkeyup = () => this.validate();
        this.HaveValue = this.refInput.current!.value.length !== 0;
    }

    addError(error: string) {
        this.isValid = false;
        let errorBox = document.createElement("span");
        errorBox.innerText = error;
        this.refError.current!.appendChild(errorBox);
    }

    getValue() {
        return this.refInput.current!.value;
    }

    setValue(value: any) {
        this.refInput.current.value = value;
    }

    validate() {
        let currentVal = this.refInput.current!.value;
        this.refError.current!.innerHTML = "";
        this.isValid = true;
        this.errorList = [];
        this.isValid = true;
        if (currentVal.length === 0) {
            this.HaveValue = false;
            if (this.required)
                this.errorList.push(this.requiredErrorMsg.replace("{0}", this.displayName!));
        }
        else {
            this.HaveValue = true;
        }
        if (this.errorList.length > 0) {
            this.addError(this.errorList[0]);
        }
    }
}