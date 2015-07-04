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
    /// Wrapper class for AutoResetEvent to allow waiting for events while still processing Windows messages.
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    public class WaitForFormsEvent
    {

        private AutoResetEvent AutoResetEvent;
        private EventInfo ListenEvent;
        private object EventContainer;
        private Delegate ListenEventHandler;

        public WaitForFormsEvent()
        {

            AutoResetEvent = new AutoResetEvent(false);
            ListenEvent = null;
            EventContainer = null;
            ListenEventHandler = null;
        }

        public void ListenForEvent<TEventArgs>(object eventContainer, string eventName)
        {

            EventContainer = eventContainer;
            ListenEvent = eventContainer.GetType().GetEvent(eventName);
            ListenEventHandler = new Action<object, TEventArgs>((sender, args) => { AutoResetEvent.Set(); });

            // Install the event handler.
            ListenEvent.AddEventHandler(EventContainer, ListenEventHandler);
        }

        /// <summary>
        /// Manually sets the event. Use this in a custom event handler function.
        /// </summary>
        public void SetEvent()
        {
            AutoResetEvent.Set();
        }

        public bool WaitForEvent(TimeSpan timeout)
        {
            return WaitForEvent((int)timeout.TotalMilliseconds);
        }

        /// <summary>
        /// Waits for the event with the given timeout while still running the message loop.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool WaitForEvent(int timeout)
        {

            int waitInterval = 50;

            // Execute application events.
            while (!AutoResetEvent.WaitOne(waitInterval))
            {
                    Application.DoEvents();
                timeout -= waitInterval;

                // Check if the time out expired. If so return false after cleaning up.
                if (timeout <= 0)
                {
                    if (ListenEventHandler != null)
                        ListenEvent.RemoveEventHandler(EventContainer, ListenEventHandler);
                    return false;
                }
            }

            // Cleanup and return true.
            if(ListenEventHandler != null)
                ListenEvent.RemoveEventHandler(EventContainer, ListenEventHandler);
            return true;
        }
    }
}
