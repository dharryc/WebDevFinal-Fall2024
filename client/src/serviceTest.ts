import { currentUser, addNewItem, getUserItems } from "./service.js";

interface user {
  userName: string;
  items: any[];
  id: Date;
}

const CardMaker = async (link: string, description: string) => {
  const contentNode = document.getElementById("pageContent");

  const cardWrapperNode = document.createElement("div");

  const titleNode = document.createElement("h3");
  titleNode.textContent = description;

  const linkNode = document.createElement("a");
  linkNode.textContent = link;

  linkNode.setAttribute("href", link);

  cardWrapperNode.append(titleNode, linkNode);
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
    await addNewItem(
      itemLinkNode.value,
      itemTitleNode.value,
      activeUser.userName
    );
    CardMaker(itemLinkNode.value, itemTitleNode.value);
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

formMaker();

const urlParams = new URLSearchParams(window.location.search);
const myParam = urlParams.get("user");
const activeUser = (await currentUser(myParam)) as unknown as user;
activeUser.items = await getUserItems(myParam);
activeUser.items.forEach((item) => {
  CardMaker(item.value.link, item.value.description);
});
