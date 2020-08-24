using System;
using System.Linq;
using System.Reflection;
using CommandLine;

namespace pvcms
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SubVerbsAttribute : Attribute
    {
        public Type[] Types { get; }

        public SubVerbsAttribute(params Type[] types)
        {
            Types = types;
        }
    }

    public static class ParserVerbExtensions
    {
        public static ParserResult<object> ParseVerbs(this Parser parser, string[] args, params Type[] types)
        {
            if (args.Length == 0 || args[0].StartsWith("-"))
            {
                return parser.ParseArguments(args, types);
            }

            var verb = args[0];
            foreach (var type in types)
            {
                var verbAttribute = type.GetCustomAttribute<VerbAttribute>();
                if (verbAttribute == null || verbAttribute.Name != verb)
                {
                    continue;
                }

                var subVerbsAttribute = type.GetCustomAttribute<SubVerbsAttribute>();
                if (subVerbsAttribute != null)
                {
                    return ParseVerbs(parser, args.Skip(1).ToArray(), subVerbsAttribute.Types);
                }

                break;
            }

            return parser.ParseArguments(args, types);
        }
    }
}
