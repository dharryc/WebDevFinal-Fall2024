import { togglePurchase, allUsers, getFamily } from "./familyListService.js";
const unprocessedFam = await allUsers();
const myFam = await getFamily(unprocessedFam);
const userNameInUrl = new URLSearchParams(window.location.search);
const activeUserName = userNameInUrl.get("user");
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
    purchaseButton.textContent = "Mark as purchased";
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
const GenerateList = () => {
    const contentNode = document.getElementById("pageContent");
    myFam.forEach((user) => {
        if (user.userName != activeUserName) {
            const userNode = document.createElement("div");
            userNode.setAttribute("id", user.userName);
            const userTitle = document.createElement("h2");
            userTitle.textContent =
                user.userName.charAt(0).toUpperCase() + user.userName.slice(1);
            userNode.append(userTitle);
            user.items.forEach((item) => {
                CardMaker(item.value.link, item.value.description, item.key, userNode, user.userName, item.purchased);
            });
            contentNode?.append(userNode);
        }
    });
};
GenerateList();
const familyList = document.getElementById("familyList");
familyList?.setAttribute("href", `./familyListPagePrototype.html?user=${activeUserName}`);
const userList = document.getElementById("userList");
userList?.setAttribute("href", `./userListPagePrototype.html?user=${activeUserName}`);
//# sourceMappingURL=familyListUi.js.map