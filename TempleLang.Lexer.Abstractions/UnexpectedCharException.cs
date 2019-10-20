namespace TempleLang.Lexer.Abstractions.Exceptions
{
    /// <summary>
    /// Non-generic static helper class for creating instances of the generic <see cref="UnexpectedCharException{TTokenType}"/> class.
    /// </summary>
    public static class UnexpectedCharException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedCharException{TTokenType}"/> class.
        /// </summary>
        /// <param name="actual">The char that was read. Null if EoF</param>
        /// <param name="context">Short description of the context under which the char was encountered</param>
        /// <param name="expectedDescription">A short description of the chars expected to be read, 
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}"</param>
        /// <param name="expected">The full list of all expected characters</param>
        public static UnexpectedCharException<TTokenType>
            Create<TTokenType>(char? actual, TTokenType context, string expectedDescription, params char[] expected)
            => new UnexpectedCharException<TTokenType>(actual, context, expectedDescription, expected);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedCharException{TTokenType}"/> class.
        /// </summary>
        /// <param name="actual">The char that was read. Null if EoF</param>
        /// <param name="context">Short description of the context under which the char was encountered</param>
        /// <param name="expectedDescription">A short description of the chars expected to be read,
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}"</param>
        public static UnexpectedCharException<TTokenType>
            Create<TTokenType>(char? actual, TTokenType context, string expectedDescription)
            => new UnexpectedCharException<TTokenType>(actual, context, expectedDescription);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedCharException{TTokenType}"/> class.
        /// </summary>
        /// <param name="actual">The char that was read. Null if EoF</param>
        /// <param name="context">Short description of the context under which the char was encountered</param>
        /// <param name="expected">The full list of all expected characters</param>
        public static UnexpectedCharException<TTokenType>
            Create<TTokenType>(char? actual, TTokenType context, params char[] expected)
            => new UnexpectedCharException<TTokenType>(actual, context, expected);
    }
}
