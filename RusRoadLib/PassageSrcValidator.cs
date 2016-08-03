using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
namespace RusRoadLib
{


    public class PassageSrcValidator : AbstractValidator<PassageSrc>
    {
        public PassageSrcValidator()
        {
            //int n;
            RuleFor(p => p.Govnumber).NotEmpty().Must(IsInDB);
            RuleFor(p => p.Highway_Id).NotEmpty().Must(IsNumber).Must(IsInDBHighway);
            
            //RuleFor(p => p.Govnumber).NotEmpty().Must(IsInDB2);
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
        private bool IsInDB(string govnum)
        {
            bool res = false;
            using (RusRoadsData db = new RusRoadsData())
            {
                var cars = from c in db.CarOwner
                           where c.Govnumber == govnum
                           select new { c.CarOwner_Id };
                if (cars.Count() > 0)
                {
                    res = true;
                }
            }
            return res;
        }
        private bool IsInDBHighway(string highway)
        {
            bool res = false;
            using (RusRoadsData db = new RusRoadsData())
            {
                int n = int.Parse(highway);
                var h = from c in db.Highway
                           where c.Highway_Id == n
                           select new {c.Highway_Id };
                if (h.Count() > 0)
                {
                    res = true;
                }
            }
            return res;
        }
        private bool IsNumber(string field)
        {
            int n;
            return int.TryParse(field, out n);
        }
    }
}

