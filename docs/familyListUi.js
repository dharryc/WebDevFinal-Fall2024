import { togglePurchase, allUsers, getFamily } from "./familyListService.js";
const unprocessedFam = await allUsers();
const myFam = await getFamily(unprocessedFam);
const userNameInUrl = new URLSearchParams(window.location.search);
const activeUserName = userNameInUrl.get("user");
const parentNode = document.getElementById("pageContent");
const contentNode = document.createElement("div");
const CardMaker = (link, description, id, userNode, userName, purchased) => {
    var beenPurchased = purchased;
    const itemId = id;
    const parentUserName = userName;
    const cardWrapperNode = document.createElement("div");
    cardWrapperNode.classList.add("itemCard");
    const linkNode = document.createElement("a");
    linkNode.textContent = description;
    linkNode.setAttribute("href", link);
    const purchaseButton = document.createElement("button");
    if (!beenPurchased)
        purchaseButton.textContent = "Mark as purchased";
    else {
        purchaseButton.textContent = "Remove purchased status";
        cardWrapperNode.classList.add("purchased");
    }
    purchaseButton.addEventListener("click", async (ev) => {
        ev.preventDefault();
        await togglePurchase(id, parentUserName);
        beenPurchased = !beenPurchased;
        if (beenPurchased) {
            purchaseButton.textContent = "Remove purchased status";
            cardWrapperNode.classList.add("purchased");
        }
        else {
            purchaseButton.textContent = "Mark as purchased";
            cardWrapperNode.classList.remove("purchased");
        }
    });
    cardWrapperNode.append(linkNode, purchaseButton);
    userNode.append(cardWrapperNode);
};
const GenerateList = (familyList) => {
    contentNode.replaceChildren();
    familyList.forEach((user) => {
        if (user.items != null) {
            if (user.userName != activeUserName) {
                const userNode = document.createElement("div");
                userNode.setAttribute("id", user.userName);
                const userTitle = document.createElement("h2");
                userTitle.textContent =
                    user.userName.charAt(0).toUpperCase() + user.userName.slice(1);
                userNode.append(userTitle);
                user.items.forEach((item) => {
                    CardMaker(item.value.link, item.value.description, item.key, userNode, user.userName, item.value.purchased);
                });
                contentNode?.append(userNode);
            }
        }
    });
};
const makeFilter = () => {
    const filterWrapper = document.createElement("div");
    const filterLabel = document.createElement("label");
    filterLabel.setAttribute("for", "filterBar");
    filterLabel.textContent = "Search for person:";
    const filterInput = document.createElement("input");
    filterInput.setAttribute("id", "filterBar");
    filterInput.setAttribute("type", "text");
    filterInput.addEventListener("input", () => {
        const filteredFamily = myFam.filter((u) => u.userName.includes(filterInput.value));
        GenerateList(filteredFamily);
    });
    filterWrapper.append(filterLabel, filterInput);
    parentNode?.append(filterWrapper);
};
const familyList = document.getElementById("familyList");
familyList?.setAttribute("href", `./familyListPagePrototype.html?user=${activeUserName}`);
const userList = document.getElementById("userList");
userList?.setAttribute("href", `./userListPagePrototype.html?user=${activeUserName}`);
GenerateList(myFam);
makeFilter();
parentNode?.append(contentNode);
//# sourceMappingURL=familyListUi.js.map