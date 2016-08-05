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
            RuleFor(p => p.Govnumber).NotEmpty().Must(IsInDB).WithMessage("Ошибка в поле Госномер");
            RuleFor(p => p.Highway_Id).NotEmpty().Must(CheckHighway).WithMessage("Ошибка в поле идентификатор дороги");
            RuleFor(p => p.Time).NotEmpty().Must(CheckTime).WithMessage("Ошибка в поле Время проезда");
            RuleFor(p => p.Speed).NotEmpty().Must(CheckSpeed).WithMessage("Ошибка в поле Скорость проезда");

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
                    // не хорошо использовать глобальные переменные
                    // но дважды вычислять иx при проверке и заполнении еще хуже
                    RusRoadSettings.CarOwner_Id = cars.ToList()[0].CarOwner_Id;
                }
            }
            return res;
        }
        private bool CheckHighway(string highway)
        {
            int n;
         // экономим одно обращение к БД для каждой записи проезда
         // об отсутствии дороги в справочнике скажет исключение            

            //bool res = false;
            //if (int.TryParse(highway, out n))
            //{
            //    using (RusRoadsData db = new RusRoadsData())
            //    {
            //        var h = from c in db.Highway
            //                where c.Highway_Id == n
            //                select new { c.Highway_Id };
            //        if (h.Count() > 0)
            //        {
            //            res = true;

            //        }
            //    }

            //}
            return int.TryParse(highway, out n) ? true : false;
        }
        private bool CheckTime(string time)
        {
            DateTime result = new DateTime();
            return DateTime.TryParse(time, out result );
        }
        private bool CheckSpeed(string speed)
        {
            bool res = false;
            int n;
            if (int.TryParse(speed, out n))
            {
                if (n>10 && n<350)
                {
                    res = true;

                }
            }
            return res;
        }

    }
}

