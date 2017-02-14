using System;

namespace GarageProject.Vehicles
{
    [Serializable]
    class Bus : Vehicle
    {
        public int NumberOfSeats { get; }

        public Bus(string regNr, string color, int numberOfWheels, int maxSpeed, int numberOfSeats) : base(regNr, color, numberOfWheels, maxSpeed)
        {
            NumberOfSeats = numberOfSeats;
        }

        public override string ToString()
        {
            return $"Parkingspace: {Index + 1} \n\t\t Type: Bus \n\t\t RegNumber: {RegNr} \n\t\t Color: {Color} \n\t\t Number of Wheels: {NumberOfWheels} \n\t\t Max speed: {MaxSpeed} \n\t\t Number of seats: {NumberOfSeats}\n";
        }

        public override string ToStringLite()
        {
            return $"Parkingspace: {Index + 1} \n\t\t Type: Bus \n\t\t RegNumber: {RegNr}\n";
        }

        public override Type GetVehicleType()
        {
            return this.GetType();
        }
    }
}
