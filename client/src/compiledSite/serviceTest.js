let StringThatShowsYouCanDoSomething = "";
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
    inputButtonNode.addEventListener("click", (ev) => {
        StringThatShowsYouCanDoSomething += itemLinkNode.value;
        StringThatShowsYouCanDoSomething += " ";
        StringThatShowsYouCanDoSomething += itemTitleNode.value;
        itemTitleNode.value = "";
        itemLinkNode.value = "";
        console.log(StringThatShowsYouCanDoSomething);
    });
    inputParentNode.append(titleLabel, itemTitleNode, linkLabel, itemLinkNode, inputButtonNode);
    formNode?.append(inputParentNode);
};
console.log("this be workin");
formMaker();

const makeBlogPost = async (blog) => {
    await fetch("http://localhost:5065/newUser", {
      method: "POST",
      body: JSON.stringify(blog),
      headers: {
        "Content-Type": "application/json",
      },
    });
  };

  const Harry = {"userName":"Harry", "links" : [] , "descriptions" : [] , "id" : 55}
  await makeBlogPost(Harry)
export {};
//# sourceMappingURL=serviceTest.js.map