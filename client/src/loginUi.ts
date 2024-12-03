import { validateUser } from "./loginService.js";
const logInNode = document.getElementById("userLogin");

logInNode?.addEventListener("submit", async (ev) => {
  ev.preventDefault();
  const username = (
    document.getElementById("username") as any
  ).value.toLowerCase();
  if (await validateUser(username)) {
    window.location.href = `./userListPagePrototype.html?user=${username}`;
  }
  const errorText = (document.getElementById("error") as any)
  errorText.textContent = "I couldn't find that user."
});
