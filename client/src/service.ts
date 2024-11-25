export const currentUser = async (userName: string | null) => {
  const userPromise = await fetch(`http://localhost:5065/user/${userName}`);
  const userObj = await userPromise.json();
  return userObj;
};
