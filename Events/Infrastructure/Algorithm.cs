using Events.Model;
using Events.Models.Request;
using System;
using System.Collections.Generic;

namespace Events.Infrastructure
{
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
            var beginDate = GetTime(usersDates);
            var endDate = GetTime(usersDates, beginDate);
            return (beginDate, endDate);
        }

        private DateTime? GetTime(List<UserDates> usersDates, DateTime? beginDate = null)
        {
            for (int i = 0; i < usersDates.Count; i++)
            {
                foreach (var date in usersDates[i].Dates)
                {
                    var result = CheckCurrentDate(usersDates, i, date, beginDate);
                    if (result is not null)
                        return result;
                }
            }
            return null;
        }

        private DateTime? CheckCurrentDate(List<UserDates> usersDates, int currentIndex, Date currentDate, DateTime? beginDate = null)
        {
            DateTime? temp = beginDate is null ? currentDate.DateStart : currentDate.DateEnd;
            int count = 0;
            for (int i = 0; i < usersDates.Count; i++)
            {
                if (i == currentIndex)
                    continue;
                foreach (var date in usersDates[i].Dates)
                {
                    if (beginDate is null)
                    {
                        if (date.DateStart <= temp && (date.DateEnd is null || date.DateEnd > temp))
                        {
                            count++;
                            break;
                        }
                    }
                    else
                    {
                        if (date.DateEnd <= temp && beginDate < temp && beginDate >= date.DateStart)
                        {
                            count++;
                            break;
                        }
                    }
                }
            }
            return count == usersDates.Count - 1 ? temp : null;
        }
    }
}
