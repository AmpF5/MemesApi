using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
namespace MemesApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class WykopController : ControllerBase
{
    const string url = "https://www.wykop.pl/mikroblog/hot/";
    static readonly HtmlDocument doc = GetDocument(url);

    static HtmlDocument GetDocument(string url)
    {
        HtmlWeb web = new();
        HtmlDocument doc = web.Load(url);
        return doc;
    }
    static List<string> GetMemesText(string url)
    {
        List<string> memeText = new();
        HtmlNodeCollection textNodes = doc.DocumentNode.SelectNodes("//*[@id=\"itemsStream\"]/li/div//div[@class=\"text \"]/p");
        foreach (var text in textNodes)
        {
            // string href = text.Attributes["href"].Value;
            // memeText.Add(text.);
        }

        return memeText;
    }
    [HttpGet("MikroBlog/Hot")]
    public IEnumerable<Meme> Get()
    {
        List<string> textList = GetMemesText(url);
        return Enumerable.Range(0, textList.Count).Select(index => new Meme
        {
            Title = "Title",
            Author = "Author",
            Url = "url",
            Likes = "123",
            Text = textList[index]
        }
        ).ToArray();
    }
}