import { rootUrl } from "./constants.js";
export const validateUser = async (user: string | null | undefined) => {
  const users = await fetch(`${rootUrl}/userList`);
  const usersObj = await users.json();
  if (usersObj.includes(user)) {
    return true;
  }
  return false;
};
