//
// Summary:
//     Represents an enumeration of the data types associated with data fields and parameters.
export enum AspDataType {
  //
  // Summary:
  //     Represents a custom data type.
  Custom,
  //
  // Summary:
  //     Represents an instant in time, expressed as a date and time of day.
  DateTime,
  //
  // Summary:
  //     Represents a date value.
  Date,
  //
  // Summary:
  //     Represents a time value.
  Time,
  //
  // Summary:
  //     Represents a continuous time during which an object exists.
  Duration,
  //
  // Summary:
  //     Represents a phone number value.
  PhoneNumber,
  //
  // Summary:
  //     Represents a currency value.
  Currency,
  //
  // Summary:
  //     Represents text that is displayed.
  Text,
  //
  // Summary:
  //     Represents an HTML file.
  Html,
  //
  // Summary:
  //     Represents multi-line text.
  MultilineText,
  //
  // Summary:
  //     Represents an email address.
  EmailAddress,
  //
  // Summary:
  //     Represent a password value.
  Password,
  //
  // Summary:
  //     Represents a URL value.
  Url,
  //
  // Summary:
  //     Represents a URL to an image.
  ImageUrl,
  //
  // Summary:
  //     Represents a credit card number.
  CreditCard,
  //
  // Summary:
  //     Represents a postal code.
  PostalCode,
  //
  // Summary:
  //     Represents file upload data type.
  Upload
}
