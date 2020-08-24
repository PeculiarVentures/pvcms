using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using pvcms.Commands;

namespace pvcms
{
    class Program
    {
        public static int result;

        public static int Main(string[] args)
        {
            ProcessArgs(args);
            return result;
        }

        private static void ProcessArgs(IEnumerable<string> args)
        {
            // We can use generic helpers or pass the command types manually.
            Parser.Default.ParseVerbs(
                args.ToArray(),
                typeof(VerifyCommand))
                .WithParsed(ExecuteCommand);
                //.WithNotParsed(ParseError);
        }

        private static void ExecuteCommand(object arg)
        {
            var command = (BaseCommand)arg;
            // Actual work is done here.
            command.Execute();
        }

        //private static void ParseError(IEnumerable<Error> obj) => returnValue = 1;
    }
}
