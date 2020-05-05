using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Afonsoft.Date.Extensions;

namespace Afonsoft.Holiday.Extensions
{
    public static class HolidayExtensions
    {
        /// <summary>
        /// Lista de feriados nacionais brasileiros (PT-BR)
        /// </summary>
        /// <returns>Retorna um array de feriados nacionais</returns>
        public static DateTime[] Holidays(this DateTime from)
        {
            List<DateTime> arrayOfDate = new List<DateTime>();
            int year = from.Year;
            arrayOfDate.Add(DateTime.ParseExact("01/01/" + year, "dd/MM/yyyy", new CultureInfo("pt-BR")));
            arrayOfDate.Add(DateTime.ParseExact("21/04/" + year, "dd/MM/yyyy", new CultureInfo("pt-BR")));
            arrayOfDate.Add(DateTime.ParseExact("01/05/" + year, "dd/MM/yyyy", new CultureInfo("pt-BR")));
            arrayOfDate.Add(DateTime.ParseExact("07/09/" + year, "dd/MM/yyyy", new CultureInfo("pt-BR")));
            arrayOfDate.Add(DateTime.ParseExact("12/10/" + year, "dd/MM/yyyy", new CultureInfo("pt-BR")));
            arrayOfDate.Add(DateTime.ParseExact("02/11/" + year, "dd/MM/yyyy", new CultureInfo("pt-BR")));
            arrayOfDate.Add(DateTime.ParseExact("15/11/" + year, "dd/MM/yyyy", new CultureInfo("pt-BR")));
            arrayOfDate.Add(DateTime.ParseExact("25/12/" + year, "dd/MM/yyyy", new CultureInfo("pt-BR")));
            DateTime easterDay = CalculateEaster(year);
            arrayOfDate.Add(easterDay); //Domingo de Pascoa (Easter Day)
            arrayOfDate.Add(easterDay.AddDays(-47)); //The Carnival falls always 47 days before the Easter. Terça-feira de carnaval
            arrayOfDate.Add(easterDay.AddDays(-2)); //Paixão de Cristo
            arrayOfDate.Add(easterDay.AddDays(+60)); //The Corpus Christi falls always 60 days after the Easter.
            return arrayOfDate.ToArray();
        }

        /// <summary>
        /// Metodo para Calcular o Domingo de Pascoa.
        /// </summary>
        /// <param name="from">DateTime para pegar o ano</param>
        /// <returns>DateTime com a data da Pascoa</returns>
        public static DateTime EasterDay(this DateTime from)
        {
            return EasterDay(from.Year);
        }

        /// <summary>
        /// Metodo para Calcular o Domingo de Pascoa.
        /// </summary>
        /// <param name="year">O ano que será retornado a pascoa</param>
        /// <returns>DateTime com a data da Pascoa</returns>
        public static DateTime EasterDay(int year)
        {
            return CalculateEaster(year);
        }

        /// <summary>
        /// Verifica se a data é um feriado
        /// </summary>
        public static bool IsHoliday(this DateTime from)
        {
            return Holidays(from).Any(x => x.Year == @from.Year && x.Month == @from.Month && x.Day == @from.Day);
        }

        #region CalculateEaster 
        /// <summary>
        /// FUNÇÃO PARA CALCULAR A DATA DO DOMINGO DE PASCOA
        /// http://www.inf.ufrgs.br/~cabral/Pascoa.html
        /// </summary>
        /// <param name="year">ANO QUALQUER</param>
        /// <returns>DateTime</returns>
        private static DateTime CalculateEaster(int year)
        {
            int x = 24;
            int y = 5;

            if (year >= 1582 && year <= 1699)
            {
                x = 22;
                y = 2;
            }
            else if (year >= 1700 && year <= 1799)
            {
                x = 23;
                y = 3;
            }
            else if (year >= 1800 && year <= 1899)
            {
                x = 24;
                y = 4;
            }
            else if (year >= 1900 && year <= 2099)
            {
                x = 24;
                y = 5;
            }
            else if (year >= 2100 && year <= 2199)
            {
                x = 24;
                y = 6;
            }
            else if (year >= 2200 && year <= 2299)
            {
                x = 25;
                y = 7;
            }

            int a = year % 19;
            int b = year % 4;
            int c = year % 7;
            int d = (19 * a + x) % 30;
            int e = (2 * b + 4 * c + 6 * d + y) % 7;
            int day;
            int month;

            if (d + e > 9)
            {
                day = (d + e - 9);
                month = 4;
            }
            else
            {
                day = (d + e + 22);
                month = 3;
            }
            return new DateTime(year, month, day);
        }
        #endregion

        public static bool IsWorkingDay(this DateTime from)
        {
            return from.IsWorkingDay(from.Holidays());
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
        /// <returns></returns>
        public static DateTime AddWorkDays(this DateTime from, int workingDays)
        {
            return from.AddWorkDays(workingDays, from.Holidays());
        }


        /// <summary>
        /// Calcula a quantidade de dias uteis entre um intervalo.
        /// </summary>
        /// <param name="firstDay">Data inicial</param>
        /// <param name="lastDay">Data Final</param>
        /// <returns>Numero de dias uteis</returns>
        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay)
        {
            return firstDay.BusinessDaysUntil(lastDay, firstDay.Holidays());
        }
    }
}
