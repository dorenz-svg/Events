using Events.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Infrastructure
{
    public class Algorithm : IAlgorithm
    {
        public (DateTime?,DateTime?) GetDate(List<Dictionary<DateTime, DateTime?>> dates)
        {
            var beginDate = GetTime(dates, true);
            var endDate = GetTime(dates, true,beginDate);
            return (beginDate,endDate);
        }
        private DateTime? GetTime(List<Dictionary<DateTime, DateTime?>> dates, bool isBeginDate, DateTime? beginDate = null)
        {
            DateTime? result = null;
            for (int i = 0; i < dates.Count; i++)
            {
                foreach (var d in dates[i])
                {
                    DateTime? temp = isBeginDate ? d.Key : d.Value;
                    int count = 0;
                    for (int k = 0; k < dates.Count && k != i; k++)
                    {
                        foreach (var item in dates[k])
                        {
                            if (isBeginDate)
                            {
                                if (item.Key <= temp && item.Value >= temp)
                                {
                                    count++;
                                    break;
                                }
                            }
                            else
                            {
                                if(item.Key <= temp && item.Value >= temp && beginDate > temp)
                                {
                                    count++;
                                    break;
                                }
                            }
                        }
                    }
                    if (count == dates.Count - 1)
                        return result;
                }
            }
            return result;
        }
    }
}
