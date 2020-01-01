﻿using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Diagnostic;
using TempleLang.Lexer;
using TempleLang.Parser.Abstractions;

namespace TempleLang.Parser.Statements
{
    public class ContinueStatement : Statement
    {
        public ContinueStatement(FileLocation location) : base(location)
        {
        }

        public override string ToString() => $"break";

        public static new readonly Parser<ContinueStatement, Token> Parser =
            from continueKeyword in Parse.Token(Token.Continue)
            from __ in Parse.Token(Token.Semicolon)
            select new ContinueStatement(continueKeyword.Location);
    }
}
