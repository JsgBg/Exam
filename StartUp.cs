using SoftUniRestaurant.Core;
using SoftUniRestaurant.Models.Drinks.Contracts;
using SoftUniRestaurant.Models.Tables.ChildClasses;
using SoftUniRestaurant.Models.Tables.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SoftUniRestaurant
{
    public class StartUp
    {
        public static void Main()
        {
            var assembly = Assembly.GetCallingAssembly();
            var controllerType = assembly.GetTypes().First(x => x.Name == "RestaurantController");
            RestaurantController controller = new RestaurantController();
            var controllerMethods = controller.GetType().GetMethods();
            while (true)
            {
                string[] input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (input[0] == "END")
                {
                    Console.WriteLine(controller.GetSummary());
                    return;
                }
                var method = controllerMethods.First(x => x.Name == input[0]);
                object[] args = getArgs(input, method);
                try
                {
                    Console.WriteLine(method.Invoke(controller, args));
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.InnerException.InnerException.Message);
                }

            }
        }

        private static object[] getArgs(string[] input, MethodInfo method)
        {
            var argsType = method.GetParameters().Select(x => x.ParameterType.Name).ToArray();
            object[] args = new object[input.Length - 1];
            int p = 0;
            for (int i = 1; i < input.Length; i++)
            {
                var type = Type.GetType("System." + argsType[p]);
                args[p] = Convert.ChangeType(input[i], type);
                p++;
            }

            return args;
        }
    }
}
