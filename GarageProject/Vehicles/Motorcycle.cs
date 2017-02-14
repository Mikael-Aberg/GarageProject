using System;

namespace GarageProject.Vehicles
{
    [Serializable]
    class Motorcycle : Vehicle
    {
        public float CylinderVolume { get; }

        public Motorcycle(string regNr, string color, int numberOfWheels, int maxSpeed, float cylinderVolume) : base(regNr, color, numberOfWheels, maxSpeed)
        {
            CylinderVolume = cylinderVolume;
        }

        public override string ToString()
        {
            return $"Parkingspace: {Index + 1} \n\t\t Type: Motorcycle \n\t\t RegNumber: {RegNr} \n\t\t Color: {Color} \n\t\t Number of Wheels: {NumberOfWheels} \n\t\t Max speed: {MaxSpeed} \n\t\t Cylinder volume: {CylinderVolume}\n";
        }

        public override string ToStringLite()
        {
            return $"Parkingspace: {Index + 1} \n\t\t Type: Motorcycle \n\t\t RegNumber: {RegNr}\n";
        }

        public override Type GetVehicleType()
        {
            return this.GetType();
        }
    }
}
