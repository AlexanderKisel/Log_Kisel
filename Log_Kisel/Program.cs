using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Log_Kisel
{
    internal class Program
    {
        static void Main(string[] args)
        {

            double a = 0;
            var prov = false;
            var curs = 100;
            var rub = 0d;
            double proc = 0.37, rubproc = 0;
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .Enrich.WithProperty("Курс доллара: ", curs)
                        .WriteTo.Seq("http://localhost:5341/", apiKey: "xqiG45dv2wlEJYPSvK4y")
                        .CreateLogger();
            while (!prov || a < 1)
            {
                Console.Write("Напишите сумму перевода из долларов в рубли: ");
                var dol = Console.ReadLine();
                prov = double.TryParse(dol, out a);
                if (!prov || a < 1)
                {
                    Log.Error("Пользователь ввел неверное значение!");
                }
            }
            Log.Information($"Пользователь ввел верное значение: {a}");
            rub = a * curs;
            if (a < 500)
            {
                Console.WriteLine("Сумма перевода меньше 500$ ");
                Console.WriteLine("Комиссия 8 р.");
                Console.WriteLine("Итог перевода:" + (rub - 8));
            }
            else
            {
                rubproc = rub * (proc / 100);
                Console.WriteLine("Сумма перевода больше 500$ ");
                Console.WriteLine($"Процент комиссии: {proc} % =  {rubproc}");
                Console.Write("Итог с комиссией:");
                rub = rub - rubproc;
                Console.WriteLine(rub);
            }
            Log.Information($"Пользователь получил: {rub}");
            Log.CloseAndFlush();
            Console.ReadKey();
        }
    }
}
