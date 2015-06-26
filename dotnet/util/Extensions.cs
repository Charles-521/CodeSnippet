    public static class Extensions
    {
        public static DateTime ToDateTime(this long jsTimestamp)
        {
            var strTimestamp = jsTimestamp.ToString();
            var utcDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            utcDate = strTimestamp.Length <= 10 ? utcDate.AddSeconds(double.Parse(strTimestamp)) : utcDate.AddMilliseconds(double.Parse(strTimestamp));
            return utcDate.ToLocalTime();
        }
		
		public static IEnumerable<T> AsNotNull<T>(this IEnumerable<T> src)
        {
            if (src == null)
                return Enumerable.Empty<T>();
            return src;
        }
		
		public static string ToMessage(this DbEntityValidationException e)
        {
            var sb = new StringBuilder();
            foreach (var eve in e.EntityValidationErrors)
            {
                sb.AppendLine(
                    string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                foreach (var ve in eve.ValidationErrors)
                {
                    sb.AppendLine();
                    sb.Append(string.Format("    - Property: \"{0}\", Error: \"{1}\"",
                        ve.PropertyName, ve.ErrorMessage));
                }
            }

            return sb.ToString();
        }
		
		public static bool ToBoolean(this string str)
        {
            return bool.Parse(str);
        }
		
		public static long ToJsTimestamp(this DateTime dateTime)
        {
            return (long)dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        public static long ToJsTimestamp(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return 0;
            return (long)dateTime.Value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
		
		public static bool IsNullOrEmpty(this Guid? data)
        {
            if (data == null || data == Guid.Empty)
                return true;
            return false;
        }
    }