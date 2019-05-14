using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    /// <summary>
    /// Класс для хранения информации о логируемом событии
    /// </summary>
    class LogEventArgs : EventArgs
    {
        /// <summary>
        /// Объект - Отправитель данных для логирования
        /// </summary>
        public Object Sender { get; private set; }
        /// <summary>
        /// Метод - Отправитель данных для логирования
        /// </summary>
        public MethodInfo SenderMethodInfo { get; private set; }
        /// <summary>
        /// Свойство - Отправитель данных для логирования
        /// </summary>
        public PropertyInfo SenderPropertyInfo { get; private set; }
        /// <summary>
        /// Объект, который изменяется в методе/свойстве отправителя данных для логирования
        /// </summary>
        public Object ObjectToModifiedInMethod { get; private set; }
        /// <summary>
        /// Поле для хранения числовых данных для логирования
        /// </summary>
        public int Number { get; private set; }
        /// <summary>
        /// Сообщение для логирования
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Поле для хранения результатов первичной обработки информации по данным из других полей этого объекта
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Конструктор экземпляра класса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="senderMethodInfo"></param>
        /// <param name="senderPropertyInfo"></param>
        /// <param name="objectToModifiedInMethod"></param>
        /// <param name="number"></param>
        /// <param name="message"></param>
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
        /// <summary>
        /// Переопределённый метод вывода данных объекта в виде текста
        /// </summary>
        /// <returns>Возвращает представление объекта в виде строки текста</returns>
        public override string ToString()
        {
            return $"{Sender?.GetType().Name} {Message} {(Number == 0 ? "" : Number.ToString())}";
        }
    }
}
