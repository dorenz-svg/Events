using System;
using System.Collections.Generic;

namespace Events.Infrastructure
{
    public class Algorithm : IAlgorithm
    {
        /// <summary>
        /// алгоритм пересечения дат
        /// </summary>
        /// <param name="dates"></param>
        /// <returns>возвращает либо диапозон дат либо фиксированную дату</returns>
        public (DateTime?, DateTime?) GetDate(List<Dictionary<DateTime, DateTime?>> dates)
        {
            var beginDate = GetTime(dates);
            var endDate = GetTime(dates, beginDate);
            return (beginDate, endDate);
        }
        private DateTime? GetTime(List<Dictionary<DateTime, DateTime?>> dates, DateTime? beginDate = null)
        {
            DateTime? result = null;
            for (int i = 0; i < dates.Count; i++)
            {
                foreach (var d in dates[i])
                {
                    DateTime? temp = beginDate is null ? d.Key : d.Value;
                    int count = 0;
                    for (int k = 0; k < dates.Count; k++)
                    {
                        if (k == i)
                            continue;
                        foreach (var item in dates[k])
                        {
                            if (beginDate is null)
                            {
                                if (item.Key <= temp && item.Value is null || item.Value > temp)
                                {
                                    count++;
                                    break;
                                }
                            }
                            else
                            {
                                if (item.Value <= temp && beginDate < temp && beginDate >= item.Key)
                                {
                                    count++;
                                    break;
                                }
                            }
                        }
                    }
                    if (count == dates.Count - 1)
                        return temp;
                }
            }
            return result;
        }
    }
}
