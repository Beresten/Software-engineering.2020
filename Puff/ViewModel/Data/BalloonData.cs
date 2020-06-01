using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puff.VM.Data
{

    public class InitialConditions
    {
        /// <summary>
        /// Расходуемое топливо в мг/сек.
        /// </summary>
        public double FuelConsumption { get; set; }

        /// <summary>
        /// Удельная теплота сгорания в Дж/мг.
        /// </summary>
        public double ClorificValue { get; set; }

        /// <summary>
        /// Вес корзины аэростата в кг.
        /// </summary>
        public double BasketWeight { get; set; }

        /// <summary>
        /// Температура воздуха у земли в Кельвинах.
        /// </summary>
        public double GroundTemperature { get; set; }

        /// <summary>
        /// Высота запуска относительно уровня моря в метрах.
        /// </summary>
        public double SeaLevelHeight { get; set; }

        /// <summary>
        /// Объем аэростата в м^3.
        /// </summary>
        public double Volume { get; set; }

    }

    public class DynamicData
    {
        /// <summary>
        /// Расходуемое топливо в мг/сек.
        /// </summary>
        public double FuelConsumption { get; set; }
    }

    public class ProceduralData
    {
        /// <summary>
        /// Температура воздуха около аэростата в Кельвинах.
        /// </summary>
        public double AirTemperature { get; set; }

        /// <summary>
        /// Высота подъема аэростата в метрах.
        /// </summary>
        public double RelativeHeight { get; set; }

        /// <summary>
        /// Ускорение аэростата в метрах в секунду в квадрате.
        /// </summary>
        public double Acceleration { get; set; }

        /// <summary>
        /// Скорость аэростата в метрах в секунду.
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// Температура воздуха в аэростате в Кельвинах.
        /// </summary>
        public double Temperature { get; set; }
    }
}
