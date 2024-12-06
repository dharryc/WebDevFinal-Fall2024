import { rootUrl } from "./constants.js";

export const DeleteUser = async (username: any) => {
  await fetch(`${rootUrl}/user/${username}/delete`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  });
};