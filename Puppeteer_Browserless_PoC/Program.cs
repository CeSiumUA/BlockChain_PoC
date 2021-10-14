using PuppeteerSharp;

//var browserFetcher = new BrowserFetcher();

//await browserFetcher.DownloadAsync();

//var browser = await Puppeteer.LaunchAsync(new LaunchOptions()
//{
//    Headless = false,
//    DefaultViewport = new ViewPortOptions()
//    {
//        IsLandscape = true
//    }
//});

var options = new ConnectOptions()
{
    BrowserWSEndpoint = "ws://127.0.0.1:3000"
};

var browser = await Puppeteer.ConnectAsync(options);

var page = await browser.NewPageAsync();

await page.GoToAsync("https://google.com.ua");

var content = await page.GetContentAsync();

Console.ReadLine();