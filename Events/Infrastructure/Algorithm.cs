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

        private DateTime? CheckCurrentDate(List<UserDates> usersDates, int skipIndex, Date currentDate, DateTime? beginDate = null)
        {
            DateTime? expectedDate = beginDate is null ? currentDate.DateStart : currentDate.DateEnd;
            int count = 0;//количество участников кому подойдет эта дата

            for (int i = 0; i < usersDates.Count; i++)
            {
                if (i == skipIndex)
                    continue;

                foreach (var date in usersDates[i].Dates)
                {
                    bool isFindBeginDate = beginDate is null;
                    if (isFindBeginDate)
                    {
                        bool isInRange = date.DateStart <= expectedDate && (date.DateEnd is null || date.DateEnd > expectedDate);
                        if (isInRange)
                        {
                            count++;
                            break;
                        }
                    }
                    else
                    {
                        bool isInRange = date.DateEnd <= expectedDate && beginDate < expectedDate && beginDate >= date.DateStart;
                        if (isInRange)
                        {
                            count++;
                            break;
                        }
                    }
                }
            }
            return count == usersDates.Count - 1 ? expectedDate : null;
        }
    }
}
