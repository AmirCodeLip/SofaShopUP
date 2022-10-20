using System;
using System.Text.RegularExpressions;

namespace DataLayer.UnitOfWork
{
    public class SharedRegix
    {
        public const string suportedMails =
               "gmail|aol|outlook|zoho|yahoo|protonmail|icloud|gmx|mozilla|yandex";
        public const string email = @"^([a-zA-Z0-9_\-\.]+)@(" + suportedMails + ").(com|net|ir)";
        //public const string phone = @"[0-9]{10,11}";
        public const string SLError = " {0} " + "باید حداقل" + " {2} " + "وحداکثر" + " {1} " + "کاراکترباشد";
        public const string RequiredError = "لطفا فیلد" + " {0} " + "را وارد کنید";
        public const string EmialError = "لطفا آدرس ایمیل صحیح وارد کنید";
        public const string PhoneError = "لطفا تلفن همراه صحیح وارد کنید";
        public const string MaxLengthError = "{0}" + " باید کمتر از" + "{1}" + "کاراکتر باشد";
        public const string MinLengthError = "{0}" + " باید بیشتر از" + "{1}" + "کاراکتر باشد";
        public const string ExistFromPrevious = "{0}" + " مشابه از قبل وجود دارد ";

        public static string DuplicateUserNameError(string userName) => " نام کاربری با نام" + userName + "از قبل موجود هست.";
        const string hamraheAvalPhones = "0910|0911|0912|0913|0914|0915|0916|0917|0918|0919|0990|0991|0992|0993|0994|0903";
        const string iranTaliyaPhones = "0932";
        const string IrancellPhones = "0930|0933|0935|0936|0937|0938|0939|0900|0901|0902|0903|0904|0905|0941";
        const string RighTel = "0920|0921|0922";
        //const string hamraheAvalPhones = "0910|0911|0912|0913|0914|0915|0916|0917|0918|0919|0990|0991|0992|0993|0994|0903";
        public static string phone = @$"^({hamraheAvalPhones}|{iranTaliyaPhones}|{IrancellPhones}|{RighTel})[0-9]{{7,7}}";
        public static Regex RgEmail = new Regex(email);
        public static Regex RgPhone = new Regex(phone);
        public const string InvalidLoginAttempt = "تلاش برای ورود نامعتبر است.";
    }
}
