using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace _Validator
{
    class Program
    {
        static void Main(string[] args)
        {
            Passage passage = new Passage()
            {
                Passage_Id = 1,
                Time = DateTime.Now,
                CarOwner_Id = 1,
                Highway_Id = 1,
               
            };
            
            PassageValidator validator = new PassageValidator();
            ValidationResult results = validator.Validate(passage);

            bool validationSucceeded = results.IsValid;
            IList<ValidationFailure> failures = results.Errors;
        }
    }
    public class PassageValidator : AbstractValidator<Passage>
    {
        public PassageValidator()
        {
            RuleFor(p =>p.Speed).NotEmpty();
            //RuleFor(customer => customer.Forename).NotEmpty().WithMessage("Please specify a first name");
            //RuleFor(customer => customer.Discount).NotEqual(0).When(customer => customer.HasDiscount);
            //RuleFor(customer => customer.Address).Length(20, 250);
            //RuleFor(customer => customer.Postcode).Must(BeAValidPostcode).WithMessage("Please specify a valid postcode");
        }

        private bool BeAValidPostcode(string postcode)
        {
            // custom postcode validating logic goes here
            return true;
        }
    }
}

