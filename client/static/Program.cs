// var layoutHTML = File.ReadAllText("config/layout.html");
// var headerHTML = File.ReadAllText("config/header.html");
// var page1HTML = File.ReadAllText("config/page1-body.html");
// layoutHTML = layoutHTML.Replace("secretstring", headerHTML);

// File.WriteAllText("out/index.html", page1HTML);

var listPrototype = File.ReadAllText("config/template.html");
listPrototype = listPrototype.Replace("pageContent", "<div id=\"form\"> </div>");
listPrototype = listPrototype.Replace("Document", "Add Item");
listPrototype = listPrototype.Replace("jsSource", "./serviceTest.js");
File.WriteAllText("../src/compiledSite/listprototype.html", listPrototype);

var cssPrototype = File.ReadAllText("config/template-style.css");
File.WriteAllText("../src/compiledSite/template-style.css", cssPrototype);