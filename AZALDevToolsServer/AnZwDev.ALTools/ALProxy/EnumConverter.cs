using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALProxy
{
    public class EnumConverter<T>
    {

        private Dictionary<int, T> _values;

        public EnumConverter()
        {
            _values = new Dictionary<int, T>();
        }

        public T Convert(dynamic value)
        {
            int intVal = (int)value;
            if (_values.ContainsKey(intVal))
                return _values[intVal];

            try
            {
                string name = value.ToString();
                T data = (T)Enum.Parse(typeof(T), name);
                _values.Add(intVal, data);
                return data;
            }
            catch (Exception)
            {
                return default(T);
            }
        }


    }
}
