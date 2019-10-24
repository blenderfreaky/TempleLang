namespace TempleLang.Parser.Abstractions.Exceptions
{
    using Lexer;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TempleLang.Lexer.Abstractions;

    /// <summary>
    /// The exception that is thrown, when a parser encounters an unexpected token
    /// </summary>
    /// <typeparam name="TTokenType"></typeparam>
    [Serializable]
    public class UnexpectedTokenException<TParseTreeNodeType, TToken, TTokenType, TSourceFile> : Exception
        where TToken : IToken<TTokenType, TSourceFile>
        where TSourceFile : ISourceFile
    {
        /// <summary>
        /// Gets the token types that would've been accepted
        /// </summary>
        public IReadOnlyCollection<TTokenType>? Expected { get; }

        /// <summary>
        /// Gets the simplified description of the expectation (i.e. "Compound Assignment" for Expected = [+=, -=, /= ...])
        /// </summary>
        /// <remarks>
        /// Should be able to finish the sentence $"Got {Actual}, expected {ExpectedDescription}"
        /// </remarks>
        public string? ExpectedDescription { get; }

        /// <summary>
        /// Gets the type of parse tree node that was being parsed when the token was encountered
        /// </summary>
        public TParseTreeNodeType Context { get; }

        /// <summary>
        /// Gets the actual read token
        /// </summary>
        public TToken Actual { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedTokenException{TParseTreeNodeType, TToken, TTokenType, TSourceFile}"/> class.
        /// </summary>
        /// <param name="actual">The token that was read</param>
        /// <param name="context">Short description of the context under which the token was encountered</param>
        /// <param name="expectedDescription">A short description of the token types expected to be read, 
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}"</param>
        /// <param name="expected">The full list of all expected token types</param>
        public UnexpectedTokenException(TToken actual, TParseTreeNodeType context, string expectedDescription, params TTokenType[] expected)
            : base($"Got {actual?.ToString() ?? "EoF"}, expected {expectedDescription} ({expected}), while parsing {context}")
        {
            Actual = actual;
            ExpectedDescription = expectedDescription;
            Expected = expected;
            Context = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedTokenException{TParseTreeNodeType, TToken, TTokenType, TSourceFile}"/> class.
        /// </summary>
        /// <param name="actual">The token that was read</param>
        /// <param name="context">Short description of the context under which the token was encountered</param>
        /// <param name="expectedDescription">A short description of the token types expected to be read,
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}"</param>
        public UnexpectedTokenException(TToken actual, TParseTreeNodeType context, string expectedDescription)
            : base($"Got {actual?.ToString() ?? "EoF"}, expected {expectedDescription}, while parsing {context}")
        {
            Actual = actual;
            ExpectedDescription = expectedDescription;
            Context = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedTokenException{TParseTreeNodeType, TToken, TTokenType, TSourceFile}"/> class.
        /// </summary>
        /// <param name="actual">The token that was read</param>
        /// <param name="context">Short description of the context under which the token was encountered</param>
        /// <param name="expected">The full list of all expected token types</param>
        public UnexpectedTokenException(TToken actual, TParseTreeNodeType context, params TTokenType[] expected)
            : base($"Got {actual?.ToString() ?? "EoF"}, expected {expected}, while parsing {context}")
        {
            Actual = actual;
            ExpectedDescription = $"{expected}";
            Expected = expected;
            Context = context;
        }

#nullable disable
        // <inheritdoc/>
        public UnexpectedTokenException() { }

        // <inheritdoc/>
        public UnexpectedTokenException(string message) : base(message) { }

        // <inheritdoc/>
        public UnexpectedTokenException(string message, Exception innerException) : base(message, innerException) { }

        // <inheritdoc/>
        protected UnexpectedTokenException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#nullable restore
    }
}
