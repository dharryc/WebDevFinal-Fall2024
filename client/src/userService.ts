import { rootUrl } from "./constants.js";
export const currentUser = async (userName: string | null) => {
  const userPromise = await fetch(`${rootUrl}/user/${userName}`);
  const userObj = await userPromise.json();
  return userObj;
};

export const addNewItem = async (
  mylink: string,
  mydescription: string,
  userName: string | null,
  id: number
) => {
  const thisItem = {
    link: mylink,
    description: mydescription,
  };
  await fetch(`${rootUrl}/user/${userName}/addItem/${id}`, {
    method: "POST",
    body: JSON.stringify(thisItem),
    headers: {
      "Content-Type": "application/json",
    },
  });
};

export const getUserItems = async (userName: string | null) => {
  const itemsPromise = await fetch(`${rootUrl}/${userName}/items`);
  const itemsObj = await itemsPromise.json();
  return itemsObj;
};

export const deleteItem = async (userName: string | null, itemId: any) => {
  await fetch(`${rootUrl}/${userName}/${itemId}/deleteItem`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  });
};

export const addDate = async (userName: string | null, birthDay: number) => {
  await fetch(`${rootUrl}/${userName}/${birthDay}/setBirthday`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
  });
};

export const addMoreDescription = async (userName: string | null, itemId: number, description: string) =>{
  await fetch(`${rootUrl}/${userName}/${itemId}/addDetails`, {
    method: "POST",
    body : JSON.stringify(description),
    headers: {
      "Content-Type": "application/json",
    },
  });
}