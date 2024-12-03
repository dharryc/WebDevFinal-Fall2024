export const validateUser = async (user: string | null | undefined) => {
  const users = await fetch("https://finalproject-35asp3tk.b4a.run/userList");
  const usersObj = await users.json();
  if (usersObj.includes(user)) {
    return true;
  }
  return false;
};
