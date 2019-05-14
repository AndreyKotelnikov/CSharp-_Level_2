using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    /// <summary>
    /// Интерфейс для объектов, действия которых нужно логировать
    /// </summary>
    interface ILog
    {
        /// <summary>
        /// Событие, которое активируется для действий, подлежащих логированию
        /// </summary>
        event EventHandler<LogEventArgs> Logging;
    }
}
