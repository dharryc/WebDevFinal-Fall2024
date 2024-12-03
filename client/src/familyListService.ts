import { getUserItems } from "./service.js";
export const togglePurchase = async (
  itemId: number | null,
  userName: string | null
) => {
  await fetch(`https://finalproject-35asp3tk.b4a.run/${userName}/${itemId}`);
};

export const allUsers = async () => {
  const usersPromise = await fetch(`https://finalproject-35asp3tk.b4a.run/allUsers`);
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
