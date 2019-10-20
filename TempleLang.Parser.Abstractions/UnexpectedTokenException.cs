namespace TempleLang.Parser.Abstractions.Exceptions
{
    using TempleLang.Lexer.Abstractions;

    /// <summary>
    /// Non-generic static helper class for creating instances of the generic <see cref="UnexpectedTokenException{TParseTreeNodeType, TToken, TTokenType, TSourceFile}"/> class.
    /// </summary>
    public static class UnexpectedTokenException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedTokenException{TParseTreeNodeType, TToken, TTokenType, TSourceFile}"/> class.
        /// </summary>
        /// <param name="actual">The token that was read</param>
        /// <param name="context">Short description of the context under which the token was encountered</param>
        /// <param name="expectedDescription">A short description of the token types expected to be read, 
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}"</param>
        /// <param name="expected">The full list of all expected token types</param>

        public static UnexpectedTokenException<TParseTreeNodeType, TToken, TTokenType, TSourceFile>
            Create<TParseTreeNodeType, TToken, TTokenType, TSourceFile>(TToken actual, TParseTreeNodeType context, string expectedDescription, params TTokenType[] expected)
            where TToken : IToken<TTokenType, TSourceFile>
            where TSourceFile : ISourceFile
            => new UnexpectedTokenException<TParseTreeNodeType, TToken, TTokenType, TSourceFile>(actual, context, expectedDescription, expected);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedTokenException{TParseTreeNodeType, TToken, TTokenType, TSourceFile}"/> class.
        /// </summary>
        /// <param name="actual">The token that was read</param>
        /// <param name="context">Short description of the context under which the token was encountered</param>
        /// <param name="expectedDescription">A short description of the token types expected to be read,
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}"</param>

        public static UnexpectedTokenException<TParseTreeNodeType, TToken, TTokenType, TSourceFile>
            Create<TParseTreeNodeType, TToken, TTokenType, TSourceFile>(TToken actual, TParseTreeNodeType context, string expectedDescription)
            where TToken : IToken<TTokenType, TSourceFile>
            where TSourceFile : ISourceFile
            => new UnexpectedTokenException<TParseTreeNodeType, TToken, TTokenType, TSourceFile>(actual, context, expectedDescription);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedTokenException{TParseTreeNodeType, TToken, TTokenType, TSourceFile}"/> class.
        /// </summary>
        /// <param name="actual">The token that was read</param>
        /// <param name="context">Short description of the context under which the token was encountered</param>
        /// <param name="expected">The full list of all expected token types</param>

        public static UnexpectedTokenException<TParseTreeNodeType, TToken, TTokenType, TSourceFile>
            Create<TParseTreeNodeType, TToken, TTokenType, TSourceFile>(TToken actual, TParseTreeNodeType context, params TTokenType[] expected)
            where TToken : IToken<TTokenType, TSourceFile>
            where TSourceFile : ISourceFile
            => new UnexpectedTokenException<TParseTreeNodeType, TToken, TTokenType, TSourceFile>(actual, context, expected);
    }
}
