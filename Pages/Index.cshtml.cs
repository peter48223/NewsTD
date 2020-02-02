using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewsTD.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<IndexModel> _logger;
        private readonly ArticleDbContext _articleDbContext;

        public List<Article> Articles { get; set; }
        public string zeroRecordsMessage { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory clientFactory, ArticleDbContext articleDbContext)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _articleDbContext = articleDbContext;

            this.Articles = new List<Article>();
        }

        public async Task OnPostAsync(string searchFor, string country)
        {
            if (searchFor is null)
            {
                return;
            }

            await DoSearchPrivate(searchFor, country);
        }

        private async Task DoSearchPrivate(string searchFor, string country = "us")
        {
            var dateFrom = DateTime.Now.AddDays(-25).ToString("yyyy-MM-dd");

            var url = "https://newsapi.org/v2/top-headlines?" +
                      "q=" + searchFor +
                      "&from=" + dateFrom +
                      "&sortBy=popularity&" +
                      "country=" + country +
                      "&apiKey=691c99d17ab64640a1113e289427717d";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var newsApiResult = JsonConvert.DeserializeObject<NewsApiResult>(responseString);

                _articleDbContext.AddRange(newsApiResult.Articles);
                _articleDbContext.SaveChanges();

                this.Articles = newsApiResult.Articles;
                zeroRecordsMessage = this.Articles.Count == 0 ? "No records returned, refine your search." : "";
            }
            else
            {
                _logger.LogError("Data fetch unsuccesful.");
            }
        }
    }
}
