using System;

namespace GarageProject.Vehicles
{
    [Serializable]
    abstract class Vehicle
    {
        public string RegNr { get; }
        public string Color { get; }
        public int NumberOfWheels { get; }
        public int MaxSpeed { get; }
        private int index = -1;

        public int Index
        {
            get { return index; }
            set { if (index == -1) index = value; }
        }

        public Vehicle(string regNr, string color, int numberOfWheels, int maxSpeed)
        {
            this.RegNr = regNr;
            this.Color = color;
            this.MaxSpeed = maxSpeed;
            this.NumberOfWheels = numberOfWheels;
        }
        public abstract string ToStringLite();

        public abstract Type GetVehicleType();
    }
}
