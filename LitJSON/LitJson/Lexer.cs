namespace LitJson
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal class Lexer
    {
        private bool allow_comments = true;
        private bool allow_single_quoted_strings = true;
        private bool end_of_input = false;
        private FsmContext fsm_context;
        private static StateHandler[] fsm_handler_table;
        private static int[] fsm_return_table;
        private int input_buffer = 0;
        private int input_char;
        private TextReader reader;
        private int state = 1;
        private StringBuilder string_buffer = new StringBuilder(0x80);
        private string string_value;
        private int token;
        private int unichar;

        static Lexer()
        {
            PopulateFsmTables();
        }

        public Lexer(TextReader reader)
        {
            this.reader = reader;
            this.fsm_context = new FsmContext();
            this.fsm_context.L = this;
        }

        private bool GetChar()
        {
            this.input_char = this.NextChar();
            if (this.input_char != -1)
            {
                return true;
            }
            this.end_of_input = true;
            return false;
        }

        private static int HexValue(int digit)
        {
            switch (digit)
            {
                case 0x41:
                case 0x61:
                    return 10;

                case 0x42:
                case 0x62:
                    return 11;

                case 0x43:
                case 0x63:
                    return 12;

                case 0x44:
                case 100:
                    return 13;

                case 0x45:
                case 0x65:
                    return 14;

                case 70:
                case 0x66:
                    return 15;
            }
            return (digit - 0x30);
        }

        private int NextChar()
        {
            if (this.input_buffer != 0)
            {
                int num = this.input_buffer;
                this.input_buffer = 0;
                return num;
            }
            return this.reader.Read();
        }

        public bool NextToken()
        {
            this.fsm_context.Return = false;
            while (true)
            {
                StateHandler handler = fsm_handler_table[this.state - 1];
                if (!handler(this.fsm_context))
                {
                    throw new JsonException(this.input_char);
                }
                if (this.end_of_input)
                {
                    return false;
                }
                if (this.fsm_context.Return)
                {
                    this.string_value = this.string_buffer.ToString();
                    this.string_buffer.Remove(0, this.string_buffer.Length);
                    this.token = fsm_return_table[this.state - 1];
                    if (this.token == 0x10006)
                    {
                        this.token = this.input_char;
                    }
                    this.state = this.fsm_context.NextState;
                    return true;
                }
                this.state = this.fsm_context.NextState;
            }
        }

        private static void PopulateFsmTables()
        {
            fsm_handler_table = new StateHandler[] { 
                new StateHandler(Lexer.State1), new StateHandler(Lexer.State2), new StateHandler(Lexer.State3), new StateHandler(Lexer.State4), new StateHandler(Lexer.State5), new StateHandler(Lexer.State6), new StateHandler(Lexer.State7), new StateHandler(Lexer.State8), new StateHandler(Lexer.State9), new StateHandler(Lexer.State10), new StateHandler(Lexer.State11), new StateHandler(Lexer.State12), new StateHandler(Lexer.State13), new StateHandler(Lexer.State14), new StateHandler(Lexer.State15), new StateHandler(Lexer.State16), 
                new StateHandler(Lexer.State17), new StateHandler(Lexer.State18), new StateHandler(Lexer.State19), new StateHandler(Lexer.State20), new StateHandler(Lexer.State21), new StateHandler(Lexer.State22), new StateHandler(Lexer.State23), new StateHandler(Lexer.State24), new StateHandler(Lexer.State25), new StateHandler(Lexer.State26), new StateHandler(Lexer.State27), new StateHandler(Lexer.State28)
             };
            fsm_return_table = new int[] { 
                0x10006, 0, 0x10001, 0x10001, 0, 0x10001, 0, 0x10001, 0, 0, 0x10002, 0, 0, 0, 0x10003, 0, 
                0, 0x10004, 0x10005, 0x10006, 0, 0, 0x10005, 0x10006, 0, 0, 0, 0
             };
        }

        private static char ProcessEscChar(int esc_char)
        {
            switch (esc_char)
            {
                case 0x2f:
                case 0x5c:
                case 0x22:
                case 0x27:
                    return Convert.ToChar(esc_char);

                case 0x62:
                    return '\b';

                case 0x66:
                    return '\f';

                case 0x72:
                    return '\r';

                case 0x74:
                    return '\t';

                case 110:
                    return '\n';
            }
            return '?';
        }

        private static bool State1(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if ((ctx.L.input_char == 0x20) || ((ctx.L.input_char >= 9) && (ctx.L.input_char <= 13)))
                {
                    continue;
                }
                if ((ctx.L.input_char >= 0x31) && (ctx.L.input_char <= 0x39))
                {
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    ctx.NextState = 3;
                    return true;
                }
                switch (ctx.L.input_char)
                {
                    case 0x2c:
                    case 0x3a:
                    case 0x5b:
                    case 0x5d:
                    case 0x7b:
                    case 0x7d:
                        ctx.NextState = 1;
                        ctx.Return = true;
                        return true;

                    case 0x2d:
                        ctx.L.string_buffer.Append((char) ctx.L.input_char);
                        ctx.NextState = 2;
                        return true;

                    case 0x2f:
                        if (ctx.L.allow_comments)
                        {
                            break;
                        }
                        return false;

                    case 0x30:
                        ctx.L.string_buffer.Append((char) ctx.L.input_char);
                        ctx.NextState = 4;
                        return true;

                    case 0x22:
                        ctx.NextState = 0x13;
                        ctx.Return = true;
                        return true;

                    case 0x27:
                        if (!ctx.L.allow_single_quoted_strings)
                        {
                            return false;
                        }
                        ctx.L.input_char = 0x22;
                        ctx.NextState = 0x17;
                        ctx.Return = true;
                        return true;

                    case 0x66:
                        ctx.NextState = 12;
                        return true;

                    case 0x74:
                        ctx.NextState = 9;
                        return true;

                    case 110:
                        ctx.NextState = 0x10;
                        return true;

                    default:
                        return false;
                }
                ctx.NextState = 0x19;
                return true;
            }
            return true;
        }

        private static bool State10(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char == 0x75)
            {
                ctx.NextState = 11;
                return true;
            }
            return false;
        }

        private static bool State11(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char == 0x65)
            {
                ctx.Return = true;
                ctx.NextState = 1;
                return true;
            }
            return false;
        }

        private static bool State12(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char == 0x61)
            {
                ctx.NextState = 13;
                return true;
            }
            return false;
        }

        private static bool State13(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char == 0x6c)
            {
                ctx.NextState = 14;
                return true;
            }
            return false;
        }

        private static bool State14(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char == 0x73)
            {
                ctx.NextState = 15;
                return true;
            }
            return false;
        }

        private static bool State15(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char == 0x65)
            {
                ctx.Return = true;
                ctx.NextState = 1;
                return true;
            }
            return false;
        }

        private static bool State16(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char == 0x75)
            {
                ctx.NextState = 0x11;
                return true;
            }
            return false;
        }

        private static bool State17(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char == 0x6c)
            {
                ctx.NextState = 0x12;
                return true;
            }
            return false;
        }

        private static bool State18(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char == 0x6c)
            {
                ctx.Return = true;
                ctx.NextState = 1;
                return true;
            }
            return false;
        }

        private static bool State19(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                switch (ctx.L.input_char)
                {
                    case 0x22:
                        ctx.L.UngetChar();
                        ctx.Return = true;
                        ctx.NextState = 20;
                        return true;

                    case 0x5c:
                        ctx.StateStack = 0x13;
                        ctx.NextState = 0x15;
                        return true;
                }
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
            }
            return true;
        }

        private static bool State2(FsmContext ctx)
        {
            ctx.L.GetChar();
            if ((ctx.L.input_char >= 0x31) && (ctx.L.input_char <= 0x39))
            {
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 3;
                return true;
            }
            if (ctx.L.input_char == 0x30)
            {
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 4;
                return true;
            }
            return false;
        }

        private static bool State20(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char == 0x22)
            {
                ctx.Return = true;
                ctx.NextState = 1;
                return true;
            }
            return false;
        }

        private static bool State21(FsmContext ctx)
        {
            ctx.L.GetChar();
            switch (ctx.L.input_char)
            {
                case 0x2f:
                case 0x5c:
                case 0x22:
                case 0x27:
                case 0x62:
                case 0x66:
                case 0x72:
                case 0x74:
                case 110:
                    ctx.L.string_buffer.Append(ProcessEscChar(ctx.L.input_char));
                    ctx.NextState = ctx.StateStack;
                    return true;

                case 0x75:
                    ctx.NextState = 0x16;
                    return true;
            }
            return false;
        }

        private static bool State22(FsmContext ctx)
        {
            int num = 0;
            int num2 = 0x1000;
            ctx.L.unichar = 0;
            while (ctx.L.GetChar())
            {
                if ((((ctx.L.input_char < 0x30) || (ctx.L.input_char > 0x39)) && ((ctx.L.input_char < 0x41) || (ctx.L.input_char > 70))) && ((ctx.L.input_char < 0x61) || (ctx.L.input_char > 0x66)))
                {
                    return false;
                }
                ctx.L.unichar += HexValue(ctx.L.input_char) * num2;
                num++;
                num2 /= 0x10;
                if (num == 4)
                {
                    ctx.L.string_buffer.Append(Convert.ToChar(ctx.L.unichar));
                    ctx.NextState = ctx.StateStack;
                    return true;
                }
            }
            return true;
        }

        private static bool State23(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                switch (ctx.L.input_char)
                {
                    case 0x27:
                        ctx.L.UngetChar();
                        ctx.Return = true;
                        ctx.NextState = 0x18;
                        return true;

                    case 0x5c:
                        ctx.StateStack = 0x17;
                        ctx.NextState = 0x15;
                        return true;
                }
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
            }
            return true;
        }

        private static bool State24(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char == 0x27)
            {
                ctx.L.input_char = 0x22;
                ctx.Return = true;
                ctx.NextState = 1;
                return true;
            }
            return false;
        }

        private static bool State25(FsmContext ctx)
        {
            ctx.L.GetChar();
            switch (ctx.L.input_char)
            {
                case 0x2a:
                    ctx.NextState = 0x1b;
                    return true;

                case 0x2f:
                    ctx.NextState = 0x1a;
                    return true;
            }
            return false;
        }

        private static bool State26(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if (ctx.L.input_char == 10)
                {
                    ctx.NextState = 1;
                    return true;
                }
            }
            return true;
        }

        private static bool State27(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if (ctx.L.input_char == 0x2a)
                {
                    ctx.NextState = 0x1c;
                    return true;
                }
            }
            return true;
        }

        private static bool State28(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if (ctx.L.input_char != 0x2a)
                {
                    if (ctx.L.input_char == 0x2f)
                    {
                        ctx.NextState = 1;
                        return true;
                    }
                    ctx.NextState = 0x1b;
                    return true;
                }
            }
            return true;
        }

        private static bool State3(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if ((ctx.L.input_char >= 0x30) && (ctx.L.input_char <= 0x39))
                {
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    continue;
                }
                if ((ctx.L.input_char == 0x20) || ((ctx.L.input_char >= 9) && (ctx.L.input_char <= 13)))
                {
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;
                }
                int num = ctx.L.input_char;
                if (num <= 0x45)
                {
                    switch (num)
                    {
                        case 0x2c:
                            goto Label_00BE;

                        case 0x2d:
                            goto Label_0125;

                        case 0x2e:
                            ctx.L.string_buffer.Append((char) ctx.L.input_char);
                            ctx.NextState = 5;
                            return true;

                        case 0x45:
                            goto Label_00FF;
                    }
                    goto Label_0125;
                }
                if (num != 0x5d)
                {
                    if (num == 0x65)
                    {
                        goto Label_00FF;
                    }
                    if (num != 0x7d)
                    {
                        goto Label_0125;
                    }
                }
            Label_00BE:
                ctx.L.UngetChar();
                ctx.Return = true;
                ctx.NextState = 1;
                return true;
            Label_00FF:
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 7;
                return true;
            Label_0125:
                return false;
            }
            return true;
        }

        private static bool State4(FsmContext ctx)
        {
            ctx.L.GetChar();
            if ((ctx.L.input_char == 0x20) || ((ctx.L.input_char >= 9) && (ctx.L.input_char <= 13)))
            {
                ctx.Return = true;
                ctx.NextState = 1;
                return true;
            }
            int num = ctx.L.input_char;
            if (num <= 0x45)
            {
                switch (num)
                {
                    case 0x2c:
                        goto Label_0085;

                    case 0x2d:
                        goto Label_00EC;

                    case 0x2e:
                        ctx.L.string_buffer.Append((char) ctx.L.input_char);
                        ctx.NextState = 5;
                        return true;

                    case 0x45:
                        goto Label_00C6;
                }
                goto Label_00EC;
            }
            if (num != 0x5d)
            {
                if (num == 0x65)
                {
                    goto Label_00C6;
                }
                if (num != 0x7d)
                {
                    goto Label_00EC;
                }
            }
        Label_0085:
            ctx.L.UngetChar();
            ctx.Return = true;
            ctx.NextState = 1;
            return true;
        Label_00C6:
            ctx.L.string_buffer.Append((char) ctx.L.input_char);
            ctx.NextState = 7;
            return true;
        Label_00EC:
            return false;
        }

        private static bool State5(FsmContext ctx)
        {
            ctx.L.GetChar();
            if ((ctx.L.input_char >= 0x30) && (ctx.L.input_char <= 0x39))
            {
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 6;
                return true;
            }
            return false;
        }

        private static bool State6(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if ((ctx.L.input_char >= 0x30) && (ctx.L.input_char <= 0x39))
                {
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    continue;
                }
                if ((ctx.L.input_char == 0x20) || ((ctx.L.input_char >= 9) && (ctx.L.input_char <= 13)))
                {
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;
                }
                int num = ctx.L.input_char;
                if (num <= 0x45)
                {
                    switch (num)
                    {
                        case 0x2c:
                            goto Label_00AE;

                        case 0x45:
                            goto Label_00C9;
                    }
                    goto Label_00EF;
                }
                if (num != 0x5d)
                {
                    if (num == 0x65)
                    {
                        goto Label_00C9;
                    }
                    if (num != 0x7d)
                    {
                        goto Label_00EF;
                    }
                }
            Label_00AE:
                ctx.L.UngetChar();
                ctx.Return = true;
                ctx.NextState = 1;
                return true;
            Label_00C9:
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 7;
                return true;
            Label_00EF:
                return false;
            }
            return true;
        }

        private static bool State7(FsmContext ctx)
        {
            ctx.L.GetChar();
            if ((ctx.L.input_char >= 0x30) && (ctx.L.input_char <= 0x39))
            {
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 8;
                return true;
            }
            switch (ctx.L.input_char)
            {
                case 0x2b:
                case 0x2d:
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    ctx.NextState = 8;
                    return true;
            }
            return false;
        }

        private static bool State8(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if ((ctx.L.input_char >= 0x30) && (ctx.L.input_char <= 0x39))
                {
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                }
                else
                {
                    if ((ctx.L.input_char == 0x20) || ((ctx.L.input_char >= 9) && (ctx.L.input_char <= 13)))
                    {
                        ctx.Return = true;
                        ctx.NextState = 1;
                        return true;
                    }
                    int num = ctx.L.input_char;
                    if (((num != 0x2c) && (num != 0x5d)) && (num != 0x7d))
                    {
                        return false;
                    }
                    ctx.L.UngetChar();
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;
                }
            }
            return true;
        }

        private static bool State9(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char == 0x72)
            {
                ctx.NextState = 10;
                return true;
            }
            return false;
        }

        private void UngetChar()
        {
            this.input_buffer = this.input_char;
        }

        public bool AllowComments
        {
            get
            {
                return this.allow_comments;
            }
            set
            {
                this.allow_comments = value;
            }
        }

        public bool AllowSingleQuotedStrings
        {
            get
            {
                return this.allow_single_quoted_strings;
            }
            set
            {
                this.allow_single_quoted_strings = value;
            }
        }

        public bool EndOfInput
        {
            get
            {
                return this.end_of_input;
            }
        }

        public string StringValue
        {
            get
            {
                return this.string_value;
            }
        }

        public int Token
        {
            get
            {
                return this.token;
            }
        }

        private delegate bool StateHandler(FsmContext ctx);
    }
}

