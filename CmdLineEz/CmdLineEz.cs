namespace CmdLineEz
{
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
            throw new NotImplementedException();
        }
    }
}
