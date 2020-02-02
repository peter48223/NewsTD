using System.Collections.Generic;

namespace NewsTD
{
    public class NewsApiResult
    {
        public string Status { get; set; }
        public int TotalResults { get; set; }
        public List<Article> Articles { get; set; }

    }
}
