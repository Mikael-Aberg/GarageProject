using GarageProject.Vehicles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GarageProject
{
    class Garage<T> : IEnumerable<T> where T : Vehicle
    {
        private Vehicle[] vechicles;
        public int MaxCapacity { get; }
        public int Count { get; private set; }

        public Garage(int maxCapacity, Vehicle[] vehicleList = null)
        {
            // either loads the last used garage or creates a new
            if (vehicleList != null)
            {
                vechicles = vehicleList;
                MaxCapacity = vechicles.Length;
                CountItems();
            }
            else
            {
                MaxCapacity = maxCapacity;
                vechicles = new Vehicle[maxCapacity];
            }
        }

        private void CountItems()
        {
            // counts all the vehicles after loading a garage
            foreach(var v in vechicles)
            {
                if(v != null)
                {
                    Count++;
                }
            }
        }

        public Vehicle[] GetVehicles() => vechicles.ToArray();

        public Vehicle Remove(int index)
        {
            // checks all occupied places to find the one with the right index to remove
            var count = 0;
            for (int i = 0; i < MaxCapacity; i++)
            {
                if (vechicles[i] != null)
                {
                    if (count == index)
                    {
                        Vehicle v = vechicles[i];
                        vechicles[i] = null;
                        Count--;
                        return v;
                    }
                    count++;
                }
            }
            return null;
        }

        public bool Add(Vehicle vehicle)
        {
            if (Count >= MaxCapacity) return false;

            // finds the first empty spot and adds the new vehicle
            for (int i = 0; i < MaxCapacity; i++)
            {
                if (vechicles[i] == null)
                {
                    vehicle.Index = i;
                    vechicles[i] = vehicle;
                    Count++;
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            // returns all vehicles in the garage that is not null
            for (int i = 0; i < MaxCapacity; i++)
            {
                var v = vechicles[i];
                if (v != null)
                {
                    yield return v as T;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Vehicle SearchWithRegNumber(string regNr)
        {
            // if there are vehicles in the garage we search for the regNr else just return null
            if (Count > 0)
            {
                return vechicles.FirstOrDefault(v => v.RegNr == regNr);
            }
            else
            {
                return null;
            }
        }
    }
}