namespace TempleLang.Parser.FSharp

open TempleLang.Lexer.Abstractions

type ParserSuccess<'Result, 'Token> = { Result: 'Result; Remaining: 'Token list }

type ParserResult<'Result, 'Token> =
    | Success of ParserSuccess<'Result, 'Token>
    | Failure

type Parser<'Result, 'Token> = 'Token list -> ParserResult<'Result, 'Token>
    
    //static member (->>) (parser : Parser<'ResultA, 'Token>, binder : ('ResultA -> Parser<'ResultB, 'Token>)) = Parser.Bind parser binder

module Parser =
    let Lexemes (lexer : ILexer<'Token>) terminator =
        lexer.LexUntil terminator

    let Result result : Parser<'Result, 'Token> =
        (fun input -> Success { Result = result; Remaining = input })

    let Any : Parser<'Token, 'Token> = (fun input -> 
        match input with
        | [] -> Failure
        | x::xs -> Success { Result = x; Remaining = xs })

    let None : Parser<'Result, 'Token> = fun _ -> Failure

    let Bind (parser : Parser<'T, 'Token>) (binder : (ParserResult<'T, 'Token> -> Parser<'U, 'Token>)) = (fun input -> input |> parser |> binder)
    let inline (->>) (parser, binder) = Bind (parser) (binder)

    let Where parser predicate =
        Bind parser (fun x ->
            match x with
            | Failure -> None
            | Success success ->
                if predicate x then Result success
                else None)

    let WhereChar predicate = Where Any predicate

    let Char char = WhereChar (fun c -> c = char)
    
    let inline (*) (parserA : Parser<'T, 'Token>, parserB : Parser<'T, 'Token>) =
        fun input -> 
            match parserA input with
            | Failure -> Failure
            | Success success -> parserB success.Remaining

    let inline (+) (parserA : Parser<'T, 'Token>, parserB : Parser<'T, 'Token>) =
        fun input -> 
            match parserA input with
                | Success success -> Success success
                | Failure -> parserB input