using Events.Model;
using Events.Models.Request;
using System;
using System.Collections.Generic;

namespace Events.Infrastructure
{

    /*
    1) вынести логику из репозиториев
    2) разнести все по слоям (по проектам)
    3) jwt generator сделать как middleware
    4) покрыть код юнит тестами
    */
    public class Algorithm : IAlgorithm
    {
        /// <summary>
        /// алгоритм пересечения диапозонов дат
        /// </summary>
        /// <param name="dates"></param>
        /// <returns>возвращает либо диапозон дат либо фиксированную дату</returns>
        public (DateTime? dateBegin, DateTime? dateEnd) FindConvenientDate(List<UserDates> usersDates)
        {
            return GetDate(usersDates);
        }

        private (DateTime? dateBegin, DateTime? dateEnd) GetDate(List<UserDates> usersDates)
        {
            var beginDate = GetStartDate(usersDates);
            var endDate = GetEndDate(usersDates, beginDate);
            return (beginDate, endDate);
        }

        private DateTime? GetStartDate(List<UserDates> usersDates)
        {
            foreach (var user in usersDates)
            {
                foreach (var date in user.Dates)
                {
                    var visitors = usersDates.Where(x => x.Name != user.Name).ToArray();
                    DateTime? result = null; 

                    result = CheckCurrentDate(visitors, date.DateStart);

                    if (result is not null)
                        return result;
                }
            }

            return null;
        }


        private DateTime? GetEndDate(List<UserDates> usersDates, DateTime beginDate)
        {
            foreach (var user in usersDates)
            {
                foreach (var date in user.Dates)
                {
                    var visitors = usersDates.Where(x => x.Name != user.Name).ToArray();
                    DateTime? result = null; 

                    result = CheckCurrentDate(visitors, date.DateEnd, beginDate);

                    if (result is not null)
                        return result;
                }
            }

            return null;
        }

        // private DateTime? GetTime(List<UserDates> usersDates, DateTime? beginDate = null)
        // {
        //     foreach (var user in usersDates)
        //     {
        //         foreach (var date in user.Dates)
        //         {
        //             var visitors = usersDates.Where(x => x.Name != user.Name).ToArray();
        //             DateTime? result = null; 


        //             if (beginDate is null)
        //             {
        //                 result = CheckCurrentDate(visitors, date.DateStart);
        //             }
        //             else
        //             {
        //                 result = CheckCurrentDate(visitors, date.DateEnd, beginDate);
        //             }


        //             if (result is not null)
        //                 return result;
        //         }
        //     }
        //     return null;
        // }

        private DateTime? CheckCurrentDate(List<UserDates> visitors, DateTime dateStart)
        {
            var countUsers = visitors
                .Sum(x => x.Dates
                    .Count(date => date.IsInRange(dateStart));

            return countUsers == usersDates.Count - 1 ? dateStart : null;
        }

        private DateTime? CheckCurrentDate(List<UserDates> usersDates, DateTime dateEnd, DateTime beginDate)
        {
            var countUsers = visitors
                .Sum(x => x.Dates
                    .Count(date => date.IsInRange(dateStart, dateEnd));

            return countUsers == usersDates.Count - 1 ? dateEnd : null;
        }
    }
}
