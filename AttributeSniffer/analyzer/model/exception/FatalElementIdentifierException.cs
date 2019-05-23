using System;
using System.Collections.Generic;
using System.Text;

namespace AttributeSniffer.analyzer.model.exception
{
    class FatalElementIdentifierException : Exception
    {
        public FatalElementIdentifierException(string message) : base(message)
        {
        }
    }
}
