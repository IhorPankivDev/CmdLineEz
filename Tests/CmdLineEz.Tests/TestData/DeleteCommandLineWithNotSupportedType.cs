using CmdLineEz.Attributes;

namespace CmdLineEz.Tests.TestData
{
    public class DeleteCommandLineWithNotSupportedType
    {
        [CmdLineEz("confirm", CmdLineEzAttributeFlags.Required)]
        public bool ConfirmNeeded { get; set; }

        [CmdLineEz("char")]
        public char NotSupportedPror { get; set; }

        public bool Recursive { get; set; }

        [CmdLineEz("verbose")]
        public bool PrintDetails { get; set; }

        public string Prefix { get; set; }

        [CmdLineEz("verbose", CmdLineEzAttributeFlags.Remaining)]
        public List<string> Remaining { get; set; }
    }
}
