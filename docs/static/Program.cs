var navTemplate = File.ReadAllText("./config/navbar.html");
var logInTemplate = File.ReadAllText("./config/login.html");
var baseTemplate = File.ReadAllText("./config/template.html");
var userListTemplate = File.ReadAllText("./config/userList.html");
var pageContentTemplate = File.ReadAllText("./config/pageContent.html");
string navKey = "NavBar";
string loginKey = "Login";
string contentKey = "Content";
string js = "jsSource";

//login prototype creation
var loginPrototype = baseTemplate;
loginPrototype = loginPrototype.Replace(loginKey, logInTemplate);
loginPrototype = loginPrototype.Replace(navKey, "");
loginPrototype = loginPrototype.Replace(contentKey, "");
loginPrototype = loginPrototype.Replace(js, "./loginUi.js");
loginPrototype = loginPrototype.Replace("Document", "Login");
File.WriteAllText("../src/compiledSite/loginPrototype.html", loginPrototype);

//user creation page Prototype
var userCreationPrototype = baseTemplate;
userCreationPrototype = userCreationPrototype.Replace(loginKey, "");
userCreationPrototype = userCreationPrototype.Replace(contentKey, "<div id=\"userForm\"> </div>");
userCreationPrototype = userCreationPrototype.Replace(navKey, "");
userCreationPrototype = userCreationPrototype.Replace("Document", "New User");
userCreationPrototype = userCreationPrototype.Replace(js, "./userCreation.js");
File.WriteAllText("../src/compiledSite/userCreationPrototype.html", userCreationPrototype);

//list prototype creation (for users to see and edit their own list)
var userListPagePrototype = baseTemplate;
userListPagePrototype = userListPagePrototype.Replace(loginKey, userListTemplate);
userListPagePrototype = userListPagePrototype.Replace("Document", "Add Item");
userListPagePrototype = userListPagePrototype.Replace(js, "./serviceTest.js");
userListPagePrototype = userListPagePrototype.Replace(navKey, navTemplate);
userListPagePrototype = userListPagePrototype.Replace(contentKey, pageContentTemplate);
File.WriteAllText("../src/compiledSite/userListPagePrototype.html", userListPagePrototype);

var familyListPagePrototype = baseTemplate;
familyListPagePrototype = familyListPagePrototype.Replace(navKey, navTemplate);
familyListPagePrototype = familyListPagePrototype.Replace(loginKey, "");
familyListPagePrototype = familyListPagePrototype.Replace(contentKey, pageContentTemplate);
familyListPagePrototype = familyListPagePrototype.Replace(js, "./familyListUi.js");
familyListPagePrototype = familyListPagePrototype.Replace("Document", "Family list");
File.WriteAllText("../src/compiledSite/familyListPagePrototype.html", familyListPagePrototype);

//setting up some basic CSS
var cssPrototype = File.ReadAllText("config/base-style.css");
File.WriteAllText("../src/compiledSite/base-style.css", cssPrototype);