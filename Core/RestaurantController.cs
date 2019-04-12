namespace SoftUniRestaurant.Core
{
    using SoftUniRestaurant.Models.Drinks.Contracts;
    using SoftUniRestaurant.Models.Foods;
    using SoftUniRestaurant.Models.Foods.ChildClasses;
    using SoftUniRestaurant.Models.Foods.Contracts;
    using SoftUniRestaurant.Models.Tables.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class RestaurantController
    {
        private List<IFood> menu;
        private List<IDrink> drinks;
        private List<ITable> tables;
        private decimal totalMoney;
        public RestaurantController()
        {
            menu = new List<IFood>();
            drinks = new List<IDrink>();
            tables = new List<ITable>();
            totalMoney = 0;
        }

        public string AddFood(string type, string name, decimal price)
        {
            var assembly = Assembly.GetCallingAssembly();
            var foodType = assembly.GetTypes().First(x => x.Name == type);
            var ctor = foodType.GetConstructors().First();
            var food = (IFood)ctor.Invoke(new object[] { name, price });
            menu.Add(food);
            return $"Added {name} ({type}) with price {price:f2} to the pool";
        }

        public string AddDrink(string type, string name, int servingSize, string brand)
        {
            var assembly = Assembly.GetCallingAssembly();
            var drinkType = assembly.GetTypes().First(x => x.Name == type);
            var ctor = drinkType.GetConstructors().First();
            var drink = (IDrink)ctor.Invoke(new object[] { name, servingSize, brand });
            drinks.Add(drink);
            return $"Added {name} ({brand}) to the drink pool";
        }

        public string AddTable(string type, int tableNumber, int capacity)
        {
            var assembly = Assembly.GetCallingAssembly();
            var tableType = assembly.GetTypes().First(x => x.Name.StartsWith(type));
            var ctor = tableType.GetConstructors().First();
            var table = (ITable)ctor.Invoke(new object[] { tableNumber, capacity });
            tables.Add(table);
            return $"Added table number {tableNumber} in the restaurant";
        }

        public string ReserveTable(int numberOfPeople)
        {
            var notReserved = tables.FirstOrDefault(x => x.IsReserved == false && x.Capacity >= numberOfPeople);
            if (notReserved is null)
            {
                return $"No available table for {numberOfPeople} people";
            }
            else
            {
                notReserved.Reserve(numberOfPeople);
                return $"Table {notReserved.TableNumber} has been reserved for {numberOfPeople} people";
            }
        }

        public string OrderFood(int tableNumber, string foodName)
        {
            var table = tables.FirstOrDefault(x => x.TableNumber == tableNumber);
            if (table is null)
            {
                return $"Could not find table with {tableNumber}";
            }
            else
            {
                if (menu.Any(x => x.Name == foodName))
                {
                    table.OrderFood(menu.First(x => x.Name == foodName));
                    return $"Table {tableNumber} ordered {foodName}";
                }
                else
                {
                    return $"No {foodName} in the menu";
                }
            }
        }

        public string OrderDrink(int tableNumber, string drinkName, string drinkBrand)
        {
            var table = tables.FirstOrDefault(x => x.TableNumber == tableNumber);
            if (table is null)
            {
                return $"Could not find table with {tableNumber}";
            }
            else
            {
                if (drinks.Any(x => x.Name == drinkName && x.Brand == drinkBrand))
                {
                    table.OrderDrink(drinks.First(x => x.Name == drinkName && x.Brand == drinkBrand));
                    return $"Table {tableNumber} ordered {drinkName} {drinkBrand}";
                }
                else
                {
                    return $"There is no {drinkName} {drinkBrand} available";
                }
            }
        }

        public string LeaveTable(int tableNumber)
        {
            var table = tables.First(x => x.TableNumber == tableNumber);
            var bill = table.GetBill();
            table.Clear();
            totalMoney += bill;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Table: {tableNumber}").AppendLine($"Bill: {bill:f2}");
            return sb.ToString().TrimEnd();
        }

        public string GetFreeTablesInfo()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in tables.Where(x => !x.IsReserved))
            {
                sb.AppendLine(item.GetFreeTableInfo());
            }

            return sb.ToString().TrimEnd();
        }

        public string GetOccupiedTablesInfo()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in tables.Where(x => x.IsReserved))
            {
                sb.AppendLine(item.GetOccupiedTableInfo());
            }

            return sb.ToString().TrimEnd();
        }

        public string GetSummary()
        {

            return $"Total income: {totalMoney:f2}lv";
        }
    }
}
