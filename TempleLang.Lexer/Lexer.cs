namespace TempleLang.Lexer.Abstractions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using State = LexerState<TokenType>;
    using Step = LexerStep<TokenType>;

    public static class Lexer
    {
        private static readonly Lexer<TokenType, SourceFile> InternalLexer = new Lexer<TokenType, SourceFile>(BaseState);

        private static readonly State BaseState = new State(nameof(BaseState), characterInt =>
        {
            if (characterInt == -1) throw new UnexpectedCharException(null, "Letter/Digit/Operator", "the lexer was in the state " + nameof(BaseState));

            char character = (char)characterInt;

            if (char.IsLetter(character)) return new Step(IdentifierOrKeyword);
            if (char.IsDigit(character)) return new Step(IntOrRealLHS);

            switch (character)
            {
                case '+': return new Step(BaseState, TokenType.Add);
                case '-': return new Step(BaseState, TokenType.Subtract);
                case '*': return new Step(BaseState, TokenType.Multiply);
                case '/': return new Step(BaseState, TokenType.Divide);
                case '!': return new Step(BaseState, TokenType.Not);
                case '&': return new Step(
                    State.Combine("AndOrBitwiseAnd",
                        BaseState.Step, 
                        character => character != '&' ? null : new Step(BaseState, TokenType.And), 
                        TokenType.And);
            }

            throw new UnexpectedCharException(character, "Letter/Digit/Operator", "the lexer was in the state " + nameof(BaseState));
        });

        private static readonly State IdentifierOrKeyword;

        private static readonly State IntOrRealLHS;

        public static IEnumerable<Token<TokenType, SourceFile>> Tokenize(TextReader textReader, SourceFile sourceFile) =>
            InternalLexer.Tokenize(textReader, sourceFile);
    }
}
