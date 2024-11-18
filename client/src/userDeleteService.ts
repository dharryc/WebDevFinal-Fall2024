export const DeleteUser = async (username: any) => {
  await fetch(`http://localhost:5065/user/${username}/delete`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  });
};