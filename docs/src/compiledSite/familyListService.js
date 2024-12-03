import { getUserItems } from "./service.js";
export const togglePurchase = async (itemId, userName) => {
    await fetch(`http://localhost:5065/${userName}/${itemId}`);
};
export const allUsers = async () => {
    const usersPromise = await fetch(`http://localhost:5065/allUsers`);
    const userList = await usersPromise.json();
    return userList;
};
export const getFamily = async (famList) => {
    const returnList = Promise.all(famList.map(async (user) => {
        const modifiedUser = user;
        modifiedUser.items = await getUserItems(user.userName);
        return modifiedUser;
    }));
    return returnList;
};
//# sourceMappingURL=familyListService.js.map