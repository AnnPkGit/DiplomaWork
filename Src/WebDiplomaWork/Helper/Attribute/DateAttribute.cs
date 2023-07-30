using System.ComponentModel.DataAnnotations;

namespace WebDiplomaWork.Helper.Attribute
{
    public class DateAttribute : RangeAttribute
    {
        public DateAttribute()
          : base(typeof(DateTime),
                  DateTime.UtcNow.AddYears(-106).ToShortDateString(),
                  DateTime.UtcNow.ToShortDateString())
        { }
    }
}
