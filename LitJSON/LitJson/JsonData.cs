namespace LitJson
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Reflection;

    public class JsonData : IJsonWrapper, IList, IOrderedDictionary, IDictionary, ICollection, IEnumerable, IEquatable<JsonData>
    {
        private IList<JsonData> inst_array;
        private bool inst_boolean;
        private double inst_double;
        private int inst_int;
        private long inst_long;
        private IDictionary<string, JsonData> inst_object;
        private string inst_string;
        private string json;
        private IList<KeyValuePair<string, JsonData>> object_list;
        private JsonType type;

        public JsonData()
        {
        }

        public JsonData(bool boolean)
        {
            this.type = JsonType.Boolean;
            this.inst_boolean = boolean;
        }

        public JsonData(double number)
        {
            this.type = JsonType.Double;
            this.inst_double = number;
        }

        public JsonData(int number)
        {
            this.type = JsonType.Int;
            this.inst_int = number;
        }

        public JsonData(long number)
        {
            this.type = JsonType.Long;
            this.inst_long = number;
        }

        public JsonData(object obj)
        {
            if (obj is bool)
            {
                this.type = JsonType.Boolean;
                this.inst_boolean = (bool) obj;
            }
            else if (obj is double)
            {
                this.type = JsonType.Double;
                this.inst_double = (double) obj;
            }
            else if (obj is int)
            {
                this.type = JsonType.Int;
                this.inst_int = (int) obj;
            }
            else if (obj is long)
            {
                this.type = JsonType.Long;
                this.inst_long = (long) obj;
            }
            else
            {
                if (!(obj is string))
                {
                    throw new ArgumentException("Unable to wrap the given object with JsonData");
                }
                this.type = JsonType.String;
                this.inst_string = (string) obj;
            }
        }

        public JsonData(string str)
        {
            this.type = JsonType.String;
            this.inst_string = str;
        }

        public int Add(object value)
        {
            JsonData data = this.ToJsonData(value);
            this.json = null;
            return this.EnsureList().Add(data);
        }

        public void Clear()
        {
            if (this.IsObject)
            {
                ((IDictionary) this).Clear();
            }
            else if (this.IsArray)
            {
                ((IList) this).Clear();
            }
        }

        private ICollection EnsureCollection()
        {
            if (this.type == JsonType.Array)
            {
                return (ICollection) this.inst_array;
            }
            if (this.type != JsonType.Object)
            {
                throw new InvalidOperationException("The JsonData instance has to be initialized first");
            }
            return (ICollection) this.inst_object;
        }

        private IDictionary EnsureDictionary()
        {
            if (this.type != JsonType.Object)
            {
                if (this.type != JsonType.None)
                {
                    throw new InvalidOperationException("Instance of JsonData is not a dictionary");
                }
                this.type = JsonType.Object;
                this.inst_object = new Dictionary<string, JsonData>();
                this.object_list = new List<KeyValuePair<string, JsonData>>();
            }
            return (IDictionary) this.inst_object;
        }

        private IList EnsureList()
        {
            if (this.type != JsonType.Array)
            {
                if (this.type != JsonType.None)
                {
                    throw new InvalidOperationException("Instance of JsonData is not a list");
                }
                this.type = JsonType.Array;
                this.inst_array = new List<JsonData>();
            }
            return (IList) this.inst_array;
        }

        public bool Equals(JsonData x)
        {
            if (x != null)
            {
                if (x.type != this.type)
                {
                    return false;
                }
                switch (this.type)
                {
                    case JsonType.None:
                        return true;

                    case JsonType.Object:
                        return this.inst_object.Equals(x.inst_object);

                    case JsonType.Array:
                        return this.inst_array.Equals(x.inst_array);

                    case JsonType.String:
                        return this.inst_string.Equals(x.inst_string);

                    case JsonType.Int:
                        return this.inst_int.Equals(x.inst_int);

                    case JsonType.Long:
                        return this.inst_long.Equals(x.inst_long);

                    case JsonType.Double:
                        return this.inst_double.Equals(x.inst_double);

                    case JsonType.Boolean:
                        return this.inst_boolean.Equals(x.inst_boolean);
                }
            }
            return false;
        }

        public JsonType GetJsonType()
        {
            return this.type;
        }

        bool IJsonWrapper.GetBoolean()
        {
            if (this.type != JsonType.Boolean)
            {
                throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
            }
            return this.inst_boolean;
        }

        double IJsonWrapper.GetDouble()
        {
            if (this.type != JsonType.Double)
            {
                throw new InvalidOperationException("JsonData instance doesn't hold a double");
            }
            return this.inst_double;
        }

        int IJsonWrapper.GetInt()
        {
            if (this.type != JsonType.Int)
            {
                throw new InvalidOperationException("JsonData instance doesn't hold an int");
            }
            return this.inst_int;
        }

        long IJsonWrapper.GetLong()
        {
            if (this.type != JsonType.Long)
            {
                throw new InvalidOperationException("JsonData instance doesn't hold a long");
            }
            return this.inst_long;
        }

        string IJsonWrapper.GetString()
        {
            if (this.type != JsonType.String)
            {
                throw new InvalidOperationException("JsonData instance doesn't hold a string");
            }
            return this.inst_string;
        }

        void IJsonWrapper.SetBoolean(bool val)
        {
            this.type = JsonType.Boolean;
            this.inst_boolean = val;
            this.json = null;
        }

        void IJsonWrapper.SetDouble(double val)
        {
            this.type = JsonType.Double;
            this.inst_double = val;
            this.json = null;
        }

        void IJsonWrapper.SetInt(int val)
        {
            this.type = JsonType.Int;
            this.inst_int = val;
            this.json = null;
        }

        void IJsonWrapper.SetLong(long val)
        {
            this.type = JsonType.Long;
            this.inst_long = val;
            this.json = null;
        }

        void IJsonWrapper.SetString(string val)
        {
            this.type = JsonType.String;
            this.inst_string = val;
            this.json = null;
        }

        string IJsonWrapper.ToJson()
        {
            return this.ToJson();
        }

        void IJsonWrapper.ToJson(JsonWriter writer)
        {
            this.ToJson(writer);
        }

        public static explicit operator bool(JsonData data)
        {
            if (data.type != JsonType.Boolean)
            {
                throw new InvalidCastException("Instance of JsonData doesn't hold a double");
            }
            return data.inst_boolean;
        }

        public static explicit operator double(JsonData data)
        {
            if (data.type != JsonType.Double)
            {
                throw new InvalidCastException("Instance of JsonData doesn't hold a double");
            }
            return data.inst_double;
        }

        public static explicit operator int(JsonData data)
        {
            if (data.type != JsonType.Int)
            {
                throw new InvalidCastException("Instance of JsonData doesn't hold an int");
            }
            return data.inst_int;
        }

        public static explicit operator long(JsonData data)
        {
            if (data.type != JsonType.Long)
            {
                throw new InvalidCastException("Instance of JsonData doesn't hold an int");
            }
            return data.inst_long;
        }

        public static explicit operator string(JsonData data)
        {
            if (data.type != JsonType.String)
            {
                throw new InvalidCastException("Instance of JsonData doesn't hold a string");
            }
            return data.inst_string;
        }

        public static implicit operator JsonData(bool data)
        {
            return new JsonData(data);
        }

        public static implicit operator JsonData(double data)
        {
            return new JsonData(data);
        }

        public static implicit operator JsonData(int data)
        {
            return new JsonData(data);
        }

        public static implicit operator JsonData(long data)
        {
            return new JsonData(data);
        }

        public static implicit operator JsonData(string data)
        {
            return new JsonData(data);
        }

        public void SetJsonType(JsonType type)
        {
            if (this.type != type)
            {
                switch (type)
                {
                    case JsonType.Object:
                        this.inst_object = new Dictionary<string, JsonData>();
                        this.object_list = new List<KeyValuePair<string, JsonData>>();
                        break;

                    case JsonType.Array:
                        this.inst_array = new List<JsonData>();
                        break;

                    case JsonType.String:
                        this.inst_string = null;
                        break;

                    case JsonType.Int:
                        this.inst_int = 0;
                        break;

                    case JsonType.Long:
                        this.inst_long = 0L;
                        break;

                    case JsonType.Double:
                        this.inst_double = 0.0;
                        break;

                    case JsonType.Boolean:
                        this.inst_boolean = false;
                        break;
                }
                this.type = type;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.EnsureCollection().CopyTo(array, index);
        }

        void IDictionary.Add(object key, object value)
        {
            JsonData data = this.ToJsonData(value);
            this.EnsureDictionary().Add(key, data);
            KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>((string) key, data);
            this.object_list.Add(item);
            this.json = null;
        }

        void IDictionary.Clear()
        {
            this.EnsureDictionary().Clear();
            this.object_list.Clear();
            this.json = null;
        }

        bool IDictionary.Contains(object key)
        {
            return this.EnsureDictionary().Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IOrderedDictionary) this).GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            this.EnsureDictionary().Remove(key);
            for (int i = 0; i < this.object_list.Count; i++)
            {
                KeyValuePair<string, JsonData> pair = this.object_list[i];
                if (pair.Key == ((string) key))
                {
                    this.object_list.RemoveAt(i);
                    break;
                }
            }
            this.json = null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.EnsureCollection().GetEnumerator();
        }

        int IList.Add(object value)
        {
            return this.Add(value);
        }

        void IList.Clear()
        {
            this.EnsureList().Clear();
            this.json = null;
        }

        bool IList.Contains(object value)
        {
            return this.EnsureList().Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return this.EnsureList().IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            this.EnsureList().Insert(index, value);
            this.json = null;
        }

        void IList.Remove(object value)
        {
            this.EnsureList().Remove(value);
            this.json = null;
        }

        void IList.RemoveAt(int index)
        {
            this.EnsureList().RemoveAt(index);
            this.json = null;
        }

        IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
        {
            this.EnsureDictionary();
            return new LitJson.OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
        }

        void IOrderedDictionary.Insert(int idx, object key, object value)
        {
            string str = (string) key;
            JsonData data = this.ToJsonData(value);
            this[str] = data;
            KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>(str, data);
            this.object_list.Insert(idx, item);
        }

        void IOrderedDictionary.RemoveAt(int idx)
        {
            this.EnsureDictionary();
            KeyValuePair<string, JsonData> pair = this.object_list[idx];
            this.inst_object.Remove(pair.Key);
            this.object_list.RemoveAt(idx);
        }

        public string ToJson()
        {
            if (this.json == null)
            {
                StringWriter writer = new StringWriter();
                JsonWriter writer2 = new JsonWriter(writer) {
                    Validate = false
                };
                WriteJson(this, writer2);
                this.json = writer.ToString();
            }
            return this.json;
        }

        public void ToJson(JsonWriter writer)
        {
            bool validate = writer.Validate;
            writer.Validate = false;
            WriteJson(this, writer);
            writer.Validate = validate;
        }

        private JsonData ToJsonData(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is JsonData)
            {
                return (JsonData) obj;
            }
            return new JsonData(obj);
        }

        public override string ToString()
        {
            switch (this.type)
            {
                case JsonType.Object:
                    return "JsonData object";

                case JsonType.Array:
                    return "JsonData array";

                case JsonType.String:
                    return this.inst_string;

                case JsonType.Int:
                    return this.inst_int.ToString();

                case JsonType.Long:
                    return this.inst_long.ToString();

                case JsonType.Double:
                    return this.inst_double.ToString();

                case JsonType.Boolean:
                    return this.inst_boolean.ToString();
            }
            return "Uninitialized JsonData";
        }

        private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
        {
            if (obj.IsString)
            {
                writer.Write(obj.GetString());
            }
            else if (obj.IsBoolean)
            {
                writer.Write(obj.GetBoolean());
            }
            else if (obj.IsDouble)
            {
                writer.Write(obj.GetDouble());
            }
            else if (obj.IsInt)
            {
                writer.Write(obj.GetInt());
            }
            else if (obj.IsLong)
            {
                writer.Write(obj.GetLong());
            }
            else if (obj.IsArray)
            {
                writer.WriteArrayStart();
                foreach (object obj2 in obj)
                {
                    WriteJson((JsonData) obj2, writer);
                }
                writer.WriteArrayEnd();
            }
            else if (obj.IsObject)
            {
                writer.WriteObjectStart();
                foreach (DictionaryEntry entry in obj)
                {
                    writer.WritePropertyName((string) entry.Key);
                    WriteJson((JsonData) entry.Value, writer);
                }
                writer.WriteObjectEnd();
            }
        }

        public int Count
        {
            get
            {
                return this.EnsureCollection().Count;
            }
        }

        public bool IsArray
        {
            get
            {
                return (this.type == JsonType.Array);
            }
        }

        public bool IsBoolean
        {
            get
            {
                return (this.type == JsonType.Boolean);
            }
        }

        public bool IsDouble
        {
            get
            {
                return (this.type == JsonType.Double);
            }
        }

        public bool IsInt
        {
            get
            {
                return (this.type == JsonType.Int);
            }
        }

        public bool IsLong
        {
            get
            {
                return (this.type == JsonType.Long);
            }
        }

        public bool IsObject
        {
            get
            {
                return (this.type == JsonType.Object);
            }
        }

        public bool IsString
        {
            get
            {
                return (this.type == JsonType.String);
            }
        }

        public JsonData this[int index]
        {
            get
            {
                this.EnsureCollection();
                if (this.type == JsonType.Array)
                {
                    return this.inst_array[index];
                }
                KeyValuePair<string, JsonData> pair = this.object_list[index];
                return pair.Value;
            }
            set
            {
                this.EnsureCollection();
                if (this.type == JsonType.Array)
                {
                    this.inst_array[index] = value;
                }
                else
                {
                    KeyValuePair<string, JsonData> pair = this.object_list[index];
                    KeyValuePair<string, JsonData> pair2 = new KeyValuePair<string, JsonData>(pair.Key, value);
                    this.object_list[index] = pair2;
                    this.inst_object[pair.Key] = value;
                }
                this.json = null;
            }
        }

        public JsonData this[string prop_name]
        {
            get
            {
                this.EnsureDictionary();
                return this.inst_object[prop_name];
            }
            set
            {
                this.EnsureDictionary();
                KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>(prop_name, value);
                if (this.inst_object.ContainsKey(prop_name))
                {
                    for (int i = 0; i < this.object_list.Count; i++)
                    {
                        KeyValuePair<string, JsonData> pair2 = this.object_list[i];
                        if (pair2.Key == prop_name)
                        {
                            this.object_list[i] = item;
                            break;
                        }
                    }
                }
                else
                {
                    this.object_list.Add(item);
                }
                this.inst_object[prop_name] = value;
                this.json = null;
            }
        }

        bool IJsonWrapper.IsArray
        {
            get
            {
                return this.IsArray;
            }
        }

        bool IJsonWrapper.IsBoolean
        {
            get
            {
                return this.IsBoolean;
            }
        }

        bool IJsonWrapper.IsDouble
        {
            get
            {
                return this.IsDouble;
            }
        }

        bool IJsonWrapper.IsInt
        {
            get
            {
                return this.IsInt;
            }
        }

        bool IJsonWrapper.IsLong
        {
            get
            {
                return this.IsLong;
            }
        }

        bool IJsonWrapper.IsObject
        {
            get
            {
                return this.IsObject;
            }
        }

        bool IJsonWrapper.IsString
        {
            get
            {
                return this.IsString;
            }
        }

        int ICollection.Count
        {
            get
            {
                return this.Count;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return this.EnsureCollection().IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return this.EnsureCollection().SyncRoot;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                return this.EnsureDictionary().IsFixedSize;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return this.EnsureDictionary().IsReadOnly;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return this.EnsureDictionary()[key];
            }
            set
            {
                if (!(key is string))
                {
                    throw new ArgumentException("The key has to be a string");
                }
                JsonData data = this.ToJsonData(value);
                this[(string) key] = data;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                this.EnsureDictionary();
                IList<string> list = new List<string>();
                foreach (KeyValuePair<string, JsonData> pair in this.object_list)
                {
                    list.Add(pair.Key);
                }
                return (ICollection) list;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                this.EnsureDictionary();
                IList<JsonData> list = new List<JsonData>();
                foreach (KeyValuePair<string, JsonData> pair in this.object_list)
                {
                    list.Add(pair.Value);
                }
                return (ICollection) list;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return this.EnsureList().IsFixedSize;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return this.EnsureList().IsReadOnly;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this.EnsureList()[index];
            }
            set
            {
                this.EnsureList();
                JsonData data = this.ToJsonData(value);
                this[index] = data;
            }
        }

        object IOrderedDictionary.this[int idx]
        {
            get
            {
                this.EnsureDictionary();
                KeyValuePair<string, JsonData> pair = this.object_list[idx];
                return pair.Value;
            }
            set
            {
                this.EnsureDictionary();
                JsonData data = this.ToJsonData(value);
                KeyValuePair<string, JsonData> pair = this.object_list[idx];
                this.inst_object[pair.Key] = data;
                KeyValuePair<string, JsonData> pair2 = new KeyValuePair<string, JsonData>(pair.Key, data);
                this.object_list[idx] = pair2;
            }
        }
    }
}

