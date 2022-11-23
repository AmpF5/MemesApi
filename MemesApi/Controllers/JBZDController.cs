using System.Text;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;

namespace MemesApi.Controllers;
[ApiController]
[Route("api/[controller]")]

public class JBZDController : ControllerBase
{
    private const string url = "https://jbzd.com.pl/";
    private static readonly HtmlDocument _doc = GetDocument(url);
    private static HtmlDocument GetDocument(string url)
    {
        HtmlWeb web = new();
        var doc = web.Load(url);
        ;        
        return doc;
    }
    private static List<string> GetMemesTitle(string url)
    {
        var titlesNode = _doc.DocumentNode.SelectNodes("/html/body/div/main/section/article/div/h3/a");
        return titlesNode.Select(title => FixFormattingTitle(title.InnerHtml)).ToList();
    }
    private static List<string> GetMemesUrl()
    {
        List<string> memeUrl = new();
        var titlesNodes = _doc.DocumentNode.SelectNodes("//h3/a");
        var baseUri = new Uri(url);
        foreach (var title in titlesNodes)
        {
            var href = title.Attributes["href"].Value;
            memeUrl.Add(new Uri(baseUri, href).AbsoluteUri);
        }
        return memeUrl;
    }
    
    private static List<string> GetLikes(string url)
    {
        var tempUrl = url;
        var likesList = new List<string>();
        var likes = _doc.DocumentNode.SelectNodes("/html/body/div/main/section/article/div[2]/div[2]");
        foreach (var like in likes)
        {
            likesList.Add(FixFormattingLike(like.InnerHtml));
        }
        return likesList;
    }
    
    [HttpGet("MainPage")]
    public IEnumerable<Meme> Get()
    {
        var memeList = GetMemesTitle(url);
        var memeUrl = GetMemesUrl();
        var likeList = GetLikes(url);
        return Enumerable.Range(0, memeList.Count).Select(index => new Meme
        {
            Title = memeList[index],
            // Author = "null",
            Url = memeUrl[index],
            Likes = likeList[index]
        }).ToArray();
    }

    private static string FixFormattingLike(string preFixed)
    {
        var bobTheBuilder = new StringBuilder(preFixed);
        bobTheBuilder.Remove(0, 61);
        var x = @"\";
        // TODO add formatting for 3 digits likes count
        if (bobTheBuilder[3].ToString() == x) bobTheBuilder.Append("XDDD");
        // else bobTheBuilder.Remove(4, bobTheBuilder.Length - 4);
        else bobTheBuilder.Remove(4, bobTheBuilder.Length - 4);
        // postFix = bobTheBuilder.ToString();
        return bobTheBuilder.ToString();
    }

    private static string FixFormattingTitle(string preFixed)
    {
        var postFix = "";
        var bobTheBuilder = new StringBuilder(preFixed);
        bobTheBuilder.Remove(0, 17);
        bobTheBuilder.Remove(bobTheBuilder.Length - 13, 13);
        return bobTheBuilder.ToString();
    }
}
