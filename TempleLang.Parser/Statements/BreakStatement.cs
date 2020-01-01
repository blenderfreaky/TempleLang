﻿using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Diagnostic;
using TempleLang.Lexer;
using TempleLang.Parser.Abstractions;

namespace TempleLang.Parser.Statements
{
    public class BreakStatement : Statement
    {
        public BreakStatement(FileLocation location) : base(location)
        {
        }

        public override string ToString() => $"break";

        public static new readonly Parser<BreakStatement, Token> Parser =
            from breakKeyword in Parse.Token(Token.Break)
            from __ in Parse.Token(Token.Semicolon)
            select new BreakStatement(breakKeyword.Location);
    }
}
