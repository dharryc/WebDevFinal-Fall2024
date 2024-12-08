import { getUserItems } from "./userService.js";
import { rootUrl } from "./constants.js";
export const togglePurchase = async (
  itemId: number | null,
  userName: string | null
) => {
  await fetch(`${rootUrl}/${userName}/${itemId}`);
};

export const allUsers = async () => {
  const usersPromise = await fetch(`${rootUrl}/allUsers`);
  const userList = await usersPromise.json();
  return userList;
};

export const getFamily = async (famList: any) => {
  const returnList = Promise.all(
    famList.map(async (user: any) => {
      const modifiedUser = user;
      modifiedUser.items = await getUserItems(user.userName);
      return modifiedUser;
    })
  );
  return returnList;
};
