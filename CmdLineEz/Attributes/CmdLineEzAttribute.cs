using System.Xml.Linq;

namespace CmdLineEz.Attributes
{
    public enum CmdLineEzAttributeFlags
    {
        None = 0, 
        Required = 1, 
        Remaining = 2
    }

    
    [AttributeUsage(AttributeTargets.Property)]
    public class CmdLineEzAttribute : Attribute
    {
        public CmdLineEzAttributeFlags Flags { get; set; }
        /// <summary>
        ///  If the property has altName specified it means that that name, rather than the property name should be used.
        /// </summary>
        public string AltName { get; set; } = default!;
        public CmdLineEzAttribute(string altName, CmdLineEzAttributeFlags flags = CmdLineEzAttributeFlags.None)
        {
            AltName = altName;
            Flags = flags;
        }
        public CmdLineEzAttribute(CmdLineEzAttributeFlags flags = CmdLineEzAttributeFlags.None)
        {
            Flags = flags;
        }
    }
}
