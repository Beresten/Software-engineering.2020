using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puff.Objects
{
    public class Balloon
    {
        public double Height { get; set; }
        public double AirTemperature { get; set; }
        public double Temperature { get; set; }
        public double SurfaceArea { get; set; }
        public double Acceleration { get; set; }
        public double BasketWeight { get; set; }
        public double Volume { get; set; }
        public double Speed { get; set; }
        public double ClorificValue { get; set; }
        public double FuelConsumption { get; set; }

        /// <summary>
        /// Моделирует одну сотую секунду полета.
        /// </summary>
        public void Simulate()
        {
            // Атмосферное давление
            double p = Physics.GetAtmospherePressure(Height);
            // Температура воздуха
            AirTemperature = Physics.GetAirTemperature(Height);
            // Плотность воздуха
            double d_air = Physics.GetGasDensity(Physics.M_air, AirTemperature, p);

            // Коэфф. теплопередачи
            double K = 6;//Physics.GetThermalConductivityOfAir(AirTemperature);
            // Сообщенная воздухом теплота
            double Q_air = Physics.GetAmountOfHeat(K, SurfaceArea, AirTemperature - Temperature) * 0.01f;
            // Сообщенная горелкой теплота
            double Q_heather = ClorificValue * FuelConsumption * 0.3f * 0.01f;
            // Плотность воздуха в аэростате
            double d_balloon = Physics.GetGasDensity(Physics.M_air, Temperature, p);
            // Температура аэростата
            Temperature += (Q_heather + Q_air) / (Physics.C_v_air * Volume * d_balloon);

            //Acceleration = (d_air * Physics.g) / (d_balloon + (BasketWeight / Volume)) - Physics.g;
            double Fa = d_air * Physics.g * Volume;
            double Fg = (BasketWeight + Volume * d_balloon) * Physics.g;
            Acceleration = (Fa - Fg) / (BasketWeight + d_balloon * Volume);
            //Debug.WriteLine("==================");
            ////Debug.WriteLine($"Атмосферное давление: {p}");
            //Debug.WriteLine($"Плотность воздуха: {d_air}");
            //Debug.WriteLine($"Плотность аэростата: {d_balloon}");
            ////Debug.WriteLine($"Температура аэростата: {Temperature}");
            //Debug.WriteLine($"Сила Архимеда: {Fa}");
            //Debug.WriteLine($"Тепло воздуха: {Q_air}");
            //Debug.WriteLine($"Тепло горелки: {Q_heather}");
            ////Debug.WriteLine($"Ускорение: {Acceleration}");

            Height += Speed * 0.01f + Acceleration * 0.0001f / 2;
            Speed += Acceleration * 0.01f;
        }
    }
}
