using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Enigma
{
    internal static class ArgumentEscaper
    {
        public static string EscapeAndConcatenate(IEnumerable<string> args)
            => string.Join(" ", args.Select(EscapeSingleArg));
 
        private static string EscapeSingleArg(string arg)
        {
            var sb = new StringBuilder();
 
            var needsQuotes = ShouldSurroundWithQuotes(arg);
            var isQuoted = needsQuotes || IsSurroundedWithQuotes(arg);
 
            if (needsQuotes)
            {
                sb.Append('"');
            }
 
            for (int i = 0; i < arg.Length; ++i)
            {
                var backslashes = 0;
 
                // Consume all backslashes
                while (i < arg.Length && arg[i] == '\\')
                {
                    backslashes++;
                    i++;
                }
 
                if (i == arg.Length && isQuoted)
                {
                    // Escape any backslashes at the end of the arg when the argument is also quoted.
                    // This ensures the outside quote is interpreted as an argument delimiter
                    sb.Append('\\', 2 * backslashes);
                }
                else if (i == arg.Length)
                {
                    // At then end of the arg, which isn't quoted,
                    // just add the backslashes, no need to escape
                    sb.Append('\\', backslashes);
                }
                else if (arg[i] == '"')
                {
                    // Escape any preceding backslashes and the quote
                    sb.Append('\\', (2 * backslashes) + 1);
                    sb.Append('"');
                }
                else
                {
                    // Output any consumed backslashes and the character
                    sb.Append('\\', backslashes);
                    sb.Append(arg[i]);
                }
            }
 
            if (needsQuotes)
            {
                sb.Append('"');
            }
 
            return sb.ToString();
        }
 
        private static bool ShouldSurroundWithQuotes(string argument)
        {
            // Don't quote already quoted strings
            if (IsSurroundedWithQuotes(argument))
            {
                return false;
            }
 
            // Only quote if whitespace exists in the string
            return ContainsWhitespace(argument);
        }
 
        private static bool IsSurroundedWithQuotes(string argument)
        {
            if (argument.Length <= 1)
            {
                return false;
            }
 
            return argument[0] == '"' && argument[argument.Length - 1] == '"';
        }
 
        private static bool ContainsWhitespace(string argument)
            => argument.IndexOfAny(new [] { ' ', '\t', '\n' }) >= 0;
    }
}