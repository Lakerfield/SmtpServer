﻿using System;
using System.Diagnostics;

namespace SmtpServer.Text
{
    [DebuggerDisplay("[{Kind}] {Text}")]
    public struct Token
    {
        // ReSharper disable InconsistentNaming
        public static readonly Token None = new Token(TokenKind.None);
        public static readonly Token NewLine = new Token(TokenKind.NewLine);
        public static readonly Token CR = new Token(TokenKind.Space, (char)13);
        public static readonly Token LF = new Token(TokenKind.Space, (char)10);
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="kind">The token kind.</param>
        Token(TokenKind kind) : this(kind, String.Empty)
        {
            Kind = kind;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="kind">The token kind.</param>
        /// <param name="text">The token text.</param>
        public Token(TokenKind kind, string text) : this()
        {
            Text = text;
            Kind = kind;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="kind">The token kind.</param>
        /// <param name="ch">The character to create the token from.</param>
        public Token(TokenKind kind, char ch) : this()
        {
            Text = ch.ToString();
            Kind = kind;
        }

        /// <summary>
        /// Create a token for the given character.
        /// </summary>
        /// <param name="ch">The character to create the token for.</param>
        /// <returns>The token that was created.</returns>
        public static Token Create(char ch)
        {
            return Create((byte) ch);
        }

        /// <summary>
        /// Create a token for the given byte value.
        /// </summary>
        /// <param name="b">The byte value to create the token for.</param>
        /// <returns>The token that was created.</returns>
        public static Token Create(byte b)
        {
            return new Token(KindOf(b), (char)b);
        }

        /// <summary>
        /// Returns the token kind for the given byte value.
        /// </summary>
        /// <param name="value">The byte value to return the token kind for.</param>
        /// <returns>The token kind for the given byte value.</returns>
        public static TokenKind KindOf(byte value)
        {
            if (IsText(value))
            {
                return TokenKind.Text;
            }

            if (IsNumber(value))
            {
                return TokenKind.Number;
            }

            if (IsWhiteSpace(value))
            {
                return TokenKind.Space;
            }

            return TokenKind.Other;
        }

        /// <summary>
        /// Returns a value indicating whether or not the given byte is considered a carriage return (CR).
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>true if the value is considered a carriage return (CR) character, false if not.</returns>
        // ReSharper disable once InconsistentNaming
        public static bool IsCR(byte value)
        {
            return value == 13;
        }

        /// <summary>
        /// Returns a value indicating whether or not the given byte is considered a line feed (LF).
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>true if the value is considered a line feed (LF) character, false if not.</returns>
        // ReSharper disable once InconsistentNaming
        public static bool IsLF(byte value)
        {
            return value == 10;
        }

        /// <summary>
        /// Returns a value indicating whether or not the given byte is considered a text character.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>true if the value is considered a text character, false if not.</returns>
        public static bool IsText(byte value)
        {
            return IsBetween(value, 65, 90) || IsBetween(value, 97, 122);
        }

        /// <summary>
        /// Returns a value indicating whether or not the given byte is considered a digit character.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>true if the value is considered a digit character, false if not.</returns>
        public static bool IsNumber(byte value)
        {
            return IsBetween(value, 48, 57);
        }

        /// <summary>
        /// Returns a value indicating whether or not the given byte is considered a whitespace.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>true if the value is considered a whitespace character, false if not.</returns>
        public static bool IsWhiteSpace(byte value)
        {
            return value == 32 || IsBetween(value, 9, 13);
        }

        /// <summary>
        /// Returns a value indicating whether or not the given value is inclusively between a given range.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="low">The lower value of the range.</param>
        /// <param name="high">The higher value of the range.</param>
        /// <returns>true if the value is between the range, false if not.</returns>
        static bool IsBetween(byte value, byte low, byte high)
        {
            return value >= low && value <= high;
        }
        
        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="other">Another object to compare to. </param>
        /// <returns>true if <paramref name="other"/> and this instance are the same type and represent the same value; otherwise, false. </returns>
        public bool Equals(Token other)
        {
            return Kind == other.Kind && String.Equals(Text, other.Text, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to. </param>
        /// <returns>true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false. </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
            {
                return false;
            }

            return obj is Token && Equals((Token)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Text?.GetHashCode() ?? 0) * 397) ^ (int)Kind;
            }
        }

        /// <summary>
        /// Returns a value indicating the equality of the two objects.
        /// </summary>
        /// <param name="left">The left hand side of the comparisson.</param>
        /// <param name="right">The right hand side of the comparisson.</param>
        /// <returns>true if the left and right side are equal, false if not.</returns>
        public static bool operator ==(Token left, Token right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Returns a value indicating the inequality of the two objects.
        /// </summary>
        /// <param name="left">The left hand side of the comparisson.</param>
        /// <param name="right">The right hand side of the comparisson.</param>
        /// <returns>false if the left and right side are equal, true if not.</returns>
        public static bool operator !=(Token left, Token right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns the string representation of the token.
        /// </summary>
        /// <returns>The string representation of the token.</returns>
        public override string ToString()
        {
            return String.Format("[{0}] {1}", Kind, Text);
        }

        /// <summary>
        /// Gets the token text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the token kind.
        /// </summary>
        public TokenKind Kind { get; }
    }
}