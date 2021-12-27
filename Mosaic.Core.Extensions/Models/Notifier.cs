using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensibleProgramming.Core.Models
{
    public abstract class Notifier
    {
        #region delegates/actions

        public Action<string> OnNotify { get; set; }

        protected virtual void Notify(string message)
        {
            if (OnNotify != null)
            {
                OnNotify(message);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(message);
            }
        }

        public Action<Progress> OnProgress { get; set; }

        protected virtual void NotifyProgress(Progress message)
        {
            if (OnNotify != null)
            {
                OnProgress(message);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(message);
            }
        }

        public Action<string> OnWarn { get; set; }

        protected virtual void Warn(string message)
        {
            if (OnWarn != null)
            {
                OnWarn(message);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(message);
            }
        }

        public Action<Exception> OnError { get; set; }

        protected virtual void PassError(Exception ex)
        {
            if (OnError != null)
            {
                OnError(ex);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        #endregion
    }

    public struct Progress
    {
        public int Processed { get; set; }
        public int Max { get; set; }

    }
}
