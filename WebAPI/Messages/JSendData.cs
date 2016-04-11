
using System.Collections.Generic;

namespace WebAPI.Messages
{
    public class JSendData<T> : JSend where T : class
    {
        public T data;

        public JSendData(string status, T data) : base(status)
        {
            this.data = data;
        }
    }
}