using CmdLineEz.Attributes;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CmdLineEz
{
    /// <summary>
    /// Processor for command line arguments.
    /// </summary>
    public class CmdLineEz<T> where T : new()
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

            #endregion

            #region Parsing remaining props

            if (remainingProperty.Count() > 1)
            {
                errors.Add($"There are not allowed more then 1 remaining properties");
            }

            List<string> remainingArgs = args.Where(a => !a.StartsWith("/")).ToList();
            remainingProperty.First().SetValue(result, remainingArgs);
            properties = properties.Except(remainingProperty).ToArray();

            #endregion

            #region Parsing main props

            foreach (var prop in properties)
            {
                var attribute = prop.GetCustomAttribute<CmdLineEzAttribute>();
                string paramName = attribute?.AltName ?? prop.Name.ToLower();

                IEnumerable<string> relevantByTypeParams = new List<string>();

                try
                {
                    relevantByTypeParams = Type.GetTypeCode(prop.PropertyType) switch
                    {
                        TypeCode.Boolean => argsList.Where(a => Regex.Match(a, $@"^/{paramName}\b", RegexOptions.IgnoreCase).Success),
                        TypeCode.Int32 or TypeCode.Decimal or TypeCode.String => argsList.Where(a => a.StartsWith($"/{paramName}=", StringComparison.OrdinalIgnoreCase)),
                        _ => throw new NotSupportedException()
                    };
                }
                catch (NotSupportedException)
                {
                    errors.Add($"invalid type of {paramName} parameter");
                }

                if (relevantByTypeParams.Count() > 1)
                {
                    errors.Add($"Ambiguous parameter {paramName}");
                }

                string inputParam = relevantByTypeParams.Count() > 0 ? relevantByTypeParams.First() : null!;

                if (inputParam != null)
                {
                    string value = null!;

                    if (inputParam.Contains("="))
                    {
                        if (inputParam.Length <= inputParam.IndexOf("=") + 1)
                        {
                            errors.Add($"invalid {paramName}");
                        }
                        else
                        {
                            value = inputParam.Split('=')[1];
                        }
                    }

                    prop.SetValue(result, Type.GetTypeCode(prop.PropertyType) switch
                    {
                        TypeCode.Boolean => true,
                        TypeCode.Int32 => int.Parse(value),
                        TypeCode.Decimal => decimal.Parse(value),
                        TypeCode.String => value,
                        _ => throw new NotSupportedException()
                    });
                }
                else if (attribute?.Flags.HasFlag(CmdLineEzAttributeFlags.Required) == true)
                {
                    errors.Add($"missing {paramName}");
                }
            }

            #endregion

            return errors.Any() ? errors : null!;
        }

        public static T Parse(string fullCommandString)
        {
            string[] args = fullCommandString
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1) // The first in line is the command type which should be skiped
                .ToArray();
            T result = new T();

            #region Check for passed duplicates

            var duplicatedParameters = args.GroupBy(p => p)
                .Select(group => new
                {
                    Name = group.Key,
                    Count = group.Count()
                })
                .Where(group => group.Count > 1)
                .ToList();

            if (duplicatedParameters.Count > 0)
            {
                throw new ArgumentException("Duplicated parameters in command line");
            }

            #endregion

            #region Check that remaining args puts in the end of the command string

            bool nonOptionalStarted = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("/"))
                {
                    if (nonOptionalStarted)
                    {
                        throw new ArgumentException("Command parsing failed: remaining parameters must come at the end.");
                    }
                }
                else
                {
                    nonOptionalStarted = true;
                }
            }

            #endregion

            var errors = Process(result, args);

            if (errors != null && errors.Count > 0)
            {
                throw new InvalidOperationException("Command parsing failed: " + string.Join(", ", errors));
            }

            return result;
        }
    }
}
