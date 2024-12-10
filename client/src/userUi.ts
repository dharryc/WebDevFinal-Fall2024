import {
  currentUser,
  addNewItem,
  getUserItems,
  deleteItem,
  addDate,
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

  submitButton.addEventListener("click", async (ev) => {
    await addDate(activeUserName, dayPicker.valueAsNumber)
  });

  countDownForm.append(dayLabel, dayPicker, submitButton);
  document.getElementById("form")?.append(countDownForm);
};

formMaker();
countDownAdder();

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
