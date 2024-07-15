using CmdLineEz.Attributes;
using System.Reflection;

namespace CmdLineEz
{
    /// <summary>
    /// Processor for command line arguments.
    /// </summary>
    public class CmdLineEz<T>
    {
        /// <summary>
        /// Pass your customized template class here to read the fields, pass the args array into this to process it and set the values.
        /// </summary>
        /// <param name="result">Your customized template class</param>
        /// <param name="args">Array of args, generally speaking pass in args from the Main function</param>
        /// <returns>Collection of string-errors or null if there are no error</returns>
        /// <exception cref="NotSupportedException">Throws if </exception>
        public static List<string> Process(T result, string[] args)
        {
            if (result == null)
                throw new ArgumentException("No parameter specification loaded.");

            var errors = new List<string>();
            var argsList = args.ToList();

            #region Here we would use reflection to get all the fields in the type T and their types

            var properties = typeof(T).GetProperties();
            var remainingProperty = properties
                .Where(p => p.GetCustomAttribute<CmdLineEzAttribute>()?.Flags.HasFlag(CmdLineEzAttributeFlags.Remaining) == true);

            foreach (var prop in properties)
            {
                var attribute = prop.GetCustomAttribute<CmdLineEzAttribute>();
                string paramName = attribute?.AltName ?? prop.Name.ToLower();

                var inputParam = argsList.FirstOrDefault(a =>
                a.StartsWith($"/{paramName}=", StringComparison.OrdinalIgnoreCase)
                ||
                a.StartsWith($"/{paramName}", StringComparison.OrdinalIgnoreCase));


                if (inputParam != null)
                {
                    string value = null!;

                    if (inputParam.Contains("="))
                        value = inputParam.Split('=')[1];

                    try
                    {
                        prop.SetValue(result, prop.PropertyType.Name switch
                        {
                            nameof(Boolean) => true,
                            nameof(Int32) => int.Parse(value),
                            nameof(Decimal) => decimal.Parse(value),
                            nameof(String) => value,
                            _ => throw new NotSupportedException($"Type {prop.PropertyType} in not supported")
                        });
                    }
                    catch (NotSupportedException)
                    {
                        errors.Add($"invalid {paramName}");
                    }
                }
                else if (attribute?.Flags.HasFlag(CmdLineEzAttributeFlags.Required) == true)
                {
                    errors.Add($"missing {paramName}");
                }
            }

            if (remainingProperty.Count() > 1)
            {
                errors.Add($"There are not allowed more then 1 remaining properties");
            }

            List<string> remainingArgs = args.Where(a => !a.StartsWith("/")).ToList();
            remainingProperty.First().SetValue(result, remainingArgs);

            #endregion




            return errors.Any() ? errors : null!;
        }
    }
}
