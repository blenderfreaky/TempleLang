using System;

namespace TempleLang.Parser.Abstractions
{
    public struct ParserError
    {
        public string ErrorMessage { get; }

        public static ParserError AggregateError(params ParserError[] parserError) => throw new NotImplementedException();
    }
}