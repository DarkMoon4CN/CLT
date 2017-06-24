namespace LitJson
{
    using System;

    public class JsonException : ApplicationException
    {
        public JsonException()
        {
        }

        internal JsonException(ParserToken token) : base(string.Format("Invalid token '{0}' in input string", token))
        {
        }

        internal JsonException(int c) : base(string.Format("Invalid character '{0}' in input string", (char) c))
        {
        }

        public JsonException(string message) : base(message)
        {
        }

        internal JsonException(ParserToken token, Exception inner_exception) : base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
        {
        }

        internal JsonException(int c, Exception inner_exception) : base(string.Format("Invalid character '{0}' in input string", (char) c), inner_exception)
        {
        }

        public JsonException(string message, Exception inner_exception) : base(message, inner_exception)
        {
        }
    }
}

