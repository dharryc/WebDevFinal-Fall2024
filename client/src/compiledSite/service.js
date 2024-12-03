export const currentUser = async (userName) => {
    const userPromise = await fetch(`http://localhost:5065/user/${userName}`);
    const userObj = await userPromise.json();
    return userObj;
};
export const addNewItem = async (mylink, mydescription, userName) => {
    const thisItem = {
        link: mylink,
        description: mydescription,
    };
    await fetch(`http://localhost:5065/user/${userName}/addItem/${Date.now()}`, {
        method: "POST",
        body: JSON.stringify(thisItem),
        headers: {
            "Content-Type": "application/json",
        },
    });
};
export const getUserItems = async (userName) => {
    const itemsPromise = await fetch(`http://localhost:5065/${userName}/items`);
    const itemsObj = await itemsPromise.json();
    return itemsObj;
};
//# sourceMappingURL=service.js.map