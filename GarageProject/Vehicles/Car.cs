using System;

namespace GarageProject.Vehicles
{
    [Serializable]
    class Car : Vehicle
    {
        public FuelType fuelType { get; }

        public Car(string regNr, string color, int numberOfWheels, int maxSpeed, FuelType fuelType) : base(regNr, color, numberOfWheels, maxSpeed)
        {
            this.fuelType = fuelType;
        }

        public override string ToString()
        {
            return $"Parkingspace: {Index + 1} \n\t\t Type: Car \n\t\t RegNumber: {RegNr} \n\t\t Color: {Color} \n\t\t Number of Wheels: {NumberOfWheels} \n\t\t Max speed: {MaxSpeed} \n\t\t Fuletype: {((FuelType)fuelType).ToString()}\n";
        }

        public override string ToStringLite()
        {
            return $"Parkingspace: {Index + 1} \n\t\t Type: Car \n\t\t RegNumber: {RegNr}\n";
        }

        public override Type GetVehicleType()
        {
            return this.GetType();
        }
    }
}
