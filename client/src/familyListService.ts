import { getUserItems } from "./service.js";
export const togglePurchase = async (
  itemId: number | null,
  userName: string | null
) => {
  await fetch(`http://localhost:5065/${userName}/${itemId}`);
};

export const allUsers = async () => {
  const usersPromise = await fetch(`http://localhost:5065/allUsers`);
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
