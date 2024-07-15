using CmdLineEz.Attributes;

namespace CmdLineEz.Tests.TestData
{
    public class DeleteCommandLine
    {
        [CmdLineEz("confirm", CmdLineEzAttributeFlags.Required)]
        public bool ConfirmNeeded { get; set; }

        public bool Recursive { get; set; }

        [CmdLineEz("verbose")]
        public bool PrintDetails { get; set; }

        public string Prefix { get; set; }

        [CmdLineEz("verbose", CmdLineEzAttributeFlags.Remaining)]
        public List<string> Remaining {get; set;}
    }
}
