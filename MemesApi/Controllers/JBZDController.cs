using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;

namespace MemesApi.Controllers;
[ApiController]
[Route("api/[controller]")]

public class JBZDController : ControllerBase
{
    const string url = "https://jbzd.com.pl/";
    static readonly HtmlDocument doc = GetDocument(url);
    static HtmlDocument GetDocument(string url)
    {
        HtmlWeb web = new();
        HtmlDocument doc = web.Load(url);
        return doc;
    }
    static List<string> GetMemesTitle(string url)
    {
        List<string> memeTitles = new();
        HtmlNodeCollection titlesNodes = doc.DocumentNode.SelectNodes("//h3/a");
        var baseUri = new Uri(url);
        foreach (var title in titlesNodes)
        {
            string href = title.Attributes["href"].Value;
            memeTitles.Add(new Uri(baseUri, href).AbsoluteUri);
        }
        return memeTitles;
    }
    static List<string> GetMemesLikes(string url)
    {
        List<string> memeLikes = new();
        HtmlNodeCollection likesNode = doc.DocumentNode.SelectNodes("//body//main/section/article////span");
        // HtmlNodeCollection
        var baseUri = new Uri(url);
        foreach (var likes in likesNode)
        {
            // likes.InnerHtml
            string href = likes.Attributes["href"].Value;
            memeLikes.Add(likes.InnerHtml);
        }
        return memeLikes;
    }
    [HttpGet("MainPage")]
    public IEnumerable<Meme> Get()
    {
        List<string> memeList = GetMemesTitle(url);
        List<string> likeList = GetMemesLikes(url);
        return Enumerable.Range(0, memeList.Count).Select(index => new Meme
        {
            Title = memeList[index],
            Author = "null",
            Url = "Test",
            Likes = likeList[index]
            // Likes = "123"
        }).ToArray();
    }
}
