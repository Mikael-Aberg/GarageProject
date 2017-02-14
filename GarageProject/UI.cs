using GarageProject.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageProject
{
    class UI
    {
        GarageHandler handler;

        public void ShowStartMenu()
        {
            CreateGarage();
            bool looping = true;
            do
            {
                Console.Clear();
                // prints the main menu and handles the input
                switch (AskForString(GetMainMenu()))
                {
                    case "1":
                        // if the garage is not full we add a new vehicle
                        if (handler.CanAddNewVehicle()) { handler.AddNewVehicle(CreateNewVehicle(GetVehicleTypeToAdd())); }
                        else { WriteMessage("You can not add a vehicle right now"); }
                        break;
                    case "2":
                        RemoveVehicle();
                        break;
                    case "3":
                        WriteMessage(handler.ListVehicles());
                        break;
                    case "4":
                        WriteMessage(handler.CountVehicels());
                        break;
                    case "5":
                        WriteMessage(handler.ListVehicleTypes(GetVehicleTypeToList()));
                        break;
                    case "6":
                        WriteMessage(handler.SearchWithRegNumber(AskForStringRange("Please enter the registration number to search for", 6, 6)));
                        break;
                    case "7":
                        WriteMessage(SearchForVehicles());
                        break;
                    case "8":
                        handler = new GarageHandler(AskForIntRange("Please enter the size of your garage:", 1, 100));
                        break;
                    case "0":
                        handler.SaveGarage();
                        looping = false;
                        break;
                }
            } while (looping);
        }

        private void RemoveVehicle()
        {
            // if there is not vehicles we tell the user and return
            if (handler.GetCount() == 0)
            {
                WriteMessage("There are no vehicles in the garage right now");
                return;
            }

            // a array of bools to determine if we should show more information about a vehicle or not
            bool[] showMoreInfo = new bool[handler.GetCount()];
            int outPut = 0;
            int selectCount = 0;
            do
            {

                // gets the choosen option from the user and the number choosen in the output variable
                RemoveListOptions selectedoption = GetRemoveListOption(handler.GetVehicleInformationMenu(showMoreInfo, out selectCount), 0, selectCount - 1, out outPut);
                outPut--;

                // if user enters 0 we quit
                if (outPut == -1) return;

                switch (selectedoption)
                {
                    case RemoveListOptions.remove:
                        handler.RemoveVehicle(outPut);
                        Array.Clear(showMoreInfo, 0, showMoreInfo.Length);
                        break;
                    case RemoveListOptions.moreinfo:
                        showMoreInfo[outPut] = true;
                        break;
                    case RemoveListOptions.lessinfo:
                        showMoreInfo[outPut] = false;
                        break;
                }
            } while (true);
        }

        private Vehicle CreateNewVehicle(VehicleTypes type)
        {
            // gets the name of the vehicle to create to use in the questions
            var vehicleName = ((VehicleTypes)type).ToString().ToLower();

            // gets all the information all vehicles have
            var regNr = AskForStringRange($"Please enter the registration number of your {vehicleName}.", 6, 6);
            var color = AskForString($"Please enter the color of your {vehicleName}.");
            var numberOfWheels = !(vehicleName == "boat") ? AskForInt($"Please enter the number of wheels on your {vehicleName}.") : 0;
            var maxSpeed = AskForInt($"Please enter the max speed of your {vehicleName}.");

            // depending on type we get the last bit of information and create the vehicle
            switch (type)
            {
                case VehicleTypes.Airplane:
                    var numberOfEngines = AskForInt($"Please enter the number of engines on your {vehicleName}.");
                    return new Airplane(regNr, color, numberOfWheels, maxSpeed, numberOfEngines);
                case VehicleTypes.Boat:
                    var length = AskForInt($"Please enter the length of your {vehicleName}.");
                    return new Boat(regNr, color, numberOfWheels, maxSpeed, length);
                case VehicleTypes.Bus:
                    var numberOfSeats = AskForInt($"Please enter the number of seats on your {vehicleName}.");
                    return new Bus(regNr, color, numberOfWheels, maxSpeed, numberOfSeats);
                case VehicleTypes.Car:
                    return new Car(regNr, color, numberOfWheels, maxSpeed, GetFuletype());
                case VehicleTypes.Motorcycle:
                    var cylinderVolume = AskForFloat($"Please enter the cylinder volume of your {vehicleName}.");
                    return new Motorcycle(regNr, color, numberOfWheels, maxSpeed, cylinderVolume);
            }
            return null;
        }

        private VehicleTypes GetVehicleTypeToAdd()
        {
            // gets the number of vehicletypes
            var length = Enum.GetNames(typeof(VehicleTypes)).Length;

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                // puts all of them in to a list
                var name = ((VehicleTypes)i).ToString();
                builder.Append($"Add new vehicle of type: {name}");
                builder.Append("\n");
            }
            // returns the selected vehicletype
            return ((VehicleTypes)AskForIntRange(builder.ToString(), 1, length) - 1);
        }

        private VehicleTypes GetVehicleTypeToList()
        {
            // gets the number of vehicletypes
            var length = Enum.GetNames(typeof(VehicleTypes)).Length;

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                // puts all of them in to a list
                var name = ((VehicleTypes)i).ToString().ToLower();
                if (name != "bus") builder.Append($"{i + 1}. List all {((VehicleTypes)i).ToString().ToLower() }s");
                else builder.Append($"{i + 1}. List all { ((VehicleTypes)i).ToString().ToLower() }es");
                builder.Append("\n");
            }
            // returns the selected vehicletype
            return ((VehicleTypes)AskForIntRange(builder.ToString(), 1, length) - 1);
        }

        private Type GetVehicleTypeToSearch()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Select what you want to search for \n");

            // lets the user select what type of vehicle to search for or to search for all vehicles
            int count = 1;
            foreach (var item in Enum.GetNames(typeof(VehicleTypes)))
            {
                builder.Append($"{count}. Search for all vehicles of type: {item} \n");
                count++;
            }
            builder.Append($"{count}. Search for all vehicles");

            // gets input from the user
            var input = AskForIntRange(builder.ToString(), 1, count) - 1;

            // last option in list is always all vehicles
            if (input == count - 1)
            {
                return Type.GetType("GarageProject.Vehicles.Vehicle");
            }
            else
            {
                // vehcileType is now the type of vehicle the user choose
                return Type.GetType("GarageProject.Vehicles." + ((VehicleTypes)input).ToString());
            }
        }

        private string BuildSearchMenu(Type vehicleType, out int count)
        {
            StringBuilder builder = new StringBuilder();
            Console.WriteLine();
            count = 1;
            foreach (var item in vehicleType.GetProperties())
            {
                // creates a list with all the properties of the choosen vehicletype
                var itemName = item.Name.First().ToString().ToUpper() + item.Name.Substring(1);
                if (itemName != "Index")
                {
                    builder.Append($"{count}. Search using {itemName} \n");
                    count++;
                }
            }

            builder.Append("Press 0 to execute search");

            return builder.ToString();
        }

        private string SearchForVehicles()
        {
            Type vehicleType = GetVehicleTypeToSearch();

            if (vehicleType == null) return "Sorry something went wrong";


            // dictionary used to save the values to search for
            Dictionary<string, object> searchTerms = new Dictionary<string, object>();

            var vehicleProperties = vehicleType.GetProperties().ToArray();

            int count;
            string searchString = BuildSearchMenu(vehicleType, out count);

            int userSelectedSearch = -1;
            string extraText = "";
            do
            {
                // gets selection from the user
                userSelectedSearch = AskForIntRange(searchString + extraText, 0, count - 1) - 1;

                // if the user entered 0 we do the search
                if (userSelectedSearch < 0) return handler.SearchVehicles(searchTerms, vehicleType);

                // gets the property the user selected
                var value = vehicleProperties[userSelectedSearch];

                // gets the type of property and the name of the property
                var name = value.Name;
                var type = value.PropertyType;

                // depending on the type of property we ask for different input from the user
                if (type == typeof(string))
                {
                    searchTerms[name] = AskForString($"Enter your search term for {name}:");
                }
                else if (type == typeof(int))
                {
                    searchTerms[name] = AskForInt($"Enter your search term for {name}:");
                }
                else if (type == typeof(float))
                {
                    searchTerms[name] = AskForFloat($"Enter your search term for {name}:");
                }
                else if (type == typeof(FuelType))
                {
                    searchTerms[name] = GetFuletype();
                }

                // prints a list of all the current searchterms
                extraText = "\n\nCurrent search terms: \n";
                foreach (var item in searchTerms)
                {
                    extraText += $"{item.Key} : {item.Value} \n";
                }
            } while (true);
        }

        private FuelType GetFuletype()
        {
            //Gets a fuletype from the user
            var fuleTypeNumber = AskForIntRange("Please choose your fuletype. \n1. Gasoline \n2. Diesel", 1, 2);
            FuelType fuleType;
            fuleType = (fuleTypeNumber == 1) ? FuelType.Gasoline : FuelType.Diesel;
            return fuleType;
        }

        private void CreateGarage()
        {
            var input = AskForIntRange("1. Create new garage \n2. Load last used garage", 1, 2);

            if (input == 1) handler = new GarageHandler(AskForIntRange("Please enter the size of your garage:", 1, 100));
            else handler = new GarageHandler(0, true);
        }

        private void WriteMessage(string message)
        {
            // writes a single message to the console
            Console.Clear();
            Console.WriteLine(message);
            Console.WriteLine("\nPress any key to return to the main menu . . .");
            Console.ReadKey();
        }

        private int AskForInt(string query)
        {
            // gets an int value from the user
            string input = "";
            string error = "";
            int result = 0;
            do
            {
                input = AskForString(error + query);
                error = $"Please enter only numbers \n(between {int.MinValue} & {int.MaxValue})\n\n";

            } while (!int.TryParse(input, out result));

            return result;
        }

        private int AskForIntRange(string query, int minRange, int maxRange)
        {
            // gets a int value between a min and max value from the user
            int input = 0;
            string error = "";
            do
            {
                input = AskForInt(error + query);
                if (minRange != maxRange) error = $"Please enter a number between {minRange} and {maxRange} \n\n";
                else error = $"Your only choice here is {minRange} \n\n";

            } while (input < minRange || input > maxRange);

            return input;
        }

        private float AskForFloat(string query)
        {
            // gets a float value from the user
            string input = "";
            string error = "";
            float result = 0f;
            do
            {
                input = AskForString(error + query);
                error = $"Please enter only numbers and only use \",\" to use decimals \n(between {float.MinValue} & {float.MaxValue})\n\n";

            } while (!float.TryParse(input, out result));

            return result;
        }

        private RemoveListOptions GetRemoveListOption(string query, int minRange, int maxRange, out int outPut)
        {
            var input = "";
            var error = "";
            do
            {
                // gets input from the user
                input = AskForString(error + query);
                char firstChar = input[0];

                // if the first character is + or - we show more or less information about the selected vehicle
                // otherwise we remove it
                switch (firstChar)
                {
                    case '+':
                        if (int.TryParse(input.Substring(1), out outPut))
                        {
                            if (outPut <= maxRange && outPut >= minRange)
                                return RemoveListOptions.moreinfo;
                            error = $"Please enter a number between {minRange} and {maxRange}\n\n";
                        }
                        break;
                    case '-':
                        if (int.TryParse(input.Substring(1), out outPut))
                        {
                            if (outPut <= maxRange && outPut >= minRange)
                                return RemoveListOptions.lessinfo;
                            error = $"Please enter a number between {minRange} and {maxRange}\n\n";
                        }
                        break;
                    default:
                        if (int.TryParse(input, out outPut))
                        {
                            if (outPut <= maxRange && outPut >= minRange)
                                return RemoveListOptions.remove;
                            error = $"Please enter a number between {minRange} and {maxRange}\n\n";
                        }
                        break;
                }
                error = $"Please enter only numbers between {minRange} and {maxRange} \n\n";
            } while (true);
        }

        private string AskForStringRange(string query, int minRange, int maxRange)
        {
            // gets a string value between a min and max length from the user
            var input = "";
            var error = "";
            do
            {
                input = AskForString(error + query);
                if (minRange == maxRange) error = $"Please enter a text that is {maxRange} letters long \n\n";
                else error = $"Please enter a text between {minRange} and {maxRange} letters long \n\n";

            } while (input.Length < minRange || input.Length > maxRange);

            return input;
        }

        private string AskForString(string query = "")
        {
            // gets a string value from the user
            string error = "";
            string input = "";
            do
            {
                Console.Clear();
                Console.WriteLine(error + query);
                error = "Please enter some text \n\n";
                input = Console.ReadLine();

            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }

        private string GetMainMenu()
        {
            // returns main menu, only made this way to make it easier to read
            return "1. Add new vechicle to the garage  \n"
                 + "2. Remove vechicle from the garage \n"
                 + "3. List all vehicles in the garage \n"
                 + "4. List the number of specific vehicles are in the garage \n"
                 + "5. List a specific type of vehicle in the garage \n"
                 + "6. Find vehicle using the registration number \n"
                 + "7. Search vechiles using other criteria \n"
                 + "8. Create a new garage \n"
                 + "0. Exit the application";
        }
    }
}