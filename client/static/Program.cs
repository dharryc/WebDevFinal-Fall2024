var layoutHTML = File.ReadAllText("config/layout.html");
var headerHTML = File.ReadAllText("config/header.html");
var page1HTML = File.ReadAllText("config/page1-body.html");
layoutHTML = layoutHTML.Replace("secretstring", headerHTML);

File.WriteAllText("out/index.html", page1HTML);