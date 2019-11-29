using TempleLang.Lexer;

namespace TempleLang.Parser.Abstractions.Exceptions
{
    /// <summary>
    /// Non-generic static helper class for creating instances of the generic <see cref="UnexpectedLexemeException{TParseTreeNodeType,  TToken}"/> class.
    /// </summary>
    public static class UnexpectedLexemeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedLexemeException{TParseTreeNodeType,  TToken}"/> class.
        /// </summary>
        /// <param name="actual">The token that was read</param>
        /// <param name="context">Short description of the context under which the token was encountered</param>
        /// <param name="expectedDescription">A short description of the token types expected to be read,
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}"</param>
        /// <param name="expected">The full list of all expected token types</param>

        public static UnexpectedLexemeException<TParseTreeNodeType, TToken>
            Create<TParseTreeNodeType, TToken>(Lexeme<TToken> actual, TParseTreeNodeType context, string expectedDescription, params TToken[] expected)

            => new UnexpectedLexemeException<TParseTreeNodeType, TToken>(actual, context, expectedDescription, expected);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedLexemeException{TParseTreeNodeType,  TToken}"/> class.
        /// </summary>
        /// <param name="actual">The token that was read</param>
        /// <param name="context">Short description of the context under which the token was encountered</param>
        /// <param name="expectedDescription">A short description of the token types expected to be read,
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}"</param>

        public static UnexpectedLexemeException<TParseTreeNodeType, TToken>
            Create<TParseTreeNodeType, TToken>(Lexeme<TToken> actual, TParseTreeNodeType context, string expectedDescription)

            => new UnexpectedLexemeException<TParseTreeNodeType, TToken>(actual, context, expectedDescription);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedLexemeException{TParseTreeNodeType,  TToken}"/> class.
        /// </summary>
        /// <param name="actual">The token that was read</param>
        /// <param name="context">Short description of the context under which the token was encountered</param>
        /// <param name="expected">The full list of all expected token types</param>

        public static UnexpectedLexemeException<TParseTreeNodeType, TToken>
            Create<TParseTreeNodeType, TToken>(Lexeme<TToken> actual, TParseTreeNodeType context, params TToken[] expected)

            => new UnexpectedLexemeException<TParseTreeNodeType, TToken>(actual, context, expected);
    }
}