using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using TempleLang.Lexer.Abstractions;

namespace TempleLang.Parser.Abstractions
{
    public readonly class ParserBuilder<TResult, TToken, TTokenType, TSourceFile>
        where TToken : IToken<TTokenType, TSourceFile>
        where TSourceFile : ISourceFile
    {
        public string Name { get; }

        public static IParser<TResult, TToken, TTokenType, TSourceFile> Build()
        {
            DynamicMethod method = new DynamicMethod("Parse", typeof(IParserResult<TResult>),
                new [] { typeof(IParserInput<TToken, TTokenType, TSourceFile>) });
            var  ilGenerator = method.GetILGenerator();


            ilGenerator.DeclareLocal(typeof(int));
        }
    }

    public readonly struct ParserDeconstructionBuilder<TToken, TTokenType, TSourceFile>
        where TToken : IToken<TTokenType, TSourceFile>
        where TSourceFile : ISourceFile
    {
        public readonly bool IsMany { get; }
        public readonly bool IsOptional { get; }

        public readonly bool IsTerminal { get; }
        public readonly TToken Token { get; }
#pragma warning disable CA1819 // Properties should not return arrays
        public readonly ParserDeconstructionBuilder<TToken, TTokenType, TSourceFile>[]? Deconstruction { get; }
#pragma warning restore CA1819 // Properties should not return arrays
        
        public ParserDeconstructionBuilder(TToken token)
        {
            IsMany = false;
            IsOptional = false;
            IsTerminal = true;
            Token = token;
            Deconstruction = null;
        }

        public ParserDeconstructionBuilder(bool isMany, bool isOptional, params ParserDeconstructionBuilder<TToken, TTokenType, TSourceFile>[] deconstruction)
        {
            IsMany = isMany;
            IsOptional = isOptional;
            IsTerminal = false;
            Token = default!;
            Deconstruction = deconstruction;
        }

        private static T[] AppendOne<T>(T[] source, T newElement)
        {
            int length = source.Length;
            T[] target = new T[length + 1];
            Buffer.BlockCopy(source, 0, target, 0, length);
            target[length] = newElement;
            return target;
        }
    }

    public static class Parser
    {
        public static ParserBuilder<TFinalResult, TToken, TTokenType, TSourceFile>
            Or<TFinalResult, TIntermediateResult, TToken, TTokenType, TSourceFile>(
                params ParserBuilder<TIntermediateResult, TToken, TTokenType, TSourceFile>[] options)
            where TToken : IToken<TTokenType, TSourceFile>
            where TSourceFile : ISourceFile
        {
            return new ParserBuilder<TFinalResult, TToken, TTokenType, TSourceFile>()
        }
    }

    public interface IParserResult
    {
        bool Success { get; }
    }

    public interface IParserResult<TResult> : IParserResult
    {
        TResult Result { get; }
    }

    public interface IParser<TResult, in TToken, in TTokenType, in TSourceFile>
        where TToken : IToken<TTokenType, TSourceFile>
        where TSourceFile : ISourceFile
    {
        IParserResult<TResult> Parse(IParserInput<TToken, TTokenType, TSourceFile> input);
    }
}
