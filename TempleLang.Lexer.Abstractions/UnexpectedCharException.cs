namespace TempleLang.Lexer.Abstractions
{
    using System;

    /// <summary>
    /// The exception that is thrown, when a parser or otherwise text-interpreting program encounters an unexpected character
    /// </summary>
    public class UnexpectedCharException : Exception
    {
        /// <summary>
        /// Returns the chars that would've been accepted
        /// </summary>
        public char[]? Expected { get; }

        /// <summary>
        /// Returns the simplified description of the expectation (i.e. "Digit" for Expected = ['0', '1', ... '9'])
        /// </summary>
        /// <remarks>
        /// Should be able to finish the sentence $"Got {Actual}, expected {ExpectedDescription}"
        /// </remarks>
        public string? ExpectedDescription { get; }

        /// <summary>
        /// Returns the context under which the unexpected char was discovered
        /// </summary>
        /// <remarks>
        /// Should be able to finish the sentence $"Got {Actual}, expected {ExpectedDescription}, while {Context}"
        /// </remarks>
        public string? Context { get; }

        /// <summary>
        /// Returns the actual char gotten
        /// </summary>
        public char? Actual { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedCharException"/> class.
        /// </summary>
        /// <param name="actual">The char that was read. Null if EoF</param>
        /// <param name="expectedDescription">A short description of the chars expected to be read, 
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}"</param>
        /// <param name="expected">The full list of all expected characters</param>
        /// <param name="context">Short description of the context under which the char was encountered,
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}, while {Context}"</param>
        public UnexpectedCharException(char? actual, string expectedDescription, char[] expected, string context) : base($"Got {actual?.ToString() ?? "EoF"}, expected {expectedDescription} ({expected}), while {context}")
        {
            Actual = actual;
            ExpectedDescription = expectedDescription;
            Expected = expected;
            Context = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedCharException"/> class.
        /// </summary>
        /// <param name="actual">The char that was read. Null if EoF</param>
        /// <param name="expectedDescription">A short description of the chars expected to be read,
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}"</param>
        /// <param name="context">Short description of the context under which the char was encountered,
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}, while {Context}"</param>
        public UnexpectedCharException(char? actual, string expectedDescription, string context) : base($"Got {actual?.ToString() ?? "EoF"}, expected {expectedDescription}, while {context}")
        {
            Actual = actual;
            ExpectedDescription = expectedDescription;
            Context = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedCharException"/> class.
        /// </summary>
        /// <param name="actual">The char that was read. Null if EoF</param>
        /// <param name="context">Short description of the context under which the char was encountered,
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}, while {Context}"</param>
        /// <param name="expected">The full list of all expected characters</param>
        public UnexpectedCharException(char? actual, string context, params char[] expected) : base($"Got {actual?.ToString() ?? "EoF"}, expected one of the chars {expected}, while {context}")
        {
            Actual = actual;
            ExpectedDescription = $"one of the chars {expected}";
            Expected = expected;
            Context = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedCharException"/> class.
        /// </summary>
        /// <param name="actual">The char that was read. Null if EoF</param>
        /// <param name="expected">The full list of all expected characters</param>
        /// <param name="context">Short description of the context under which the char was encountered,
        /// finishing the sentence $"Got {Actual}, expected {ExpectedDescription}, while {Context}"</param>
        public UnexpectedCharException(char? actual, char expected, string context) : base($"Got {actual?.ToString() ?? "EoF"}, expected the char {expected}, while {context}")
        {
            Actual = actual;
            ExpectedDescription = $"the char {expected}";
            Expected = new char[] { expected };
            Context = context;
        }

        /// <inheritdoc/>
        public UnexpectedCharException() : base()
        {
        }

        /// <inheritdoc/>
        public UnexpectedCharException(string message) : base(message)
        {
        }

        /// <inheritdoc/>
        public UnexpectedCharException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
