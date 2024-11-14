// var layoutHTML = File.ReadAllText("config/layout.html");
// var headerHTML = File.ReadAllText("config/header.html");
// var page1HTML = File.ReadAllText("config/page1-body.html");
// layoutHTML = layoutHTML.Replace("secretstring", headerHTML);

// File.WriteAllText("out/index.html", page1HTML);
var navTemplate = File.ReadAllText("./config/navbar.html");
var logInTemplate = File.ReadAllText("./config/login.html");
string navKey = "NavBar";
string loginKey = "Login";
string contentKey = "Content";

//login prototype creation
var loginPrototype = File.ReadAllText("config/template.html");
loginPrototype = loginPrototype.Replace(loginKey, logInTemplate);
loginPrototype = loginPrototype.Replace(navKey, "");
loginPrototype = loginPrototype.Replace(contentKey, "");
File.WriteAllText("../src/compiledSite/loginPrototype.html", loginPrototype);

//user creation page Prototype
var userCreationPrototype = File.ReadAllText("config/template.html");

//list prototype creation (for users to see and edit their own list)
var userPagePrototype = File.ReadAllText("config/template.html");
userPagePrototype = userPagePrototype.Replace("pageContent", "<div id=\"form\"> </div>");
userPagePrototype = userPagePrototype.Replace("Document", "Add Item");
userPagePrototype = userPagePrototype.Replace("jsSource", "./serviceTest.js");
File.WriteAllText("../src/compiledSite/userPagePrototype.html", userPagePrototype);


//Preview prototype (user will be able to see a "preview" of their page as it will appear to other users)
var listPrototype = File.ReadAllText("config/template.html");

//
// var listPrototype = File.ReadAllText("config/template.html");


// var listPrototype = File.ReadAllText("config/template.html");



//setting up some basic CSS
var cssPrototype = File.ReadAllText("config/template-style.css");
File.WriteAllText("../src/compiledSite/template-style.css", cssPrototype);
//find out how to run npx tsc in command line when updating