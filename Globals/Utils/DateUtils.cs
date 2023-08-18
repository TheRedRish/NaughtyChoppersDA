namespace NaughtyChoppersDA.Globals.Utils
{
    public static class DateUtils
    {
        public static int CalculateAge(DateOnly date)
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            int age = currentDate.Year - date.Year;

            if (currentDate.Month < date.Month || (currentDate.Month == date.Month && currentDate.Day < date.Day))
            {
                age--;
            }

            return age;
        }
    }
}
