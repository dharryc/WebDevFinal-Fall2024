import {
  currentUser,
  addNewItem,
  getUserItems,
  deleteItem,
} from "./userService.js";

const userNameInUrl = new URLSearchParams(window.location.search);
const activeUserName = userNameInUrl.get("user");

const CardMaker = (link: string, description: string, id: number) => {
  const contentNode = document.getElementById("pageContent");

  const cardWrapperNode = document.createElement("div");
  cardWrapperNode.setAttribute("id", id.toString());
  cardWrapperNode.classList.add("itemCard");

  const linkNode = document.createElement("a");
  linkNode.textContent = description;

  linkNode.setAttribute("href", link);

  const deleteButton = document.createElement("button");
  deleteButton.textContent = "Delete Item";
  deleteButton.addEventListener("click", async () => {
    await deleteItem(activeUserName, id);
    await cardGenerator();
    contentNode?.removeChild(cardWrapperNode);
  });

  cardWrapperNode.append(linkNode, deleteButton);
  contentNode?.append(cardWrapperNode);
};

const formMaker = () => {
  const formNode = document.getElementById("form");

  const inputParentNode = document.createElement("form");
  inputParentNode.setAttribute("action", "submit");
  inputParentNode.setAttribute("id", "itemForm");

  const itemTitleNode = document.createElement("input");
  itemTitleNode.setAttribute("type", "text");
  itemTitleNode.setAttribute("id", "title");
  itemTitleNode.setAttribute("name", "title");

  const titleLabel = document.createElement("label");
  titleLabel.setAttribute("for", "title");
  titleLabel.textContent = "Description of your item:";

  const itemLinkNode = document.createElement("input");
  itemLinkNode.setAttribute("type", "text");
  itemTitleNode.setAttribute("id", "link");
  itemTitleNode.setAttribute("name", "link");

  const linkLabel = document.createElement("label");
  linkLabel.setAttribute("for", "link");
  linkLabel.textContent = "Link for your item:";

  const inputButtonNode = document.createElement("button");
  inputButtonNode.textContent = "Add Item";

  inputParentNode.addEventListener("submit", (ev) => {
    ev.preventDefault();
  });

  inputButtonNode.addEventListener("click", async (ev) => {
    const id = Date.now();
    await addNewItem(
      itemLinkNode.value,
      itemTitleNode.value,
      activeUserName,
      id
    );
    CardMaker(itemLinkNode.value, itemTitleNode.value, id);
    itemTitleNode.value = "";
    itemLinkNode.value = "";
  });

  inputParentNode.append(
    titleLabel,
    itemTitleNode,
    linkLabel,
    itemLinkNode,
    inputButtonNode
  );
  formNode?.append(inputParentNode);
};

const countDownAdder = () => {
  const countDownForm = document.createElement("form");
  countDownForm.setAttribute("action", "submit");
  countDownForm.setAttribute("id", "dateForm");
  countDownForm.addEventListener("submit", (ev) => {
    ev.preventDefault();
  });

  const dayPicker = document.createElement("input");
  dayPicker.setAttribute("type", "date");
  dayPicker.setAttribute("id", "eventDate");
  dayPicker.setAttribute("name", "eventDate");

  const dayLabel = document.createElement("label");
  dayLabel.setAttribute("for", "eventDate");
  dayLabel.textContent = "Set countdown date: ";

  const submitButton = document.createElement("button");
  submitButton.textContent = "Confirm date";

  countDownForm.append(dayLabel, dayPicker, submitButton);
  document.getElementById("form")?.append(countDownForm);
};

const existingCountDown = () =>{
  const dateWrapperNode = document.createElement("section")
  // Set the date we're counting down to
  var countDownDate = new Date("Jan 5, 2030 15:37:25").getTime();
  
  // Update the count down every 1 second
  var x = setInterval(function() {
    
    // Get today's date and time
    var now = new Date().getTime();
    
    // Find the distance between now and the count down date
    var distance = countDownDate - now;
    
    // Time calculations for days, hours, minutes and seconds
    var days = Math.floor(distance / (1000 * 60 * 60 * 24));
    var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = Math.floor((distance % (1000 * 60)) / 1000);
    
    // Output the result in an element with id="demo"
    dateWrapperNode.textContent = days + "d " + hours + "h "
    + minutes + "m " + seconds + "s ";
    
    // If the count down is over, write some text 
    if (distance < 0) {
      clearInterval(x);
      dateWrapperNode.textContent = "EXPIRED";
    }
  }, 1000);
  document.getElementById("form")?.append(dateWrapperNode);
}

formMaker();
countDownAdder();
existingCountDown();

const cardGenerator = async () => {
  document.getElementById("pageContent")?.replaceChildren();
  const activeUser = (await currentUser(activeUserName)) as any;
  activeUser.items = await getUserItems(activeUserName);
  activeUser.items?.forEach((item: any) => {
    CardMaker(item.value.link, item.value.description, item.key);
  });
};

const familyList = document.getElementById("familyList");
familyList?.setAttribute(
  "href",
  `./familyListPagePrototype.html?user=${activeUserName}`
);
const userList = document.getElementById("userList");
userList?.setAttribute(
  "href",
  `./userListPagePrototype.html?user=${activeUserName}`
);

cardGenerator();