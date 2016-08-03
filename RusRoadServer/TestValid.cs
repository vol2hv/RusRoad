using RusRoadLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace RusRoadServer
{
    class TestValid
    {
        public static void Test()
        {
            PassageSrc pas = new PassageSrc()
            {
                Govnumber = "С666СС48", Highway_Id="25"

            };

            PassageSrcValidator validator = new PassageSrcValidator();
            ValidationResult results = validator.Validate(pas);

            bool validationSucceeded = results.IsValid;
            if (validationSucceeded)
            {
                Console.WriteLine("Запись не содержит ошибок.");
            }
            else
            {
                IList<ValidationFailure> failures = results.Errors;
                Console.WriteLine(AllErrors(failures));
            }
            
        }
        static string AllErrors(IList<ValidationFailure> failures)
        {
            string str = "Ошибки при валидации\n";
            foreach (var item in failures)
            {
                str = str + item.ErrorMessage + "\n";
            }
            return str;
        }
    }
}
