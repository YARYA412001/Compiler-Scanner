using System;

namespace SimpleScanner
{
    public enum TokenType
    {
        // Keywords
        IF, THEN, ELSE,
        FOR, WHILE, DO,
        TO, DOWNTO, REPEAT, UNTIL,
        BEGIN, END, VAR, CONST, PROCEDURE, FUNCTION,

        // Identifiers and literals
        ID, NUMBER,

        // Operators
        RELOP, ASSIGN, PLUS, MINUS, MULTIPLY, DIVIDE,

        // Punctuation
        LPAREN, RPAREN, SEMICOLON, COMMA, COLON, DOT,

        // Special
        EOF, UNKNOWN
    }
}