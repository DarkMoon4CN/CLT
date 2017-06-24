namespace LitJson
{
    using System;

    internal class FsmContext
    {
        public Lexer L;
        public int NextState;
        public bool Return;
        public int StateStack;
    }
}

