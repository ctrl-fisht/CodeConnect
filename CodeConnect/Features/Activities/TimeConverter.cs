namespace CodeConnect.Features.Activities;

public static class TimeConverter
{
    // Статический метод перевода из локального времени и даты в UTC время и дату
    public static (DateOnly, TimeOnly) CalculateUtcFromLocal(DateOnly dateLocal, TimeOnly timeLocal, int offset)
    {
        var localHour = timeLocal.Hour;

        // локальное время + смещение часового пояса относительно UTC
        var hourSum = localHour - offset;

        // расчёт времени UTC = abs(hourSum) % 24 
        // берем hourSum по модулю
        // т.к. можем перейти на день назад (hourSum отрицательное)
        // или на день вперёд (hourSum положительно)

        var utcHour = Math.Abs(hourSum) % 24;
        var utcTime = new TimeOnly(hour: utcHour, minute: timeLocal.Minute);


        DateOnly utcDate = new DateOnly(year: dateLocal.Year, month: dateLocal.Month, day: dateLocal.Day);
        if (hourSum < 0)
            utcDate = utcDate.AddDays(-1);
        else if (hourSum >= 24)
            utcDate = utcDate.AddDays(1);

        return (utcDate, utcTime);

    }

    // функция, только уже для расчёта времени для пользователя из UTC
    public static (DateOnly, TimeOnly) CalculateUserTimeFromUtc(DateOnly dateUtc, TimeOnly timeUtc, int offset)
    {
        var utcHour = timeUtc.Hour;

        var hourSum = utcHour + offset;


        var userHour = Math.Abs(hourSum) % 24;
        var userTime = new TimeOnly(hour: userHour, minute: timeUtc.Minute);


        DateOnly userDate = new DateOnly(year: dateUtc.Year, month: dateUtc.Month, day: dateUtc.Day);
        if (hourSum < 0)
            userDate = userDate.AddDays(-1);
        else if (hourSum >= 24)
            userDate = userDate.AddDays(1);

        return (userDate, userTime);
    }
}
