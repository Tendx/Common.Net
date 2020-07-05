using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Zen
{
    [Serializable]
    public abstract class ModelBase : INotifyPropertyChanged
    {
        private readonly List<Delegate> _serializableDelegates = new List<Delegate>();

        [OnSerializing]
        public void OnSerializing(StreamingContext context)
        {
            _serializableDelegates.Clear();
            if (PropertyChanged != null)
                foreach (var invocation in PropertyChanged.GetInvocationList())
                    if (invocation.Target.GetType().IsSerializable)
                        _serializableDelegates.Add(invocation);
        }

        [OnDeserializing]
        public void OnDeserializing(StreamingContext context)
        {
            if (_serializableDelegates != null)
                foreach (var invocation in _serializableDelegates)
                    PropertyChanged += (PropertyChangedEventHandler)invocation;
        }

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public T PasteInto<T>(T dst) where T : ModelBase
        {
            if (dst != null)
                foreach (PropertyInfo pi in typeof(T).GetProperties())
                    if (pi.CanWrite) pi.SetValue(dst, pi.GetValue(this));
            return dst;
        }
    }
}
