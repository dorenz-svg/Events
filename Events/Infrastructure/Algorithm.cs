using Events.Model;
using Events.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public (DateTime? dateBegin, DateTime? dateEnd) FindConvenientDate(IEnumerable<Visitors> visitors)
        {
            var beginDate = GetStartDate(visitors);
            var endDate = GetEndDate(visitors, beginDate);
            return (beginDate, endDate);
        }


        private DateTime? GetStartDate(IEnumerable<Visitors> visitors)
        {
            foreach (var user in visitors)
            {
                foreach (var date in user.Dates)
                {
                    var otherVisitors = visitors.Where(x => x.UserName != user.UserName).ToArray();
                    DateTime? result = null; 

                    result = CheckCurrentDate(otherVisitors, date.DateStart);

                    if (result is not null)
                        return result;
                }
            }

            return null;
        }


        private DateTime? GetEndDate(IEnumerable<Visitors> visitors, DateTime? beginDate)
        {
            foreach (var user in visitors)
            {
                foreach (var date in user.Dates)
                {
                    var otherVisitors = visitors.Where(x => x.UserName != user.UserName).ToArray();
                    DateTime? result = null; 

                    result = CheckCurrentDate(otherVisitors, date.DateEnd, beginDate);

                    if (result is not null)
                        return result;
                }
            }

            return null;
        }

        private DateTime? CheckCurrentDate(IEnumerable<Visitors> visitors, DateTime dateStart)
        {
            var countUsers = visitors
                .Sum(x => x.Dates
                    .Count(date => date.IsInRange(dateStart)));

            return countUsers == visitors.Count() ? dateStart : null;
        }

        private DateTime? CheckCurrentDate(IEnumerable<Visitors> visitors, DateTime? dateEnd, DateTime? beginDate)
        {
            var countUsers = visitors
                .Sum(x => x.Dates
                    .Count(date => date.IsInRange(beginDate, dateEnd)));

            return countUsers == visitors.Count() ? dateEnd : null;
        }
    }
}
