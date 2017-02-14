using System;

namespace GarageProject.Vehicles
{
    [Serializable]
    class Boat : Vehicle
    {
        public int Length { get; }

        public Boat(string regNr, string color, int numberOfWheels, int maxSpeed, int length) : base(regNr, color, numberOfWheels, maxSpeed)
        {
            Length = length;
        }

        public override string ToString()
        {
            return $"Parkingspace: {Index + 1} \n\t\t Type: Boat \n\t\t RegNumber: {RegNr} \n\t\t Color: {Color} \n\t\t Max speed: {MaxSpeed} \n\t\t Length: {Length}\n";
        }

        public override string ToStringLite()
        {
            return $"Parkingspace: {Index + 1} \n\t\t Type: Boat \n\t\t RegNumber: {RegNr}\n";
        }

        public override Type GetVehicleType()
        {
            return this.GetType();
        }
    }
}
