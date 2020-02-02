using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace NewsTD.Pages
{
    public class ArticleModel : PageModel
    {
        ArticleDbContext _articleDbContext;

        public Article Article { get; set; } = new Article();

        public ArticleModel(ArticleDbContext articleDbContext)
        {
            _articleDbContext = articleDbContext;
        }

        public void OnGet(Guid id)
        {
            var article = _articleDbContext.Article
                .Include(a => a.Source)
                .Where(x => x.Id == id)
                .FirstOrDefault();

            var source = _articleDbContext.Source;

            if (article != null)
            {
                this.Article = article;
            }
        }
    }
}