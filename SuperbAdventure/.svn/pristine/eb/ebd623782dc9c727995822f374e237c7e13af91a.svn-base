using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class MessageEventArgs : EventArgs //EventArgs is an in-built class 
    {
        public string Message { get; set; }
        public bool AddExtraNewLine { get; private set; }
        public MessageEventArgs(string message, bool addExtraNewLine)
        {
            Message = message;
            AddExtraNewLine = addExtraNewLine;
        }
    }
}
