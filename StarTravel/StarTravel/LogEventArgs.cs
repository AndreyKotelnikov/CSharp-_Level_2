using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    class LogEventArgs : EventArgs
    {
        public Object Sender { get; private set; }
        public MethodInfo SenderMethodInfo { get; private set; }
        public PropertyInfo SenderPropertyInfo { get; private set; }
        public Object ObjectToModifiedInMethod { get; private set; }
        public int Number { get; private set; }
        public string Message { get; private set; }
        public string Text { get; set; }

        public LogEventArgs(Object sender = null , MethodInfo senderMethodInfo = null, PropertyInfo senderPropertyInfo = null,
            Object objectToModifiedInMethod = null, int number = default(int), string message = default(string))
        {
            Sender = sender;
            SenderMethodInfo = senderMethodInfo;
            SenderPropertyInfo = senderPropertyInfo;
            ObjectToModifiedInMethod = objectToModifiedInMethod;
            Number = number;
            Message = message;
            Text = string.Empty;
        }

        public override string ToString()
        {
            return $"{Sender?.GetType().Name} {Message} {(Number == 0 ? "" : Number.ToString())} {ObjectToModifiedInMethod?.GetType().Name}";
        }
    }
}
