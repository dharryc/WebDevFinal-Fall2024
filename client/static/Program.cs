var navTemplate = File.ReadAllText("./config/navbar.html");
var logInTemplate = File.ReadAllText("./config/login.html");
var baseTemplate = File.ReadAllText("./config/template.html");
string navKey = "NavBar";
string loginKey = "Login";
string contentKey = "Content";

//login prototype creation
var loginPrototype = baseTemplate;
loginPrototype = loginPrototype.Replace(loginKey, logInTemplate);
loginPrototype = loginPrototype.Replace(navKey, "");
loginPrototype = loginPrototype.Replace(contentKey, "");
File.WriteAllText("../src/compiledSite/loginPrototype.html", loginPrototype);

//user creation page Prototype
var userCreationPrototype = baseTemplate;
userCreationPrototype = userCreationPrototype.Replace(loginKey, "");
userCreationPrototype = userCreationPrototype.Replace(contentKey, "<div id=\"userForm\"> </div>");
userCreationPrototype = userCreationPrototype.Replace(navKey, "");
userCreationPrototype = userCreationPrototype.Replace("Document", "New User");
userCreationPrototype = userCreationPrototype.Replace("jsSource", "./userCreation.js");
File.WriteAllText("../src/compiledSite/userCreationPrototype.html", userCreationPrototype);

//list prototype creation (for users to see and edit their own list)
var userListPagePrototype = File.ReadAllText("config/template.html");
userListPagePrototype = userListPagePrototype.Replace(contentKey, "<div id=\"form\"> </div>");
userListPagePrototype = userListPagePrototype.Replace("Document", "Add Item");
userListPagePrototype = userListPagePrototype.Replace("jsSource", "./serviceTest.js");
userListPagePrototype = userListPagePrototype.Replace(navKey, navTemplate);
userListPagePrototype = userListPagePrototype.Replace(loginKey, "");
File.WriteAllText("../src/compiledSite/userListPagePrototype.html", userListPagePrototype);


//Preview prototype (user will be able to see a "preview" of their page as it will appear to other users)
var listPrototype = File.ReadAllText("config/template.html");

//
// var listPrototype = File.ReadAllText("config/template.html");


// var listPrototype = File.ReadAllText("config/template.html");



//setting up some basic CSS
var cssPrototype = File.ReadAllText("config/base-style.css");
File.WriteAllText("../src/compiledSite/base-style.css", cssPrototype);


// string commandText = "/C cd .. /b cd src /b npx tsc";

// System.Diagnostics.Process.Start("CMD.exe", commandText);
// //find out how to run npx tsc in command line when updating