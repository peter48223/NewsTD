using System;

namespace NewsTD
{
    public class Source
    {
        public string Id
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }

        public string Name { get; set; }
        
    }
}
