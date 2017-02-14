using System;
using GarageProject.Vehicles;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GarageProject
{
    class GarageHandler
    {
        private Garage<Vehicle> garage;
        private Vehicle[] searchResults;
        private bool isRecountRequired = true;
        private string countResults = "";

        public GarageHandler(int maxCapacity, bool load = false)
        {
            garage = (load)? new Garage<Vehicle>(maxCapacity, LoadGarage()) : new Garage<Vehicle>(maxCapacity, null);
            searchResults = new Vehicle[garage.MaxCapacity];
            //PopulateGarage();
        }

        public int GetCount() => garage.Count();

        public bool CanAddNewVehicle() => garage.Count < garage.MaxCapacity;

        public string CountVehicels() => CountVehicles();

        private void PopulateGarage()
        {
            // just random data to test on
            garage.Add(new Car("123456", "1", 1, 1, FuelType.Gasoline));
            garage.Add(new Bus("123456", "1", 1, 1, 1));
            garage.Add(new Motorcycle("123456", "1", 1, 1, 1));
            garage.Add(new Airplane("123456", "1", 1, 1, 1));
            garage.Add(new Boat("123456", "1", 1, 1, 1));
            garage.Add(new Car("123456", "2", 1, 1, FuelType.Gasoline));
            garage.Add(new Bus("123456", "2", 1, 1, 1));
            garage.Add(new Motorcycle("123456", "2", 1, 1, 1));
            garage.Add(new Airplane("123456", "2", 1, 1, 1));
            garage.Add(new Boat("123456", "2", 1, 1, 1));
            garage.Add(new Car("123456", "1", 2, 1, FuelType.Gasoline));
            garage.Add(new Bus("123456", "1", 2, 1, 1));
            garage.Add(new Motorcycle("123456", "1", 2, 1, 1));
            garage.Add(new Airplane("123456", "1", 2, 1, 1));
            garage.Add(new Boat("123456", "1", 2, 1, 1));
        }

        public void AddNewVehicle(Vehicle vehicle)
        {
            if(vehicle != null) garage.Add(vehicle);
        }

        public Vehicle[] LoadGarage()
        {
            // loads the vehicle list from the saved .bin file
            string serializationFile = Path.Combine(Directory.GetCurrentDirectory(), "vehicles.bin");

            using (Stream stream = File.Open(serializationFile, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (Vehicle[])bformatter.Deserialize(stream);
            }
        }

        public void SaveGarage()
        {
            // saves the vehicle list to a .bin file
            string serializationFile = Path.Combine(Directory.GetCurrentDirectory(), "vehicles.bin");

            using (Stream stream = File.Open(serializationFile, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bformatter.Serialize(stream, garage);
            }
        }
 
        public string GetVehicleInformationMenu(bool[]showMoreInfo, out int selectCount)
        {
            selectCount = 1;
            StringBuilder builder = new StringBuilder();
            builder.Append("Selection\tInfo \n\n");
            // lists all the vehicles in the garage
            foreach (var v in garage)
            {
                if (v != null)
                {
                    builder.Append($"  {selectCount}.\t\t");
                    builder.Append((showMoreInfo[selectCount - 1]) ? v.ToString() : v.ToStringLite());
                    builder.Append("\n");
                    selectCount++;
                }
            }

            builder.Append("\nPlease choose the vehicle you want to remove or 0 to return to main menu");
            builder.Append("\nTo show more information about a vehicle enter \"+\" before the number  ex: \"+1\"");
            builder.Append("\nTo show less information about a vehicle enter \"-\" before the number  ex: \"-1\"");

            return builder.ToString();
        }

        public void RemoveVehicle(int index) => garage.Remove(index);

        public string ListVehicles()
        {
            // lits all the vehicles and all the information about them
            StringBuilder builder = new StringBuilder();

            foreach (var v in garage)
            {
                builder.Append("\t" + v.ToString());
                builder.Append("\n");
            }
            if (builder.Length == 0) builder.Append("There are no vehicles in the garage right now");

            return builder.ToString();
        }

        public string SearchWithRegNumber(string regNr)
        {
            var vehicle = garage.SearchWithRegNumber(regNr);

            // if we found a matching vehicle we print it out
            if (vehicle != null)
            {
                return vehicle.ToString();
            }
            else
            {
                return "Could not find a vehicle with that registration number";
            }
        }

        public string ListVehicleTypes(VehicleTypes type)
        {
            // lists all vehicles of a single type
            StringBuilder builder = new StringBuilder();
            var typeName = ((VehicleTypes)type).ToString();
            foreach (var v in garage)
            {
                if (v.GetType().Name == typeName)
                {
                    builder.Append(v.ToString());
                    builder.Append("\n");
                }
            }

            if (builder.Length == 0 && typeName == "Bus") builder.Append($"There are no {typeName.ToLower()}es in the garage right now");
            else if (builder.Length == 0) builder.Append($"There are no {typeName.ToLower()}s in the garage right now");
            return builder.ToString();
        }

        public string CountVehicles()
        {
            if (isRecountRequired)
            {
                var vehicleCount = garage.Where(x => x != null)
                                         .GroupBy(x => x.GetType().Name)
                                         .Select(x => new { type = x.Key, ammount = x.Distinct().Count() });

                StringBuilder builder = new StringBuilder();
                foreach (var item in vehicleCount)
                {
                    builder.Append($"Vehicles of type: {item.type}: {item.ammount} \n");
                }

                countResults = builder.ToString();
                isRecountRequired = false;
            }
            return countResults;
        }

        public string SearchVehicles(Dictionary<string, object> searchTerms, Type vehicleType)
        {
            garage.GetVehicles().CopyTo(searchResults, 0);

            // loops over all vehicles
            int count = 0;
            foreach (var v in searchResults)
            {
                if ((v != null))
                {
                    if ((v.GetVehicleType().Equals(vehicleType) || vehicleType.Equals(typeof(Vehicle))))
                    {
                        // loops over all searchterms
                        foreach (var search in searchTerms)
                        {
                            // gets the property and checks if it exists
                            var property = v.GetType().GetProperty(search.Key);
                            if (property != null)
                            {
                                // if it is a string we check if the vehicles contains the search value
                                // otherwise we check for a perfect match
                                // if they don't contain the value we remove the vehicle
                                if (search.Value is String)
                                {
                                    if (!((property.GetValue(v).ToString().ToLower()).Contains(search.Value.ToString().ToLower())))
                                    {
                                        searchResults[count] = null;
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (!(property.GetValue(v).Equals(search.Value)))
                                    {
                                        searchResults[count] = null;
                                        continue;
                                    }
                                }
                            }// if property != null
                            else
                            {
                                searchResults[count] = null;
                            }
                        }// foreach searchTerm

                    }
                    else
                    {
                        searchResults[count] = null;
                    }
                } // if v != null
                else
                {
                    searchResults[count] = null;
                }
                count++;
            } // foreach vehicle

            // creates a list of all vehicles that match the searchterms
            StringBuilder builder = new StringBuilder();
            foreach (var v in searchResults)
            {
                if (v != null)
                {
                    builder.Append(v.ToString());
                    builder.Append("\n");
                }
            }

            if (builder.Length == 0) builder.Append("Could not find any matching vehicles");
            return builder.ToString();
        }
    }
}
