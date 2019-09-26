namespace TempleLang.Lexer.Abstractions
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Represents a lexer, able to tokenize any <see cref="TextReader"/> to an <see cref="IEnumerable{TTokenType}"/> of TToken
    /// </summary>
    /// <typeparam name="TToken">The Type of the token the lexer returns. Must implement <see cref="IToken{TTokenType}"/></typeparam>
    /// <typeparam name="TTokenType">The Token Type used by the Token</typeparam>
    public interface ILexer<out TToken, out TTokenType>
        where TToken : IToken<TTokenType>
    {
        /// <summary>
        /// Splits the text from the <see cref="TextReader"/> into an <see cref="IEnumerable{TToken}"/>
        /// </summary>
        /// <param name="textReader">The <see cref="TextReader"/> reading the text to tokenize</param>
        /// <returns>The <see cref="IEnumerable{TToken}"/> containing the tokens</returns>
        IEnumerable<TToken> Tokenize(TextReader textReader);
    }
}
