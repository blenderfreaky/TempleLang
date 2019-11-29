namespace TempleLang.Parser.Abstractions.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TempleLang.Lexer;

    /// <summary>
    /// The exception that is thrown, when a parser encounters an unexpected token
    /// </summary>
    /// <typeparam name="TToken"></typeparam>
    [Serializable]
    public class UnexpectedLexemeException<TParseTreeNodeType, TToken> : Exception
    {
        /// <summary>
        /// Gets the token types that would've been accepted
        /// </summary>
        public IReadOnlyCollection<TToken>? Expected { get; }

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
        public Lexeme<TToken> Actual { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedLexemeException{TParseTreeNodeType,  TToken}"/> class.
        /// </summary>
        /// <param name="actual">The token that was read</param>
        /// <param name="context">Short description of the context under which the token was encountered</param>
        /// <param name="expectedDescription">A short description of the token types expected to be read,
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}"</param>
        /// <param name="expected">The full list of all expected token types</param>
        public UnexpectedLexemeException(Lexeme<TToken> actual, TParseTreeNodeType context, string expectedDescription, params TToken[] expected)
            : base($"Got {actual.ToString() ?? "EoF"}, expected {expectedDescription} ({expected}), while parsing {context}")
        {
            Actual = actual;
            ExpectedDescription = expectedDescription;
            Expected = expected;
            Context = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedLexemeException{TParseTreeNodeType,  TToken}"/> class.
        /// </summary>
        /// <param name="actual">The token that was read</param>
        /// <param name="context">Short description of the context under which the token was encountered</param>
        /// <param name="expectedDescription">A short description of the token types expected to be read,
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}"</param>
        public UnexpectedLexemeException(Lexeme<TToken> actual, TParseTreeNodeType context, string expectedDescription)
            : base($"Got {actual.ToString() ?? "EoF"}, expected {expectedDescription}, while parsing {context}")
        {
            Actual = actual;
            ExpectedDescription = expectedDescription;
            Context = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedLexemeException{TParseTreeNodeType,  TToken}"/> class.
        /// </summary>
        /// <param name="actual">The token that was read</param>
        /// <param name="context">Short description of the context under which the token was encountered</param>
        /// <param name="expected">The full list of all expected token types</param>
        public UnexpectedLexemeException(Lexeme<TToken> actual, TParseTreeNodeType context, params TToken[] expected)
            : base($"Got {actual.ToString() ?? "EoF"}, expected {expected}, while parsing {context}")
        {
            Actual = actual;
            ExpectedDescription = $"{expected}";
            Expected = expected;
            Context = context;
        }

#nullable disable

        // <inheritdoc/>
        public UnexpectedLexemeException() { }

        // <inheritdoc/>
        public UnexpectedLexemeException(string message) : base(message) { }

        // <inheritdoc/>
        public UnexpectedLexemeException(string message, Exception innerException) : base(message, innerException) { }

        // <inheritdoc/>
        protected UnexpectedLexemeException(SerializationInfo info, StreamingContext context) : base(info, context) { }

#nullable restore
    }
}