namespace API.Extensions
{
  public static class DateTimeExtensions
  {
    public static int CalculateAge(this DateTime dob)
    {
      var today = DateTime.UtcNow;

      var age = today.Year - dob.Year;

      // If the person is in the current year but their birthday is not there yet
      // --> Deduct their age by 1
      if (dob > today.AddYears(-age)) age--;

      return age;
    }
  }
}