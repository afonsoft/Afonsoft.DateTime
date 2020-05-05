using System;
using System.Collections.Generic;
using System.Linq;

namespace Afonsoft.Date.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// ForEach por dia
        /// <example>
        /// ForEach de uma data
        /// <code>
        /// DateTime dateTime = new DateTime(2017,1,1); 
        /// foreach(DateTime date in dateTime.EachDay(new DateTime(2017,1,30)))
        /// {
        ///  ... Code
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="from">Data Inicial</param>
        /// <param name="thru">Data final</param>
        /// <returns>Next Date</returns>
        public static IEnumerable<DateTime> EachDay(this DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        /// <summary>
        /// ForEach por hora
        /// <example>
        /// ForEach de uma data
        /// <code>
        /// DateTime dateTime = new DateTime(2017,1,1, 8,0,0); 
        /// foreach(DateTime date in dateTime.EachDay(new DateTime(2017,1,1,16,0,0)))
        /// {
        ///  ... Code
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="from">Data Inicial</param>
        /// <param name="thru">Data final</param>
        /// <returns>Next Date</returns>
        public static IEnumerable<DateTime> EachHours(this DateTime from, DateTime thru)
        {
            for (var day = from; day <= thru; day = day.AddHours(1))
                yield return day;
        }
        /// <summary>
        /// ForEach por mês
        /// <example>
        /// ForEach de uma data
        /// <code>
        /// DateTime dateTime = new DateTime(2017,1,1); 
        /// foreach(DateTime date in dateTime.EachMonth(new DateTime(2017,6,01)))
        /// {
        ///  ... Code
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="from">Data Inicial</param>
        /// <param name="thru">Data final</param>
        /// <returns>Next Month Date</returns>
        public static IEnumerable<DateTime> EachMonth(this DateTime from, DateTime thru)
        {
            for (var month = from.Date; month.Date <= thru.Date; month = month.AddMonths(1))
                yield return month;
        }

        /// <summary>
        /// Verifica se é um dia útil, se não é sabado e domigo e nem um feriado nacional brasileiro 
        /// </summary>
        public static bool IsWorkingDay(this DateTime from, params DateTime[] bankHolidays)
        {

            if (bankHolidays != null)
            {
                return from.DayOfWeek != DayOfWeek.Saturday
                       && from.DayOfWeek != DayOfWeek.Sunday
                       && !bankHolidays.Contains(from);
            }
            else
            {
                return from.DayOfWeek != DayOfWeek.Saturday
                      && from.DayOfWeek != DayOfWeek.Sunday;
            }
        }

       

        /// <summary>
        /// Verifica se é um final de semana (Sabado e Domingo)
        /// </summary>
        public static bool IsWeekEndDay(this DateTime from)
        {
            return from.DayOfWeek == DayOfWeek.Saturday
                   || from.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Retorna o proximo dia da semana selecionada
        /// <example>
        /// Hoje é Quarta-Feira e quero saber qual a data da proxima terça-feira
        /// <code>
        /// DateTime dateTime = new DateTime(2017,4,26); //Quarta-Feira (Wednesday)
        /// var date = dateTime.Next(Tuesday); //date = 2017-05-02
        /// </code>
        /// </example>
        /// </summary>
        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            /*
                   DayOfWeek
       Domingo         = Sunday    = 0, 
       Segunda-feira   = Monday    = 1, 
       Terça-feira     = Tuesday   = 2, 
       Quarta-feira    = Wednesday = 3, 
       Quinta-feira    = Thursday  = 4, 
       Sexta-feira     = Friday    = 5, 
       Sábado          = Saturday  = 6
       */
            int daysToAdd = ((int)dayOfWeek - (int)from.DayOfWeek + 7) % 7;
            return from.AddDays(daysToAdd);
        }

        /// <summary>
        /// Adicionar x dia útil em uma data.
        /// <example>
        /// Como utilizar o metodo
        /// <code>
        /// DateTime dateTime = new DateTime(2017,4,26); //Quarta-Feira (Wednesday)
        /// var date = dateTime.AddWorkDays(3); //date =  2017-05-02 (01/05 é feriado)
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="from">DateTime Inicial</param>
        /// <param name="workingDays">Quantidade de dias uteis</param>
        /// <param name="bankHolidays">Lista de feriados default holidays</param>
        /// <returns></returns>
        public static DateTime AddWorkDays(this DateTime from, int workingDays, params DateTime[] bankHolidays)
        {
            int direction = workingDays < 0 ? -1 : 1;

            DateTime newDate = from;
            // If a working day count of Zero is passed, return the date passed
            if (workingDays == 0)
            {
                newDate = from;
            }
            else
            {
                while (workingDays != -direction)
                {
                    if (newDate.DayOfWeek != DayOfWeek.Saturday &&
                        newDate.DayOfWeek != DayOfWeek.Sunday &&
                        Array.IndexOf(bankHolidays, newDate) < 0)
                    {
                        workingDays -= direction;
                    }
                    // if the original return date falls on a weekend or holiday, this will take it to the previous / next workday, but the "if" statement keeps it from going a day too far.

                    if (workingDays != -direction)
                    {
                        newDate = newDate.AddDays(direction);
                    }
                }
            }
            return newDate;
        }

        /// <summary>
        /// Calcula a quantidade de dias uteis entre um intervalo.
        /// </summary>
        /// <param name="firstDay">Data inicial</param>
        /// <param name="lastDay">Data Final</param>
        /// <param name="bankHolidays">Lista de Feiados</param>
        /// <returns>Numero de dias uteis</returns>
        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;

            int businessDays = span.Days + 1;

            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstDay.DayOfWeek;

                int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)lastDay.DayOfWeek;

                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;

                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }
    }
}
