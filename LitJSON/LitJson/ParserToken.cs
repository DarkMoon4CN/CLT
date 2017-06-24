namespace LitJson
{
    using System;

    internal enum ParserToken
    {
        Array = 0x1000c,
        ArrayPrime = 0x1000d,
        Char = 0x10006,
        CharSeq = 0x10005,
        End = 0x10011,
        Epsilon = 0x10012,
        False = 0x10003,
        None = 0x10000,
        Null = 0x10004,
        Number = 0x10001,
        Object = 0x10008,
        ObjectPrime = 0x10009,
        Pair = 0x1000a,
        PairRest = 0x1000b,
        String = 0x10010,
        Text = 0x10007,
        True = 0x10002,
        Value = 0x1000e,
        ValueRest = 0x1000f
    }
}

