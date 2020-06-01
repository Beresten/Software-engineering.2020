using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puff.Objects
{
    static public class Physics
    {
        /// <summary> Гравитационная постоянная Земли. </summary>
        public const double g = 9.80665f;

        /// <summary> Ноль градусов Целсия. </summary>
        public const double T0 = 273;

        /// <summary> Стандартное атмосферное давление. </summary>
        public const double p0 = 101325;

        /// <summary> Молярная масса сухого воздуха кг/молль. </summary>
        public const double M_air = 0.02896;

        /// <summary> Универсальная газовая постоянная. </summary>
        public const double R = 8.31446261815324;

        /// <summary> Средняя удельная теплоёмкость воздуха при постоянном объёме Дж/(кг*К). </summary>
        public const double C_v_air = 717;

        /// <summary> Возвращает плотность газа. </summary>
        /// <param name="T"> Температура в Кельвинах. </param>
        /// <param name="p"> Плотность газа в г/молль. </param>
        /// <param name="M"> Молярная масса газа. </param>
        public static double GetGasDensity(double M, double T, double p)
        {
            return (M*p*T0)/(0.0224f*p0*T);
        }

        /// <summary> Возвращает аппроксимированное значение температуры воздуха в Кельвинах от высоты h от 0 до 80000 м. </summary>
        public static double GetAirTemperature(double h)
        {
            return 7.05512f * Math.Pow(10, -22) * Math.Pow(h, 5)
                 - 1.00425f * Math.Pow(10, -16) * Math.Pow(h, 4)
                 + 1.62846f * Math.Pow(10, -12) * Math.Pow(h, 3)
                 + 2.47058f * Math.Pow(10, -7) * Math.Pow(h, 2)
                 - 0.008821287f * h + 290.7826491f;
        }

        /// <summary> Возвращает атмосферное давление от высоты h. </summary>
        public static double GetAtmospherePressure(double h)
        {
            return p0 * Math.Exp((-1 * h * M_air * g) / (GetAirTemperature(h) * R));
        }

        /// <summary> Возвращает аппроксимированный коэффициент теплопередачи воздуха от температуры T от 170 до 300 К. </summary>
        public static double GetThermalConductivityOfAir(double T)
        {
            return 3.6452f * Math.Pow(10, -11) * Math.Pow(T, 4)
                 - 3.47338f * Math.Pow(10, -8) * Math.Pow(T, 3)
                 + 1.21746f * Math.Pow(10, -5) * Math.Pow(T, 2)
                 - 0.001773422f * T + 0.105376835;
        }

        /// <summary> Возвращает количество теплоты из основного уравнения теплообмена переданного за секунду. </summary>
        /// <param name="K"> Коэффициент теплопередачи вдоль поверхности теплообмена. </param>
        /// <param name="F"> Поверхность теплообмена. </param>
        /// <param name="T"> Разница темератур в Кельвинах. </param>
        public static double GetAmountOfHeat(double K, double F, double T)
        {
            return K * F * T;
        }
    }
}
