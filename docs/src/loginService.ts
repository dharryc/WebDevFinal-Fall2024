export const validateUser = async (user: string | null | undefined) => {
  const users = await fetch("http://localhost:5065/userList");
  const usersObj = await users.json();
  if (usersObj.includes(user)) {
    return true;
  }
  return false;
};
