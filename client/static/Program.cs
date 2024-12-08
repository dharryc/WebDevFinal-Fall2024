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
var index = baseTemplate;
index = index.Replace(loginKey, logInTemplate);
index = index.Replace(navKey, "");
index = index.Replace(contentKey, "");
index = index.Replace(js, "./loginUi.js");
index = index.Replace("Document", "Login");
File.WriteAllText("../../docs/index.html", index);

//user creation page Prototype
var userCreationPrototype = baseTemplate;
userCreationPrototype = userCreationPrototype.Replace(loginKey, "");
userCreationPrototype = userCreationPrototype.Replace(contentKey, "<div id=\"userForm\"> </div>");
userCreationPrototype = userCreationPrototype.Replace(navKey, "");
userCreationPrototype = userCreationPrototype.Replace("Document", "New User");
userCreationPrototype = userCreationPrototype.Replace(js, "./userCreation.js");
File.WriteAllText("../../docs/userCreationPrototype.html", userCreationPrototype);

//list prototype creation (for users to see and edit their own list)
var userListPagePrototype = baseTemplate;
userListPagePrototype = userListPagePrototype.Replace(loginKey, userListTemplate);
userListPagePrototype = userListPagePrototype.Replace("Document", "Add Item");
userListPagePrototype = userListPagePrototype.Replace(js, "./userUi.js");
userListPagePrototype = userListPagePrototype.Replace(navKey, navTemplate);
userListPagePrototype = userListPagePrototype.Replace(contentKey, pageContentTemplate);
File.WriteAllText("../../docs/userListPagePrototype.html", userListPagePrototype);

var familyListPagePrototype = baseTemplate;
familyListPagePrototype = familyListPagePrototype.Replace(navKey, navTemplate);
familyListPagePrototype = familyListPagePrototype.Replace(loginKey, "");
familyListPagePrototype = familyListPagePrototype.Replace(contentKey, pageContentTemplate);
familyListPagePrototype = familyListPagePrototype.Replace(js, "./familyListUi.js");
familyListPagePrototype = familyListPagePrototype.Replace("Document", "Family list");
File.WriteAllText("../../docs/familyListPagePrototype.html", familyListPagePrototype);

//setting up some basic CSS
var cssPrototype = File.ReadAllText("config/base-style.css");
File.WriteAllText("../../docs/base-style.css", cssPrototype);