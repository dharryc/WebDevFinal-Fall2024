export const makeUser = async (user: any) => {
  await fetch("http://localhost:5133/blogs", {
    method: "POST",
    body: JSON.stringify(user),
    headers: {
      "Content-Type": "application/json",
    },
  });
};
