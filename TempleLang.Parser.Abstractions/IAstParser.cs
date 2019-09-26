namespace TempleLang.Parser.Abstractions
{
    using TempleLang.Lexer.Abstractions;

    /// <summary>
    /// Represents a parser, able to parse a <see cref="TAstNode"/> of any given <see cref="TAstNodeType"/>
    /// </summary>    
    /// <typeparam name="TTerminalAstNode">The type to use to represent terminal AST Nodes</typeparam>
    /// <typeparam name="TAstNode">The type to use to represent non-terminal AST Nodes</typeparam>
    /// <typeparam name="TAstNodeType">The </typeparam>
    /// <typeparam name="TToken"></typeparam>
    /// <typeparam name="TTokenType"></typeparam>
    public interface IAstParser<out TTerminalAstNode, out TAstNode, in TAstNodeType, in TToken, in TTokenType, in TFile>
        where TTerminalAstNode : TAstNode, IAstNodeTerminal<TAstNode, TAstNodeType, TToken, TTokenType, TFile>
        where TAstNode : class, IAstNode<TAstNode, TAstNodeType>
        where TToken : IToken<TTokenType, TFile>
        where TFile : IFile
    {
        /// <summary>
        /// Parses one non-terminal AST Node of the given type
        /// </summary>
        /// <param name="nodeType">The type of AST Node to parse</param>
        /// <returns>The parsed AST Node of the given type</returns>
        TAstNode Parse(TAstNodeType nodeType);

        /// <summary>
        /// Parses one terminal AST Node of the given token type. Throws if the given token type isn't the next in the token-stream
        /// </summary>
        /// <param name="tokenType">The token type to match</param>
        /// <returns>The token as a terminal AST Node</returns>
        TTerminalAstNode ParseTerminal(TTokenType tokenType);
    }
}
