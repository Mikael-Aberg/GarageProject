using System;

namespace GarageProject.Vehicles
{
    [Serializable]
    class Airplane : Vehicle
    {
        public int NumberOfEngines { get; }

        public Airplane(string regNr, string color, int numberOfWheels, int maxSpeed, int numberOfEngines) : base(regNr, color, numberOfWheels, maxSpeed)
        {
            this.NumberOfEngines = numberOfEngines;
        }

        public override string ToString()
        {
            return $"Parkingspace: {Index + 1} \n\t\t Type: Airplane \n\t\t RegNumber: {RegNr} \n\t\t Color: {Color} \n\t\t Number of Wheels: {NumberOfWheels} \n\t\t Max speed: {MaxSpeed} \n\t\t Number of engines: {NumberOfEngines}\n";
        }

        public override string ToStringLite()
        {
            return $"Parkingspace: {Index + 1} \n\t\t Type: Airplane \n\t\t RegNumber: {RegNr}\n";
        }

        public override Type GetVehicleType()
        {
            return this.GetType();
        }
    }
}
