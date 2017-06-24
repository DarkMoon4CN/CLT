using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChuangLitouP2P.Models;
namespace ChuanglitouP2P.DAL
{
    public class D_Holiday
    {
        chuangtouEntities ef = new chuangtouEntities();
        public int Add(Holiday model)
        {
            if (model != null)
            {
                ef.Holidays.Add(model);
                return ef.SaveChanges();
            }
            return 0;
        }

        public Holiday GetHolidayBounds(DateTime datetime)
        {
            DateTime temp = Convert.ToDateTime(datetime.ToString("yyyy-MM-dd"));
            var query = from data in ef.Holidays
                        where data.IsEnable == 1 && data.Starttime <= temp && data.Endtime >= temp
                        select data;
            return query.ToList().FirstOrDefault();
        }

        public List<Holiday> GetYears(string year)
        {
            var query = from data in ef.Holidays select data;
            if (!string.IsNullOrWhiteSpace(year))
            {
                int intYear = Convert.ToInt32(year);
                query = query.Where(q => q.Year == intYear);
            }
            return query.ToList();
        }

        public List<Holiday> GetYears(string year, int pageIndex, int pageSize = 15)
        {
            var query = from data in ef.Holidays select data;
            if (!string.IsNullOrWhiteSpace(year))
            {
                int intYear = Convert.ToInt32(year);
                query = query.Where(q => q.Year == intYear);
            }
            return query.Skip(pageIndex * pageSize).ToList();
        }

        public bool Delete(int id)
        {
            var query = from data in ef.Holidays where data.ID == id select data;
            var temp = query.ToList().FirstOrDefault();
            if (temp != null)
            {
                ef.Holidays.Remove(temp);
                ef.SaveChanges();
            }
            return true;
        }
        public bool Delete(List<Holiday> models)
        {
            var result = true;
            foreach (var item in models)
            {
                result = result && Delete(item.ID);
            }
            return result;
        }
        public bool Update(int id, Holiday model)
        {
            var query = from data in ef.Holidays where data.ID == id select data;
            var temp = query.ToList().FirstOrDefault();
            if (temp != null)
            {
                temp.Starttime = model.Starttime;
                temp.Endtime = model.Endtime;
                temp.Comments = model.Comments;
                temp.DuringDays = model.DuringDays;
                temp.IsEnable = model.IsEnable;
                ef.SaveChanges();
                return true;
            }
            return false;
        }
        public bool Update(List<Holiday> models)
        {
            var result = true;
            foreach (var item in models)
            {
                result = result && Update(item.ID, item);
            }
            return result;
        }
    }
}
