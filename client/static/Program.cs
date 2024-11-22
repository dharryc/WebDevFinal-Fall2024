var navTemplate = File.ReadAllText("./config/navbar.html");
var logInTemplate = File.ReadAllText("./config/login.html");
var baseTemplate = File.ReadAllText("./config/template.html");
var userListTemplate = File.ReadAllText("./config/userList.html");
string navKey = "NavBar";
string loginKey = "Login";
string contentKey = "Content";
string cssTemplate = "base-style.css";
string moreStuffKey = "/* more stuff */";

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
userCreationPrototype = userCreationPrototype.Replace(cssTemplate, "newUser-style.css");
File.WriteAllText("../src/compiledSite/userCreationPrototype.html", userCreationPrototype);

//list prototype creation (for users to see and edit their own list)
var userListPagePrototype = File.ReadAllText("config/template.html");
userListPagePrototype = userListPagePrototype.Replace(contentKey, userListTemplate);
userListPagePrototype = userListPagePrototype.Replace("Document", "Add Item");
userListPagePrototype = userListPagePrototype.Replace("jsSource", "./serviceTest.js");
userListPagePrototype = userListPagePrototype.Replace(navKey, navTemplate);
userListPagePrototype = userListPagePrototype.Replace(loginKey, "");
userListPagePrototype = userListPagePrototype.Replace(cssTemplate, "userList-style.css");
File.WriteAllText("../src/compiledSite/userListPagePrototype.html", userListPagePrototype);


//setting up some basic CSS
var cssPrototype = File.ReadAllText("config/base-style.css");
File.WriteAllText("../src/compiledSite/base-style.css", cssPrototype);

var cssNewUserTemplate = File.ReadAllText("./config/newUser-style.css");
var cssNewUser = cssPrototype.Replace("/* more styling */", cssNewUserTemplate);
File.WriteAllText("../src/compiledSite/newUser-style.css", cssNewUser);

cssPrototype = File.ReadAllText("config/base-style.css");
var cssNavTemplate = File.ReadAllText("./config/navStyling.css");
var cssUserList = cssPrototype.Replace(moreStuffKey, cssNavTemplate);
File.WriteAllText("../src/compiledSite/userList-style.css", cssUserList);