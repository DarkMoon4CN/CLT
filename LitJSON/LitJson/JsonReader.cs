﻿namespace LitJson
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class JsonReader
    {
        private Stack<int> automaton_stack;
        private int current_input;
        private int current_symbol;
        private bool end_of_input;
        private bool end_of_json;
        private Lexer lexer;
        private static IDictionary<int, IDictionary<int, int[]>> parse_table;
        private bool parser_in_string;
        private bool parser_return;
        private bool read_started;
        private TextReader reader;
        private bool reader_is_owned;
        private JsonToken token;
        private object token_value;

        static JsonReader()
        {
            PopulateParseTable();
        }

        public JsonReader(TextReader reader) : this(reader, false)
        {
        }

        public JsonReader(string json_text) : this(new StringReader(json_text), true)
        {
        }

        private JsonReader(TextReader reader, bool owned)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            this.parser_in_string = false;
            this.parser_return = false;
            this.read_started = false;
            this.automaton_stack = new Stack<int>();
            this.automaton_stack.Push(0x10011);
            this.automaton_stack.Push(0x10007);
            this.lexer = new Lexer(reader);
            this.end_of_input = false;
            this.end_of_json = false;
            this.reader = reader;
            this.reader_is_owned = owned;
        }

        public void Close()
        {
            if (!this.end_of_input)
            {
                this.end_of_input = true;
                this.end_of_json = true;
                if (this.reader_is_owned)
                {
                    this.reader.Close();
                }
                this.reader = null;
            }
        }

        private static void PopulateParseTable()
        {
            parse_table = new Dictionary<int, IDictionary<int, int[]>>();
            TableAddRow(ParserToken.Array);
            TableAddCol(ParserToken.Array, 0x5b, new int[] { 0x5b, 0x1000d });
            TableAddRow(ParserToken.ArrayPrime);
            TableAddCol(ParserToken.ArrayPrime, 0x22, new int[] { 0x1000e, 0x1000f, 0x5d });
            TableAddCol(ParserToken.ArrayPrime, 0x5b, new int[] { 0x1000e, 0x1000f, 0x5d });
            TableAddCol(ParserToken.ArrayPrime, 0x5d, new int[] { 0x5d });
            TableAddCol(ParserToken.ArrayPrime, 0x7b, new int[] { 0x1000e, 0x1000f, 0x5d });
            TableAddCol(ParserToken.ArrayPrime, 0x10001, new int[] { 0x1000e, 0x1000f, 0x5d });
            TableAddCol(ParserToken.ArrayPrime, 0x10002, new int[] { 0x1000e, 0x1000f, 0x5d });
            TableAddCol(ParserToken.ArrayPrime, 0x10003, new int[] { 0x1000e, 0x1000f, 0x5d });
            TableAddCol(ParserToken.ArrayPrime, 0x10004, new int[] { 0x1000e, 0x1000f, 0x5d });
            TableAddRow(ParserToken.Object);
            TableAddCol(ParserToken.Object, 0x7b, new int[] { 0x7b, 0x10009 });
            TableAddRow(ParserToken.ObjectPrime);
            TableAddCol(ParserToken.ObjectPrime, 0x22, new int[] { 0x1000a, 0x1000b, 0x7d });
            TableAddCol(ParserToken.ObjectPrime, 0x7d, new int[] { 0x7d });
            TableAddRow(ParserToken.Pair);
            TableAddCol(ParserToken.Pair, 0x22, new int[] { 0x10010, 0x3a, 0x1000e });
            TableAddRow(ParserToken.PairRest);
            TableAddCol(ParserToken.PairRest, 0x2c, new int[] { 0x2c, 0x1000a, 0x1000b });
            TableAddCol(ParserToken.PairRest, 0x7d, new int[] { 0x10012 });
            TableAddRow(ParserToken.String);
            TableAddCol(ParserToken.String, 0x22, new int[] { 0x22, 0x10005, 0x22 });
            TableAddRow(ParserToken.Text);
            TableAddCol(ParserToken.Text, 0x5b, new int[] { 0x1000c });
            TableAddCol(ParserToken.Text, 0x7b, new int[] { 0x10008 });
            TableAddRow(ParserToken.Value);
            TableAddCol(ParserToken.Value, 0x22, new int[] { 0x10010 });
            TableAddCol(ParserToken.Value, 0x5b, new int[] { 0x1000c });
            TableAddCol(ParserToken.Value, 0x7b, new int[] { 0x10008 });
            TableAddCol(ParserToken.Value, 0x10001, new int[] { 0x10001 });
            TableAddCol(ParserToken.Value, 0x10002, new int[] { 0x10002 });
            TableAddCol(ParserToken.Value, 0x10003, new int[] { 0x10003 });
            TableAddCol(ParserToken.Value, 0x10004, new int[] { 0x10004 });
            TableAddRow(ParserToken.ValueRest);
            TableAddCol(ParserToken.ValueRest, 0x2c, new int[] { 0x2c, 0x1000e, 0x1000f });
            TableAddCol(ParserToken.ValueRest, 0x5d, new int[] { 0x10012 });
        }

        private void ProcessNumber(string number)
        {
            double num;
            if ((((number.IndexOf('.') != -1) || (number.IndexOf('e') != -1)) || (number.IndexOf('E') != -1)) && double.TryParse(number, out num))
            {
                this.token = JsonToken.Double;
                this.token_value = num;
            }
            else
            {
                int num2;
                if (int.TryParse(number, out num2))
                {
                    this.token = JsonToken.Int;
                    this.token_value = num2;
                }
                else
                {
                    long num3;
                    if (long.TryParse(number, out num3))
                    {
                        this.token = JsonToken.Long;
                        this.token_value = num3;
                    }
                    else
                    {
                        this.token = JsonToken.Int;
                        this.token_value = 0;
                    }
                }
            }
        }

        private void ProcessSymbol()
        {
            if (this.current_symbol == 0x5b)
            {
                this.token = JsonToken.ArrayStart;
                this.parser_return = true;
            }
            else if (this.current_symbol == 0x5d)
            {
                this.token = JsonToken.ArrayEnd;
                this.parser_return = true;
            }
            else if (this.current_symbol == 0x7b)
            {
                this.token = JsonToken.ObjectStart;
                this.parser_return = true;
            }
            else if (this.current_symbol == 0x7d)
            {
                this.token = JsonToken.ObjectEnd;
                this.parser_return = true;
            }
            else if (this.current_symbol == 0x22)
            {
                if (this.parser_in_string)
                {
                    this.parser_in_string = false;
                    this.parser_return = true;
                }
                else
                {
                    if (this.token == JsonToken.None)
                    {
                        this.token = JsonToken.String;
                    }
                    this.parser_in_string = true;
                }
            }
            else if (this.current_symbol == 0x10005)
            {
                this.token_value = this.lexer.StringValue;
            }
            else if (this.current_symbol == 0x10003)
            {
                this.token = JsonToken.Boolean;
                this.token_value = false;
                this.parser_return = true;
            }
            else if (this.current_symbol == 0x10004)
            {
                this.token = JsonToken.Null;
                this.parser_return = true;
            }
            else if (this.current_symbol == 0x10001)
            {
                this.ProcessNumber(this.lexer.StringValue);
                this.parser_return = true;
            }
            else if (this.current_symbol == 0x1000a)
            {
                this.token = JsonToken.PropertyName;
            }
            else if (this.current_symbol == 0x10002)
            {
                this.token = JsonToken.Boolean;
                this.token_value = true;
                this.parser_return = true;
            }
        }

        public bool Read()
        {
            if (this.end_of_input)
            {
                return false;
            }
            if (this.end_of_json)
            {
                this.end_of_json = false;
                this.automaton_stack.Clear();
                this.automaton_stack.Push(0x10011);
                this.automaton_stack.Push(0x10007);
            }
            this.parser_in_string = false;
            this.parser_return = false;
            this.token = JsonToken.None;
            this.token_value = null;
            if (!this.read_started)
            {
                this.read_started = true;
                if (!this.ReadToken())
                {
                    return false;
                }
            }
            while (true)
            {
                if (this.parser_return)
                {
                    if (this.automaton_stack.Peek() == 0x10011)
                    {
                        this.end_of_json = true;
                    }
                    return true;
                }
                this.current_symbol = this.automaton_stack.Pop();
                this.ProcessSymbol();
                if (this.current_symbol == this.current_input)
                {
                    if (!this.ReadToken())
                    {
                        if (this.automaton_stack.Peek() != 0x10011)
                        {
                            throw new JsonException("Input doesn't evaluate to proper JSON text");
                        }
                        return this.parser_return;
                    }
                }
                else
                {
                    int[] numArray;
                    try
                    {
                        numArray = parse_table[this.current_symbol][this.current_input];
                    }
                    catch (KeyNotFoundException exception)
                    {
                        throw new JsonException((ParserToken) this.current_input, exception);
                    }
                    if (numArray[0] != 0x10012)
                    {
                        for (int i = numArray.Length - 1; i >= 0; i--)
                        {
                            this.automaton_stack.Push(numArray[i]);
                        }
                    }
                }
            }
        }

        private bool ReadToken()
        {
            if (this.end_of_input)
            {
                return false;
            }
            this.lexer.NextToken();
            if (this.lexer.EndOfInput)
            {
                this.Close();
                return false;
            }
            this.current_input = this.lexer.Token;
            return true;
        }

        private static void TableAddCol(ParserToken row, int col, params int[] symbols)
        {
            parse_table[(int) row].Add(col, symbols);
        }

        private static void TableAddRow(ParserToken rule)
        {
            parse_table.Add((int) rule, new Dictionary<int, int[]>());
        }

        public bool AllowComments
        {
            get
            {
                return this.lexer.AllowComments;
            }
            set
            {
                this.lexer.AllowComments = value;
            }
        }

        public bool AllowSingleQuotedStrings
        {
            get
            {
                return this.lexer.AllowSingleQuotedStrings;
            }
            set
            {
                this.lexer.AllowSingleQuotedStrings = value;
            }
        }

        public bool EndOfInput
        {
            get
            {
                return this.end_of_input;
            }
        }

        public bool EndOfJson
        {
            get
            {
                return this.end_of_json;
            }
        }

        public JsonToken Token
        {
            get
            {
                return this.token;
            }
        }

        public object Value
        {
            get
            {
                return this.token_value;
            }
        }
    }
}

