using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleScanner
{
    public class Scanner
    {
        private readonly string _input;
        private int _position;
        private int _line;
        private int _column;
        private readonly Dictionary<string, TokenType> _keywords;

        public Scanner(string input)
        {
            _input = input;
            _position = 0;
            _line = 1;
            _column = 1;

            _keywords = new Dictionary<string, TokenType>
            {
                // Conditional keywords
                { "if", TokenType.IF },
                { "then", TokenType.THEN },
                { "else", TokenType.ELSE },
                
                // Loop keywords
                { "for", TokenType.FOR },
                { "while", TokenType.WHILE },
                { "do", TokenType.DO },
                { "to", TokenType.TO },
                { "downto", TokenType.DOWNTO },
                { "repeat", TokenType.REPEAT },
                { "until", TokenType.UNTIL },
                
                // Structure keywords
                { "begin", TokenType.BEGIN },
                { "end", TokenType.END },
                { "var", TokenType.VAR },
                { "const", TokenType.CONST },
                { "procedure", TokenType.PROCEDURE },
                { "function", TokenType.FUNCTION }
            };
        }

        private char CurrentChar()
        {
            if (_position >= _input.Length)
                return '\0';
            return _input[_position];
        }

        private void Advance()
        {
            if (_position < _input.Length)
            {
                if (CurrentChar() == '\n')
                {
                    _line++;
                    _column = 1;
                }
                else
                {
                    _column++;
                }
                _position++;
            }
        }

        private void SkipWhitespace()
        {
            while (_position < _input.Length && char.IsWhiteSpace(CurrentChar()))
            {
                Advance();
            }
        }

        private Token ScanNumber(int line, int column)
        {
            var sb = new StringBuilder();

            // Read integer part
            while (_position < _input.Length && char.IsDigit(CurrentChar()))
            {
                sb.Append(CurrentChar());
                Advance();
            }

            // Check for decimal point
            if (_position < _input.Length && CurrentChar() == '.')
            {
                sb.Append(CurrentChar());
                Advance();

                // Read decimal part
                while (_position < _input.Length && char.IsDigit(CurrentChar()))
                {
                    sb.Append(CurrentChar());
                    Advance();
                }
            }

            return new Token(TokenType.NUMBER, sb.ToString(), line, column);
        }

        private Token ScanIdentifierOrKeyword(int line, int column)
        {
            var sb = new StringBuilder();

            if (char.IsLetter(CurrentChar()))
            {
                sb.Append(CurrentChar());
                Advance();
            }

            while (_position < _input.Length &&
                   (char.IsLetterOrDigit(CurrentChar()) || CurrentChar() == '_'))
            {
                sb.Append(CurrentChar());
                Advance();
            }

            string value = sb.ToString();

            if (_keywords.ContainsKey(value.ToLower()))
                return new Token(_keywords[value.ToLower()], value, line, column);

            return new Token(TokenType.ID, value, line, column);
        }

        private Token ScanRelop(int line, int column)
        {
            char first = CurrentChar();
            Advance();

            if (_position < _input.Length)
            {
                char second = CurrentChar();
                string combined = $"{first}{second}";

                switch (combined)
                {
                    case "<=":
                    case ">=":
                    case "<>":
                        Advance();
                        return new Token(TokenType.RELOP, combined, line, column);
                    case ":=":
                        Advance();
                        return new Token(TokenType.ASSIGN, combined, line, column);
                }
            }

            if (first == '<' || first == '>' || first == '=')
                return new Token(TokenType.RELOP, first.ToString(), line, column);

            if (first == ':')
                return new Token(TokenType.COLON, ":", line, column);

            return new Token(TokenType.UNKNOWN, first.ToString(), line, column);
        }

        private bool IsRelopStart(char c)
        {
            return c == '<' || c == '>' || c == '=';
        }

        private Token ScanSingleCharacter(char current, int line, int column)
        {
            TokenType type = TokenType.UNKNOWN;

            switch (current)
            {
                case '+': type = TokenType.PLUS; break;
                case '-': type = TokenType.MINUS; break;
                case '*': type = TokenType.MULTIPLY; break;
                case '/': type = TokenType.DIVIDE; break;
                case '(': type = TokenType.LPAREN; break;
                case ')': type = TokenType.RPAREN; break;
                case ';': type = TokenType.SEMICOLON; break;
                case ',': type = TokenType.COMMA; break;
                case '.': type = TokenType.DOT; break;
            }

            Advance();
            return new Token(type, current.ToString(), line, column);
        }

        public Token GetNextToken()
        {
            SkipWhitespace();

            if (_position >= _input.Length)
                return new Token(TokenType.EOF, "", _line, _column);

            char current = CurrentChar();
            int startLine = _line;
            int startColumn = _column;

            if (char.IsLetter(current))
                return ScanIdentifierOrKeyword(startLine, startColumn);

            if (char.IsDigit(current))
                return ScanNumber(startLine, startColumn);

            if (current == ':')
                return ScanRelop(startLine, startColumn);

            if (IsRelopStart(current))
                return ScanRelop(startLine, startColumn);

            return ScanSingleCharacter(current, startLine, startColumn);
        }

        public List<Token> ScanAll()
        {
            var tokens = new List<Token>();
            Token token;

            do
            {
                token = GetNextToken();
                tokens.Add(token);
            } while (token.Type != TokenType.EOF);

            return tokens;
        }
    }
}