using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Vereyon.Windows
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    public class WaitForFormsEvent<TEventArgs>
    {

        private AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        private EventInfo _event = null;
        private object _eventContainer = null;
        private Action<object, TEventArgs> _eventHandler = null;

        public WaitForFormsEvent()
        {
        }

        public WaitForFormsEvent(object eventContainer, string eventName)
        {
            _eventContainer = eventContainer;
            _event = eventContainer.GetType().GetEvent(eventName);
            _eventHandler = ((sender, args) => { _autoResetEvent.Set(); });
        }

        public bool WaitForEvent(TimeSpan timeout)
        {
            return WaitForEvent((int)timeout.TotalMilliseconds);
        }

        public void SetEvent()
        {
            _autoResetEvent.Set();
        }

        public bool WaitForEvent(int timeout)
        {

            int waitInterval = 50;
            if(_eventHandler != null)
                _event.AddEventHandler(_eventContainer, _eventHandler);

            // Execute application events.
            while (!_autoResetEvent.WaitOne(waitInterval))
            {
                    Application.DoEvents();
                timeout -= waitInterval;

                // Check if the time out expired. If so return false after cleaning up.
                if (timeout <= 0)
                {
                    if (_eventHandler != null)
                        _event.RemoveEventHandler(_eventContainer, _eventHandler);
                    return false;
                }
            }

            // Cleanup and return true.
            if(_eventHandler != null)
                _event.RemoveEventHandler(_eventContainer, _eventHandler);
            return true;
        }
    }
}
