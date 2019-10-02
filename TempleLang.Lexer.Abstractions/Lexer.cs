using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TempleLang.Lexer.Abstractions
{
    public struct Lexer<TTokenMatcher, TToken, TTokenType, TFile> : ILexer<TToken, TTokenType, TFile>
        where TTokenMatcher : ITokenMatcher<TToken, TTokenType, TFile>
        where TToken : IToken<TTokenType, TFile>
        where TFile : IFile
    {
        public IReadOnlyCollection<TTokenMatcher> TokenMatchers { get; }

        public Lexer(IReadOnlyCollection<TTokenMatcher> tokenMatchers)
        {
            TokenMatchers = tokenMatchers;
        }

        public IEnumerable<TToken> Tokenize(TextReader textReader, TFile file)
        {
            TTokenMatcher[] tokenMatchers = TokenMatchers.ToArray();

            while (tokenMatchers.Length > 1)
            {
                foreach (var tokenMatcher in tokenMatchers)
                {
                    var nextValue = textReader.Read();

                    if (nextValue != -1)
                    {
                        var nextChar = (char)nextValue;

                        tokenMatchers = tokenMatchers.Where(x => x.StillMatches(nextChar)).ToArray();
                    }
                    else
                    {

                    }
                }
            }

            if (tokenMatchers.Length == 0) throw new Exception("No token found matching ");

            return tokenMatchers;
        }
    }
}
